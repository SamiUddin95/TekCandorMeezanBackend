namespace TekCandor.Service.Models
{
    public class CycleDTO
    {
        public long Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public bool IsDeleted { get; set; }
        public required string CreatedBy { get; set; }
        public required string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
