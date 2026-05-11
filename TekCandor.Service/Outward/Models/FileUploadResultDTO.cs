using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace TekCandor.Service.Outward.Models
{
    public class FileUploadRequestDTO
    {
        public IFormFile File { get; set; }
    }

    public class FileUploadResultDTO
    {
        public string BatchId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public int TotalInstruments { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public List<ChequeInfoDTO> Cheques { get; set; }
    }
}
