namespace student.Models
{
    public class AcdStaff
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
