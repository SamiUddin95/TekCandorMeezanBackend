using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TekCandor.Service.Outward.Models;

namespace TekCandor.Service.Outward.Interfaces
{
    public interface IBatchFileService
    {
        Task<FileUploadResultDTO> ProcessUploadedFileAsync(IFormFile file, string uploadedBy);
    }
}
