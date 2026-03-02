using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Models
{
    public class ChequeDepositResponse
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string StatusText { get; set; }
        public string ImgF { get; set; }
        public string ImgR { get; set; }
        public string ImgU { get; set; }
        public string ChequeNumber { get; set; }
        public string AccountNumber { get; set; }
        public string AccountTitle { get; set; }
        public decimal Amount { get; set; }
        public string AccountBalance { get; set; }
        public string ErrorInFields { get; set; }
        public byte[][]? Signature { get; set; }
    }
}
