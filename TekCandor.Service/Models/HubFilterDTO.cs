using System.Collections.Generic;

namespace TekCandor.Service.Models
{
    public class HubFilterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class HubFilterResponse
    {
        public List<HubFilterDTO> Hubs { get; set; } = new List<HubFilterDTO>();
        public string FilterType { get; set; } = string.Empty; // "HubWise" or "BranchWise"
    }
}
