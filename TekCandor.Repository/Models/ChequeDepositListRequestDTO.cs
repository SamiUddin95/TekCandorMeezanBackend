namespace TekCandor.Repository.Models
{
    public class ChequeDepositListRequestDTO
    {
        public string? Branch { get; set; }
        public string? AccountNumber { get; set; }
        public string? ChequeNumber { get; set; }
        public string? HubCode { get; set; }
        public bool? ServiceRun { get; set; }
        public string? Status { get; set; }
        public string? InstrumentNo { get; set; }
        public string? CycleCode { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortColumn { get; set; } = "Date";
        public string? SortDirection { get; set; } = "DESC";
    }
}
