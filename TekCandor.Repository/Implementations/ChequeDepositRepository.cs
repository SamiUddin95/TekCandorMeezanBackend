using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using TekCandor.Repository.Entities;
using TekCandor.Repository.Entities.Data;
using TekCandor.Repository.Interfaces;
using TekCandor.Repository.Models;

namespace TekCandor.Repository.Implementations
{
    public class ChequeDepositRepository : IChequeDepositRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        private static readonly HashSet<string> AllowedSortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "Date", "ChequeNumber", "AccountNumber", "Amount",
                "Status", "HubCode", "CycleCode", "InstrumentNo"
            };

        public ChequeDepositRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(ChequedepositInfo chequeDeposit)
        {
            _context.chequedepositInformation.Add(chequeDeposit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<ChequedepositInfo> chequeDeposits)
        {
            await _context.chequedepositInformation.AddRangeAsync(chequeDeposits);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string> GetHubCodeAsync(string receiverBranchCode)
        {
            var branch = await _context.Branch
                .FirstOrDefaultAsync(b => b.Code == receiverBranchCode && !b.IsDeleted);

            if (branch == null) return string.Empty;

            var hub = await _context.Hub
                .FirstOrDefaultAsync(h => h.Id == branch.HubId && !h.IsDeleted);

            return hub?.Code ?? string.Empty;
        }

        //List
        public async Task<(IEnumerable<ChequeDepositListResponseDTO> Data, int TotalCount)> GetChequeDepositListAsync(
           ChequeDepositListRequestDTO request,
           long userId,
           string branchOrHub,
           string? hubIds,
           string? branchCodes,
           CancellationToken cancellationToken = default)
        {
            var sqlParams = new List<SqlParameter>();
            var where = new StringBuilder();

            // Branch/Hub scope filter
            if (branchOrHub == "HubWise")
            {
                if (!string.IsNullOrWhiteSpace(hubIds) && !string.IsNullOrWhiteSpace(branchCodes))
                    where.Append($" AND ReceiverBranchCode IN ({branchCodes}) AND Hub.ID IN ({hubIds})");
                else
                    where.Append(" AND ReceiverBranchCode IN ('') AND Hub.ID IN (0)");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(branchCodes))
                    where.Append($" AND ReceiverBranchCode IN ({branchCodes})");
                else
                    where.Append(" AND ReceiverBranchCode IN ('')");
            }

            // User-supplied filters
            if (!string.IsNullOrWhiteSpace(request.Branch))
            {
                where.Append(" AND ChequeDepositInformation.ReceiverBranchCode = @Branch");
                sqlParams.Add(new SqlParameter("@Branch", request.Branch));
            }
            if (!string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                where.Append(" AND ChequeDepositInformation.AccountNumber = @AccountNumber");
                sqlParams.Add(new SqlParameter("@AccountNumber", request.AccountNumber));
            }
            if (!string.IsNullOrWhiteSpace(request.ChequeNumber))
            {
                where.Append(" AND ChequeDepositInformation.ChequeNumber = @ChequeNumber");
                sqlParams.Add(new SqlParameter("@ChequeNumber", request.ChequeNumber));
            }
            if (!string.IsNullOrWhiteSpace(request.HubCode))
            {
                where.Append(" AND ChequeDepositInformation.HubCode = @HubCode");
                sqlParams.Add(new SqlParameter("@HubCode", request.HubCode));
            }
            if (request.ServiceRun.HasValue)
            {
                where.Append(" AND ChequeDepositInformation.serviceRun = @ServiceRun");
                sqlParams.Add(new SqlParameter("@ServiceRun", request.ServiceRun.Value));
            }
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                where.Append(" AND ChequeDepositInformation.status = @Status");
                sqlParams.Add(new SqlParameter("@Status", request.Status));
            }
            if (!string.IsNullOrWhiteSpace(request.InstrumentNo))
            {
                where.Append(" AND ChequeDepositInformation.InstrumentNo = @InstrumentNo");
                sqlParams.Add(new SqlParameter("@InstrumentNo", request.InstrumentNo));
            }
            if (!string.IsNullOrWhiteSpace(request.CycleCode))
            {
                where.Append(" AND ChequeDepositInformation.CycleCode = @CycleCode");
                sqlParams.Add(new SqlParameter("@CycleCode", request.CycleCode));
            }

            where.Append(" AND ChequeDepositInformation.status IN ('P','AR','RRA','MAR')");
            where.Append(" AND ChequeDepositInformation.Date = CAST(CAST(GETDATE() AS date) AS datetime)");
            where.Append(" AND ChequeDepositInformation.IsDeleted = 0");

            var sortColumn = AllowedSortColumns.Contains(request.SortColumn ?? "") ? request.SortColumn : "Date";
            var sortDir = string.Equals(request.SortDirection, "ASC", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";

            var skip = (request.Page - 1) * request.PageSize;
            sqlParams.Add(new SqlParameter("@Skip", skip));
            sqlParams.Add(new SqlParameter("@Take", request.PageSize));

            var dataSql = $"""
                SELECT
                    ChequeDepositInformation.Id,
                    ChequeDepositInformation.Date,
                    Bank.Name                          AS SenderBankCode,
                    ChequeDepositInformation.ReceiverBranchCode,
                    ChequeDepositInformation.ChequeNumber,
                    ChequeDepositInformation.AccountNumber,
                    ChequeDepositInformation.TransactionCode,
                    ClearingStatuses.Text              AS Status,
                    FORMAT(
                        CAST(
                            CASE WHEN ISNUMERIC(ChequeDepositInformation.AccountBalance) = 1
                                      AND ChequeDepositInformation.AccountBalance NOT LIKE '%.%'
                                      AND ChequeDepositInformation.AccountBalance NOT LIKE '%[^0-9]%'
                                 THEN ChequeDepositInformation.AccountBalance
                                 ELSE '0'
                            END AS bigint
                        ), '##,###,##,###.00'
                    )                                  AS AccountBalance,
                    ChequeDepositInformation.AccountStatus,
                    Currency.Name                      AS Currency,
                    Hub.Name + '-' + Hub.Code          AS HubCode,
                    Cycle.Name                         AS CycleCode,
                    Instruments.Name                   AS InstrumentNo,
                    ChequeDepositInformation.BranchRemarks,
                    ChequeDepositInformation.Error,
                    ChequeDepositInformation.Callbacksend,
                    ChequeDepositInformation.Export
                FROM ChequeDepositInformation WITH (NOLOCK)
                INNER JOIN Cycles          ON Cycles.Code          = ChequeDepositInformation.CycleCode
                INNER JOIN Instruments    ON Instruments.Code     = ChequeDepositInformation.InstrumentNo
                INNER JOIN ClearingStatuses ON ClearingStatuses.Value = ChequeDepositInformation.status
                INNER JOIN Branch         ON Branch.NIFTBranchCode = ChequeDepositInformation.ReceiverBranchCode
                INNER JOIN Hub            ON Branch.HubId         = Hub.Id
                                         AND Hub.Code             = ChequeDepositInformation.HubCode
                LEFT  JOIN ReturnReason   ON ReturnReason.Code    = ChequeDepositInformation.Returnreasone
                LEFT  JOIN Bank           ON Bank.Code            = ChequeDepositInformation.SenderBankCode
                LEFT  JOIN PostingRestriction ON ChequeDepositInformation.PostRestriction = PostingRestriction.Code
                LEFT  JOIN Currency       ON Currency.CurrencyCode = ChequeDepositInformation.Currency
                WHERE 1 = 1
                {where}
                ORDER BY ChequeDepositInformation.{sortColumn} {sortDir}
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                """;

            var countSql = $"""
                SELECT COUNT(1)
                FROM ChequeDepositInformation WITH (NOLOCK)
                INNER JOIN Cycles          ON Cycles.Code          = ChequeDepositInformation.CycleCode
                INNER JOIN Instruments    ON Instruments.Code     = ChequeDepositInformation.InstrumentNo
                INNER JOIN ClearingStatuses ON ClearingStatuses.Value = ChequeDepositInformation.status
                INNER JOIN Branch         ON Branch.NIFTBranchCode = ChequeDepositInformation.ReceiverBranchCode
                INNER JOIN Hub            ON Branch.HubId         = Hub.Id
                                         AND Hub.Code             = ChequeDepositInformation.HubCode
                LEFT  JOIN ReturnReason   ON ReturnReason.Code    = ChequeDepositInformation.Returnreasone
                LEFT  JOIN Bank           ON Bank.Code            = ChequeDepositInformation.SenderBankCode
                LEFT  JOIN PostingRestriction ON ChequeDepositInformation.PostRestriction = PostingRestriction.Code
                LEFT  JOIN Currency       ON Currency.CurrencyCode = ChequeDepositInformation.Currency
                WHERE 1 = 1
                {where}
                """;

            // Build a separate param list for count query (no @Skip/@Take)
            var countParams = sqlParams
                .Where(p => p.ParameterName != "@Skip" && p.ParameterName != "@Take")
                .Select(p => new SqlParameter(p.ParameterName, p.Value))
                .ToArray();

            var data = await _context.ChequeDepositListResults
                .FromSqlRaw(dataSql, sqlParams.ToArray<object>())
                .ToListAsync(cancellationToken);

            var totalCount = await _context.Database
                .SqlQueryRaw<int>(countSql, countParams.ToArray<object>())
                .FirstOrDefaultAsync(cancellationToken);

            return (data, totalCount);
        }
    }
}
