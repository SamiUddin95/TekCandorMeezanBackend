using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Interfaces;

namespace TekCandor.Service.Services
{
    public class SsrsRenderService: ISsrsRenderService
    {
        private readonly HttpClient _httpClient;
        private readonly SsrsOptions _options;

        public SsrsRenderService(HttpClient httpClient, IOptions<SsrsOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
            if (!string.IsNullOrWhiteSpace(_options.ServerUrl))
            {
                _httpClient.BaseAddress = new Uri(AppendTrailingSlash(_options.ServerUrl));
            }
        }

        public async Task<(byte[] Content, string ContentType, string FileExtension)> RenderAsync(
            string reportPath,
            string format,
            IDictionary<string, string>? parameters,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(reportPath))
                throw new ArgumentException("Report path is required", nameof(reportPath));

            var fmt = NormalizeFormat(format);
            var (contentType, ext) = GetFormatInfo(fmt);

            var query = new List<string> { $"rs:Command=Render", $"rs:Format={Uri.EscapeDataString(fmt)}" };
            if (parameters != null)
            {
                foreach (var kv in parameters)
                {
                    if (!string.IsNullOrWhiteSpace(kv.Key) && !string.IsNullOrWhiteSpace(kv.Value) || string.IsNullOrWhiteSpace(kv.Value))
                    {
                        var key = Uri.EscapeDataString(kv.Key);
                        var val = Uri.EscapeDataString(kv.Value ?? string.Empty);
                        query.Add($"{key}={val}");
                    }
                }
            }

            var basePath = reportPath.StartsWith("/", StringComparison.Ordinal) ? reportPath : "/" + reportPath;
            var encodedPath = EncodeReportPath(basePath);
            var pathAndQuery = $"?{encodedPath}&{string.Join('&', query)}";

            Uri requestUri;
            if (_httpClient.BaseAddress is null)
            {
                if (string.IsNullOrWhiteSpace(_options.ServerUrl))
                    throw new InvalidOperationException("SSRS ServerUrl is not configured. Set Ssrs:ServerUrl in appsettings.");
                requestUri = new Uri(AppendTrailingSlash(_options.ServerUrl) + pathAndQuery, UriKind.Absolute);
            }
            else
            {
                requestUri = new Uri(pathAndQuery, UriKind.Relative);
            }

            using var req = new HttpRequestMessage(HttpMethod.Get, requestUri);
            using var resp = await _httpClient.SendAsync(req, HttpCompletionOption.ResponseContentRead, cancellationToken);
            if (!resp.IsSuccessStatusCode)
            {
                var text = await resp.Content.ReadAsStringAsync(cancellationToken);
                var preview = text?.Length > 1000 ? text.Substring(0, 1000) : text;
                throw new HttpRequestException($"SSRS error {(int)resp.StatusCode} {resp.ReasonPhrase}: {preview}");
            }
            var bytes = await resp.Content.ReadAsByteArrayAsync(cancellationToken);
            return (bytes, contentType, ext);
        }

        private static string NormalizeFormat(string? format)
        {
            if (string.IsNullOrWhiteSpace(format)) return "PDF";
            var f = format.Trim().ToUpperInvariant();
            return f switch
            {
                "EXCEL" or "XLS" or "XLSX" => "EXCEL",
                "WORD" or "DOC" or "DOCX" => "WORD",
                "IMAGE" or "PNG" or "JPG" or "JPEG" => "IMAGE",
                "MHTML" => "MHTML",
                "CSV" => "CSV",
                "HTML" or "HTML4.0" or "HTML5" => "HTML4.0",
                _ => f
            };
        }

        private static (string ContentType, string Extension) GetFormatInfo(string format)
        {
            return format switch
            {
                "PDF" => ("application/pdf", ".pdf"),
                "EXCEL" => ("application/vnd.ms-excel", ".xls"),
                "WORD" => ("application/msword", ".doc"),
                "IMAGE" => ("image/png", ".png"),
                "HTML4.0" => ("text/html", ".html"),
                "CSV" => ("text/csv", ".csv"),
                "MHTML" => ("multipart/related", ".mht"),
                _ => ("application/octet-stream", ".bin")
            };
        }

        private static string AppendTrailingSlash(string url)
        {
            if (string.IsNullOrEmpty(url)) return url;
            return url.EndsWith("/", StringComparison.Ordinal) ? url : url + "/";
        }

        private static string EncodeReportPath(string path)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = Uri.EscapeDataString(parts[i]);
            }
            return "/" + string.Join('/', parts);
        }
    }
}
