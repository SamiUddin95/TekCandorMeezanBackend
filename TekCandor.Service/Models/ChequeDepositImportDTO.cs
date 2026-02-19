using System;

namespace TekCandor.Service.Models
{
    public class ChequeDepositImportDTO
    {
        public DateTime Date { get; set; }
        public string CycleCode { get; set; }
        public string CityCode { get; set; }
        public string SerialNo { get; set; }
        public string SenderBankCode { get; set; }
        public string SenderBranchCode { get; set; }
        public string ReceiverBankCode { get; set; }
        public string ReceiverBranchCode { get; set; }
        public string ChequeNumber { get; set; }
        public string AccountNumber { get; set; }
        public string SequenceNumber { get; set; }
        public string TransactionCode { get; set; }
        public string InstrumentNo { get; set; }
        public decimal Amount { get; set; }
        public string IQATag { get; set; }
        public string DocumentSkew { get; set; }
        public string Piggyback { get; set; }
        public string ImageToolight { get; set; }
        public string HorizontalStreaks { get; set; }
        public string BelowMinimumCompressedImageSize { get; set; }
        public string AboveMaximumCompressedImageSize { get; set; }
        public string UndersizeImage { get; set; }
        public string FoldedorTornDocumentCorners { get; set; }
        public string FoldedOrTornDocumentEdges { get; set; }
        public string FramingError { get; set; }
        public string OversizeImage { get; set; }
        public string ImageTooDark { get; set; }
        public string FrontRearDimensionMismatch { get; set; }
        public string CarbonStrip { get; set; }
        public string OutOfFocus { get; set; }
        public string QRCodeMatch { get; set; }
        public string UV { get; set; }
        public string MICRPresent { get; set; }
        public string StandardCheque { get; set; }
        public string InstrumentDuplicate { get; set; }
        public decimal AverageChequeAmount { get; set; }
        public string TotalChequesCount { get; set; }
        public string TotalChequesReturnCount { get; set; }
        public string CaptureAtNIFT_Branch { get; set; }
        public string DeferredCheque { get; set; }
        public string FraudChequeHistory { get; set; }
        public string Stan { get; set; }
        public string HubCode { get; set; }
        public bool Export { get; set; }
        public bool Callback { get; set; }
        public string Status { get; set; }
        public DateTime ImportTime { get; set; }
        public string ErrorInFields { get; set; }
        public bool Error { get; set; }
    }
}
