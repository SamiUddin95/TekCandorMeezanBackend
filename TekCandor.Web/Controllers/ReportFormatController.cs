using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TekCandor.Service.Interfaces;
using TekCandor.Web.Models;

namespace TekCandor.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportFormatController : ControllerBase
    {
        private readonly ISsrsRenderService _renderService;

        public ReportFormatController(ISsrsRenderService renderService)
        {
            _renderService = renderService;
        }

        [HttpPost("BranchWise")]
        public async Task<IActionResult> BranchWise([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }
        [HttpPost("CBCReport")]
        public async Task<IActionResult> CBCReport([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }
        [HttpPost("ClearingLogReport")]
        public async Task<IActionResult> ClearingLogReport([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }
        [HttpPost("FinalReport")]
        public async Task<IActionResult> FinalReport([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }
        [HttpPost("InwardClearingReport")]
        public async Task<IActionResult> InwardClearingReport([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }
        [HttpPost("ReturnMemoReport")]
        public async Task<IActionResult> ReturnMemoReport([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }
        [HttpPost("ReturnRegisterReport")]
        public async Task<IActionResult> ReturnRegisterReport([FromBody] ReportRenderRequest request, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (bytes, contentType, ext) = await _renderService.RenderAsync(
                request.ReportPath,
                request.Format ?? "PDF",
                request.Parameters,
                ct);

            var safeName = string.IsNullOrWhiteSpace(request.FileName)
                ? SlugFromPath(request.ReportPath) + ext
                : request.FileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                    ? request.FileName
                    : request.FileName + ext;

            return File(bytes, contentType, safeName);
        }

        private static string SlugFromPath(string path)
        {
            var name = path?.TrimEnd('/') ?? "report";
            var last = name.LastIndexOf('/') >= 0 ? name[(name.LastIndexOf('/') + 1)..] : name;
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
            {
                last = last.Replace(c, '_');
            }
            return string.IsNullOrWhiteSpace(last) ? "report" : last;
        }
    }
}

