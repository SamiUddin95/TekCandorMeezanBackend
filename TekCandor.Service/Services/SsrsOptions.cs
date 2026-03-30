using System;
using System.Collections.Generic;
using System.Text;

namespace TekCandor.Service.Services
{
    public class SsrsOptions
    {
        public string ServerUrl { get; set; } = string.Empty; // e.g. https://your-ssrs-host/ReportServer
        public bool UseWindowsAuth { get; set; } = true;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Domain { get; set; }
    }
}
