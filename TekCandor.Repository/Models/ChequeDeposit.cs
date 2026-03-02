using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Repository.Models
{
    public class ChequeDeposit
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string SequenceNumber { get; set; }
        public string AccountNumber { get; set; }
        public string ChequeNumber { get; set; }
        public string InstrumentNo { get; set; }
        public string AccountBalance { get; set; }
        public string AccountTitle { get; set; }
        public decimal Amount { get; set; }
        public string PoStatus { get; set; }
    }
}
