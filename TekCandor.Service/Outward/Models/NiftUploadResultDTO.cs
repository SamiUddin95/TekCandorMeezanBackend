using System.Collections.Generic;

namespace TekCandor.Service.Outward.Models
{
    public class NiftUploadResultDTO
    {
        public List<NiftRecordDTO> MatchedRecords { get; set; } = new List<NiftRecordDTO>();
        public List<NiftRecordDTO> UnmatchedRecords { get; set; } = new List<NiftRecordDTO>();
        public NiftSummaryDTO Summary { get; set; } = new NiftSummaryDTO();
    }

    public class NiftRecordDTO
    {
        public long? ChequeInfoId { get; set; }
        public long NiftStagingId { get; set; }
        public string? ChequeNo { get; set; }
        public string? BranchName { get; set; }
        public decimal? Amount { get; set; }
        public string? Date { get; set; }
        public string? Discrepancy { get; set; }
    }

    public class NiftSummaryDTO
    {
        public int TotalLodgement { get; set; }
        public int Matched { get; set; }
        public int Unmatched { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
