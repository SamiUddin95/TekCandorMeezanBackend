using System;

namespace TekCandor.Repository.Entities.Outward
{
    public class NiftUploadStaging
    {
        public long Id { get; set; }
        public string? FileName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? ChequeNo { get; set; }
        public decimal? Amount { get; set; }
        public string? MICR { get; set; }
        public string? Status { get; set; }
        public string? ReturnCode { get; set; }
        public string? ReturnReason { get; set; }
        public bool? IsProcessed { get; set; }
    }
}
