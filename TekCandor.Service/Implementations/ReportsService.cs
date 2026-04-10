using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class ReportsService:IReportService
    {
        private readonly IReportRepository _repository;

        public ReportsService(IReportRepository repository)
        {
            _repository = repository;
        }
     
        public async Task<PagedResult<BranchWiseReportDTO>> GetBranchWiseReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? chequeNumber, string? accountNumber, string? hubCode, string? status)

        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var chequeQuery = await _repository.GetChequeQueryableAsync();
            var returnReasonQuery = await _repository.GetReturnReasonQueryableAsync();

            var query = from c in chequeQuery
                        join r in returnReasonQuery
                        on c.Returnreasone equals r.Code into rr
                        from r in rr.DefaultIfEmpty()
                        where !c.IsDeleted
                        select new
                        {
                            c,
                            ReturnReasonName = r != null ? r.Name : null
                        };

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var toDateEnd = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.c.Date < toDateEnd);
            }

            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                query = query.Where(x => x.c.ReceiverBranchCode == branchCode);
            }

            if (!string.IsNullOrWhiteSpace(chequeNumber))
            {
                query = query.Where(x => x.c.ChequeNumber.Contains(chequeNumber));
            }

            if (!string.IsNullOrWhiteSpace(accountNumber))
            {
                query = query.Where(x => x.c.AccountNumber.Contains(accountNumber));
            }

            if (!string.IsNullOrWhiteSpace(hubCode))
            {
                query = query.Where(x => x.c.AuthorizerId == hubCode);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.c.status == status);
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new BranchWiseReportDTO
            {
                Date = x.c.TrProcORRecTime,
                ChequeNumber = x.c.ChequeNumber,
                AccountNumber = x.c.AccountNumber,
                AccountTitle = x.c.AccountTitle,
                Amount = x.c.Amount,
                ReturnReason = x.ReturnReasonName,
                HubCode = x.c.AuthorizerId,
            });

            return new PagedResult<BranchWiseReportDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
      
        public async Task<PagedResult<CBCReportDTO>> GetCBCReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? accountNumber, string? status, string? hub)

        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var chequeQuery = await _repository.GetChequeQueryableAsync();
            var returnReasonQuery = await _repository.GetReturnReasonQueryableAsync();

            var settingValue = await _repository.GetSettingValueAsync("generalsettings.callbackamount");

            decimal callbackAmount = 0;

            if (!string.IsNullOrEmpty(settingValue))
            {
                decimal.TryParse(settingValue, out callbackAmount);
            }

            var query = from c in chequeQuery
                        join r in returnReasonQuery
                        on c.Returnreasone equals r.Code into rr
                        from r in rr.DefaultIfEmpty()


                        where !c.IsDeleted
                        select new
                        {
                            c,
                            ReturnReasonName = r != null ? r.Name : null,

                        };

            query = query.Where(x => x.c.Amount >= callbackAmount);

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var toDateEnd = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.c.Date < toDateEnd);
            }

            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                query = query.Where(x => x.c.ReceiverBranchCode.Contains(branchCode));
            }
            if (!string.IsNullOrWhiteSpace(accountNumber))
            {
                query = query.Where(x => x.c.AccountNumber.Contains(accountNumber));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.c.status == status);
            }
            if (!string.IsNullOrWhiteSpace(hub))
            {
                query = query.Where(x => x.c.AuthorizerId == hub);
            }
         


            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new CBCReportDTO
            {
                Date = x.c.Date,
                CycleCode = x.c.CycleCode,
                HubCode = x.c.AuthorizerId,
                CoreFTId = x.c.CoreFTId,
                Amount = x.c.Amount,
                ChequeNumber = x.c.ChequeNumber,
                SenderBranchCode = x.c.SenderBranchCode,
                AccountNumber = x.c.AccountNumber,
                IBAN = x.c.IBAN,
                AccountTitle = x.c.AccountTitle,
                Remarks = x.c.Remarks,
                BranchStaffId = x.c.BranchStaffId,
                CBCStatus=x.c.status
            });

            return new PagedResult<CBCReportDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        
        public async Task<PagedResult<FinalReportDTO>> GetFinalReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? cycleCode)

        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var chequeQuery = await _repository.GetChequeQueryableAsync();
            var returnReasonQuery = await _repository.GetReturnReasonQueryableAsync();


            var query = from c in chequeQuery
                        join r in returnReasonQuery
                        on c.Returnreasone equals r.Code into rr
                        from r in rr.DefaultIfEmpty()
                        where !c.IsDeleted
                        select new
                        {
                            c,
                            ReturnReasonName = r != null ? r.Name : null
                        };

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var toDateEnd = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.c.Date < toDateEnd);
            }

            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                query = query.Where(x => x.c.ReceiverBranchCode.Contains(branchCode));
            }

            if (!string.IsNullOrWhiteSpace(cycleCode))
            {
                query = query.Where(x => x.c.CycleCode == cycleCode);
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new FinalReportDTO
            {
                status = x.c.status,
                ChequeNumber = x.c.ChequeNumber,
                Amount = x.c.Amount,

            });

            return new PagedResult<FinalReportDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
       
        public async Task<PagedResult<ReturnMemoReportDTO>> GetReturnMemoReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? chequenumber,string? accountnumber)

        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var chequeQuery = await _repository.GetChequeQueryableAsync();
            var returnReasonQuery = await _repository.GetReturnReasonQueryableAsync();

           
            var query = from c in chequeQuery
                        join r in returnReasonQuery
                        on c.Returnreasone equals r.Code into rr
                        from r in rr.DefaultIfEmpty()
                        where !c.IsDeleted
                        select new
                        {
                            c,
                            ReturnReasonName = r != null ? r.Name : null
                        };

          
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var toDateEnd = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.c.Date < toDateEnd);
            }


            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                query = query.Where(x => x.c.ReceiverBranchCode.Contains(branchCode));
            }
            if (!string.IsNullOrWhiteSpace(chequenumber))
            {
                query = query.Where(x => x.c.ChequeNumber.Contains(chequenumber));
            }
            if (!string.IsNullOrWhiteSpace(accountnumber))
            {
                query = query.Where(x => x.c.AccountNumber.Contains(accountnumber));
            }


            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new ReturnMemoReportDTO
            {
                AccountNumber = x.c.AccountNumber,
                ChequeNumber = x.c.ChequeNumber,
                Amount = x.c.Amount,
                AccountTitle = x.c.AccountTitle,
                CycleCode = x.c.CycleCode,
                Returnreasone = x.ReturnReasonName,
                Date = x.c.Date,
                SenderBranchCode = x.c.SenderBranchCode
            });

            return new PagedResult<ReturnMemoReportDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
      
        public async Task<PagedResult<ReturnRegisterDTO>> GetReturnRegisterReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? branchCode, string? chequeNumber, string? accountNumber, string? hubCode, string? status)

        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var chequeQuery = await _repository.GetChequeQueryableAsync();
            var branchQuery = await _repository.GetBranchQueryableAsync();
            var cycleQuery = await _repository.GetCycleQueryableAsync();
            var bankQuery = await _repository.GetBankQueryableAsync();
            var returnReasonQuery = await _repository.GetReturnReasonQueryableAsync();

            var query = from c in chequeQuery
                        join b in branchQuery on c.ReceiverBranchCode equals b.NIFTBranchCode
                        join cy in cycleQuery on c.CycleCode equals cy.Code
                        join bk in bankQuery on c.SenderBankCode equals bk.Code
                        join rr in returnReasonQuery on c.Returnreasone equals rr.Code
                        where !c.IsDeleted && c.status == "R"
                        select new
                        {
                            c,
                            BranchName = b.Name,
                            CycleName = cy.Name,
                            BankName = bk.Name,
                            ReturnReasonName = rr.Name
                        };


            if (fromDate.HasValue)
            {
                query = query.Where(x => x.c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var toDateEnd = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.c.Date < toDateEnd);
            }

            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                query = query.Where(x => x.c.ReceiverBranchCode == branchCode);
            }

            if (!string.IsNullOrWhiteSpace(chequeNumber))
            {
                query = query.Where(x => x.c.ChequeNumber.Contains(chequeNumber));
            }

            if (!string.IsNullOrWhiteSpace(accountNumber))
            {
                query = query.Where(x => x.c.AccountNumber.Contains(accountNumber));
            }

            if (!string.IsNullOrWhiteSpace(hubCode))
            {
                query = query.Where(x => x.c.AuthorizerId == hubCode);
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new ReturnRegisterDTO
            {
                AccountNumber = x.c.AccountNumber,
                ChequeNumber = x.c.ChequeNumber,
                Amount = x.c.Amount, 
                AccountTitle = x.c.AccountTitle,

                ReceiverBranchCode = x.BranchName,
                CycleCode = x.CycleName,
                SenderBankCode = x.BankName,

                ApproverId = x.c.ApproverId,
                CoreFTId = x.c.CoreFTId,
                TrProcORRecTime = x.c.TrProcORRecTime
            });

            return new PagedResult<ReturnRegisterDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        
        public async Task<PagedResult<ClearingLogReportDTO>> GetClearingLogReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? clearingCycle, string? hub)


        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var chequeQuery = await _repository.GetChequeQueryableAsync();
            var branchQuery = await _repository.GetBranchQueryableAsync();
            var cycleQuery = await _repository.GetCycleQueryableAsync();
            var bankQuery = await _repository.GetBankQueryableAsync();
            var returnReasonQuery = await _repository.GetReturnReasonQueryableAsync();

            var query = from c in chequeQuery
                        join b in branchQuery on c.ReceiverBranchCode equals b.NIFTBranchCode
                        join cy in cycleQuery on c.CycleCode equals cy.Code
                        join bk in bankQuery on c.SenderBankCode equals bk.Code
                        join rr in returnReasonQuery on c.Returnreasone equals rr.Code into rrg
                        from rr in rrg.DefaultIfEmpty() 
                        where !c.IsDeleted && c.CoreFTId != null
                        select new
                        {
                            c,
                            BranchName = b.Name,
                            CycleName = cy.Name,
                            BankName = bk.Name,
                            ReturnReasonName = rr != null ? rr.Name : null
                        };

           
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var toDateEnd = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.c.Date < toDateEnd);
            }

           
            if (!string.IsNullOrWhiteSpace(clearingCycle))
            {
                query = query.Where(x => x.CycleName.Contains(clearingCycle));
            }

            
            if (!string.IsNullOrWhiteSpace(hub))
            {
                query = query.Where(x => x.c.CityCode.Contains(hub));
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new ClearingLogReportDTO
            {
                CoreFTId = x.c.CoreFTId,
                AccountNumber = x.c.AccountNumber,
                OldAccount = x.c.OldAccount,
                Amount = x.c.Amount,
                ApproverId = x.c.ApproverId,
                ChequeNumber = x.c.ChequeNumber,
                AuthorizerId = x.c.AuthorizerId,
                ReceiverBankCode = x.c.ReceiverBankCode,
                ReceiverBranchCode = x.c.ReceiverBranchCode,
                BranchStaffId = x.c.BranchStaffId,
                Remarks = x.c.Remarks,
                CityCode = x.c.CityCode,
                TrProcORRecTime = x.c.TrProcORRecTime,
                SenderBranchCode = x.c.SenderBranchCode,
                TrRecTimeBranch = x.c.TrRecTimeBranch,
                BranchRemarks = x.c.BranchRemarks
            });

            return new PagedResult<ClearingLogReportDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<PagedResult<InwardClearingReportDTO>> GetInwardClearingReportAsync(int pageNumber, int pageSize, DateTime? fromDate, DateTime? toDate, string? status, string? branchCode, string? hub)

        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var chequeQuery = await _repository.GetChequeQueryableAsync();
            var branchQuery = await _repository.GetBranchQueryableAsync();
            var cycleQuery = await _repository.GetCycleQueryableAsync();
            var bankQuery = await _repository.GetBankQueryableAsync();

            var query = from c in chequeQuery
                        join b in branchQuery on c.ReceiverBranchCode equals b.NIFTBranchCode
                        join cy in cycleQuery on c.CycleCode equals cy.Code
                        join bk in bankQuery on c.SenderBankCode equals bk.Code into bkg
                        from bk in bkg.DefaultIfEmpty() 
                        where !c.IsDeleted
                        select new
                        {
                            c,
                            BranchName = b.Name,
                            CycleName = cy.Name,
                            BankName = bk != null ? bk.Name : null
                        };

            
            if (string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.c.status == "A");
            }
            else
            {
                query = query.Where(x => x.c.status == status);
            }

            
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.c.Date >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var toDateEnd = toDate.Value.Date.AddDays(1);
                query = query.Where(x => x.c.Date < toDateEnd);
            }


            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                query = query.Where(x => x.c.ReceiverBranchCode.Contains(branchCode));
            }

            if (!string.IsNullOrWhiteSpace(hub))
            {
                query = query.Where(x => x.c.CityCode.Contains(hub));
            }

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.c.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = data.Select(x => new InwardClearingReportDTO
            {
                CoreFTId = x.c.CoreFTId,
                AccountNumber = x.c.AccountNumber,
                OldAccount = x.c.OldAccount,
                Amount = x.c.Amount,
                ApproverId = x.c.ApproverId,
                ChequeNumber = x.c.ChequeNumber,
                AuthorizerId = x.c.AuthorizerId,
                TrProcORRecTime = x.c.TrProcORRecTime
            });

            return new PagedResult<InwardClearingReportDTO>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
