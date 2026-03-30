using System.ComponentModel.DataAnnotations;

namespace TekCandor.Web.Models
{
    public class ReportRenderRequest
    {
        [Required]
        public string ReportPath { get; set; } = string.Empty; // e.g. "/MyFolder/MyReport"
        public string? Format { get; set; } = "PDF"; // PDF, EXCEL, WORD, IMAGE, HTML
        public Dictionary<string, string>? Parameters { get; set; }
        public string? FileName { get; set; } // optional preferred file name

    }
}
