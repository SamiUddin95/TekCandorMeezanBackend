namespace TekCandor.Service.Models
{
    public class LicenseStatusDTO
    {
        public bool IsValid { get; set; }
        public bool IsExpired { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysRemaining { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class UpdateLicenseDTO
    {
        public string EncryptedKey { get; set; } = string.Empty;
    }
}
