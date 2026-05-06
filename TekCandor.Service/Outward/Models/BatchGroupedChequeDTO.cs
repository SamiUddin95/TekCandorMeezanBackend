using System.Collections.Generic;

namespace TekCandor.Service.Outward.Models
{
    public class BatchGroupedChequeDTO
    {
        public string BatchId { get; set; } = string.Empty;
        //public string? Branch { get; set; }
        public string? BranchName { get; set; }
        //public int TotalInstruments { get; set; }
        //public decimal TotalAmount { get; set; }
        //public string? Status { get; set; }
        public List<ChequeInfoDTO> Items { get; set; } = new List<ChequeInfoDTO>();
    }
}
