using System.Threading.Tasks;
using TekCandor.Repository.Models;

namespace TekCandor.Service.Interfaces
{
    public interface ICoreBankingService
    {
        Task ImportRecordsAfterImportFileAsync();
        Task ImportRecordsAfterImportFileAsync(bool sendSms);
        Task SendChequeReceiveSMSAlertsAsync();
        Task GetSignaturesAsync();
        Task<MBLChequePostingExt?> MBLPendingChequePostingAsync(long id, string stan, string accountNumber, string chequeNumber, string currency, string amount, string cycleCode, string hubCode, string approverId, string authorizerId);
        Task<MBLPOLodgementExt?> MBLPOEncashmentAsync(long id, string stan, string accountNumber, string chequeNumber, string amount, string cycleCode, string hubCode, string senderBankCode, string instrumentNo, string approverId, string authorizerId);
    }
}
