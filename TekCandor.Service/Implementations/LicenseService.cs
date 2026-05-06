using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Infrastructure;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class LicenseService : ILicenseService
    {
        private readonly IApplicationConfigRepository _configRepo;
        private readonly IConfiguration _config;
        private const string LicenseKey = "LicenseKey";
        private const string DateFormat = "dd-MM-yyyy";

        public LicenseService(IApplicationConfigRepository configRepo, IConfiguration config)
        {
            _configRepo = configRepo;
            _config = config;
        }

        public async Task<LicenseStatusDTO> GetLicenseStatusAsync()
        {
            try
            {
                var encryptedKey = await _configRepo.GetValueByKeyAsync(LicenseKey);
                var privateKey = _config["License:PrivateKey"];

                if (string.IsNullOrWhiteSpace(encryptedKey))
                {
                    return new LicenseStatusDTO
                    {
                        IsValid = false,
                        IsExpired = true,
                        ExpiryDate = DateTime.MinValue,
                        DaysRemaining = 0,
                        Message = "License key not found. Please contact administrator."
                    };
                }

                if (string.IsNullOrWhiteSpace(privateKey))
                {
                    return new LicenseStatusDTO
                    {
                        IsValid = false,
                        IsExpired = true,
                        ExpiryDate = DateTime.MinValue,
                        DaysRemaining = 0,
                        Message = "License configuration missing. Please contact administrator."
                    };
                }

                var decryptedDate = RsaHelper.DecryptText(encryptedKey, privateKey);

                if (!DateTime.TryParseExact(decryptedDate.Trim(), DateFormat,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var expiryDate))
                {
                    return new LicenseStatusDTO
                    {
                        IsValid = false,
                        IsExpired = true,
                        ExpiryDate = DateTime.MinValue,
                        DaysRemaining = 0,
                        Message = "Invalid license key format. Please contact administrator."
                    };
                }

                var today = DateTime.Today;
                var daysRemaining = (int)(expiryDate.Date - today).TotalDays;
                var isExpired = expiryDate.Date < today;

                return new LicenseStatusDTO
                {
                    IsValid = !isExpired,
                    IsExpired = isExpired,
                    ExpiryDate = expiryDate,
                    DaysRemaining = isExpired ? 0 : daysRemaining,
                    Message = isExpired
                        ? $"License expired on {expiryDate:dd-MM-yyyy}. Please renew your license."
                        : daysRemaining <= 30
                            ? $"License expiring soon. {daysRemaining} day(s) remaining."
                            : "License is valid"
                };
            }
            catch (Exception ex)
            {
                return new LicenseStatusDTO
                {
                    IsValid = false,
                    IsExpired = true,
                    ExpiryDate = DateTime.MinValue,
                    DaysRemaining = 0,
                    Message = $"License validation failed: {ex.Message}"
                };
            }
        }

        public async Task<bool> IsLicenseValidAsync()
        {
            var status = await GetLicenseStatusAsync();
            return status.IsValid;
        }

        public async Task<bool> UpdateLicenseAsync(string encryptedKey, string updatedBy)
        {
            try
            {
                var privateKey = _config["License:PrivateKey"];
                if (string.IsNullOrWhiteSpace(privateKey))
                    return false;

                var decryptedDate = RsaHelper.DecryptText(encryptedKey, privateKey);

                if (!DateTime.TryParseExact(decryptedDate.Trim(), DateFormat,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out _))
                    return false;

                return await _configRepo.UpdateValueByKeyAsync(LicenseKey, encryptedKey, updatedBy);
            }
            catch
            {
                return false;
            }
        }
    }
}
