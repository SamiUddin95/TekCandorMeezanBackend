using System.Collections.Generic;

namespace TekCandor.Service.Models
{
    public class BranchFilterDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class BranchFilterResponse
    {
        public List<BranchFilterDTO> Branches { get; set; } = new List<BranchFilterDTO>();
        public string FilterType { get; set; } = string.Empty; // "HubWise" or "BranchWise"
    }

    public class FilterOptionDTO
    {
        public string Text { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class StatusFilterResponse
    {
        public List<FilterOptionDTO> Statuses { get; set; } = new List<FilterOptionDTO>();
    }

    public class InstrumentFilterResponse
    {
        public List<FilterOptionDTO> Instruments { get; set; } = new List<FilterOptionDTO>();
    }

    public class CycleFilterResponse
    {
        public List<FilterOptionDTO> Cycles { get; set; } = new List<FilterOptionDTO>();
    }

    public class ServiceRunFilterResponse
    {
        public List<FilterOptionDTO> Options { get; set; } = new List<FilterOptionDTO>();
    }

    public class ReturnReasonFilterResponse
    {
        public List<FilterOptionDTO> ReturnReasons { get; set; } = new List<FilterOptionDTO>();
    }
}
