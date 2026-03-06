namespace TekCandor.Repository.Models
{
    public class GetSignatureRequest
    {
        public long Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string ChequeNumber { get; set; } = string.Empty;
    }
}
