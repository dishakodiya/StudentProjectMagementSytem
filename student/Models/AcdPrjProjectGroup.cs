namespace student.Models
{
    public class AcdPrjProjectGroup
    {
        public int ProjectGroupId { get; set; }
        public string? ProjectGroupName { get; set; }
        public int? ProjectTypeId { get; set; }
        public int? GuideStaffId { get; set; }
        public string? ProjectTitle { get; set; }
        public string? ProjectArea { get; set; }  
        public string? ProjectDescription { get; set; }
        public decimal? AverageCpi { get; set; }
        public int? ConvenerStaffId { get; set; }
        public int? ExpertStaffId { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        // Dropdown binding
        public string? ProjectTypeName { get; set; }
        public string? GuideStaffName { get; set; }
        public string? ConvenerStaffName { get; set; }
        public string? ExpertStaffName { get; set; }
        public List<ProjectGroupMember> Members { get; set; }
    }
    public class ProjectGroupDTO
    {
        public int ProjectGroupId { get; set; }
        public string? ProjectGroupName { get; set; }
        public string? ProjectTitle { get; set; }
        public string? ProjectArea { get; set; }
        public string? ProjectTypeName { get; set; }
        public string? GuideStaffName { get; set; }
    }
}
