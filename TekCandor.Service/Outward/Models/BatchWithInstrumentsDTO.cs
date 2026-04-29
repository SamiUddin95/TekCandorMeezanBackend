using System.Collections.Generic;

namespace TekCandor.Service.Outward.Models
{
    public class BatchWithInstrumentsDTO
    {
        public BatchDTO Batch { get; set; } = new BatchDTO();
        public List<ChequeInfoDTO> Instruments { get; set; } = new List<ChequeInfoDTO>();
    }
}
