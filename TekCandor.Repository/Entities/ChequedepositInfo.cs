using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TekCandor.Repository.Entities
{
    public class ChequedepositInfo
    {
        public long Id { get; set; }
        public System.DateTime Date { get; set; }
        public string? CycleCode { get; set; }
        public string? CityCode { get; set; }
        public string? serialNo { get; set; }
        public string? SenderBankCode { get; set; }
        public string? SenderBranchCode { get; set; }
        public string? ReceiverBankCode { get; set; }
        public string? ReceiverBranchCode { get; set; }
        
        public string? ChequeNumber { get; set; }
        public string? OldChequeNo { get; set; }
        
        public string? AccountNumber { get; set; }
        
        public string? OldAccount { get; set; }
        
        public string? SequenceNumber { get; set; }
        
        public string? TransactionCode { get; set; }
        public decimal? Amount { get; set; }
        
        public string? IQATag { get; set; }
        
        public string? DocumentSkew { get; set; }
        
        public string? Piggyback { get; set; }
        
        public string? ImageToolight { get; set; }
        public string? HorizontalStreaks { get; set; }
        
        public string? BelowMinimumCompressedImageSize { get; set; }
        
        public string? AboveMaximumCompressedImageSize { get; set; }
        
        public string? UndersizeImage { get; set; }
        
        public string? FoldedorTornDocumentCorners { get; set; }
        
        public string? FoldedOrTornDocumentEdges { get; set; }
        
        public string? FramingError { get; set; }
        
        public string? OversizeImage { get; set; }
        
        public string? ImageTooDark { get; set; }
        
        public string? FrontRearDimensionMismatch { get; set; }
        
        public string? CarbonStrip { get; set; }
        
        public string? OutOfFocus { get; set; }
        
        public string? QRCodeMatch { get; set; }
        
        public string? UV { get; set; }
        
        public string? MICRPresent { get; set; }
        
        public string? StandardCheque { get; set; }

        public string? InstrumentDuplicate { get; set; }

        public decimal? AverageChequeAmount { get; set; }
        
        public string? TotalChequesCount { get; set; }
        public string? TotalChequesReturnCount { get; set; }
        
        public string? CaptureAtNIFT_Branch { get; set; }
        
        public string? DeferredCheque { get; set; }
        
        public string? Remarks { get; set; }
        
        public string? BranchRemarks { get; set; }
        
        public string? FraudChequeHistory { get; set; }
        
        public string? imgF { get; set; }
        public string? imgB { get; set; }
        public string? imgU { get; set; }
        public string? status { get; set; }
        public string? poStatus { get; set; }
        public bool? Export { get; set; }
        public bool? serviceRun { get; set; }
        
        public string? Returnreasone { get; set; }
        public string? PostRestriction { get; set; }
        public bool? Error { get; set; }
        public string? stan { get; set; }
        
        public string? HubCode { get; set; }
        public string? TrProcORRecTime { get; set; }
        public string? PostingTime { get; set; }
        public string? TrRecTimeCCU { get; set; }
        public string? TrRecTimeBranch { get; set; }
        public bool? Callback { get; set; }
        public string? Callbacksend { get; set; }
        public bool?  CallbackEmailSend { get; set; }
        public bool? isEditing { get; set; }
        public bool? chqRetSMSSend { get; set; }
        public bool? chqRecSMSSend { get; set; }
        public string? ErrorInFields { get; set; }
        public string? InstrumentNo { get; set; }
        public string? AccountBalance { get; set; }
        public string? AccountStatus { get; set; }
        public string? AccountTitle { get; set; }
        public string? IBAN { get; set; }
        public string? CoreFTId { get; set; }
        public string? BranchStaffId { get; set; }
        public string? ApproverId { get; set; }
        public string? AuthorizerId { get; set; }
        public string? Currency { get; set; }
        public string? MobileNo { get; set; }
        public System.DateTime? Importtime { get; set; }
        public bool? manual_imp { get; set; }
        public bool IsDeleted { get; set; }
    }
}
