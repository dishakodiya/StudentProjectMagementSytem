namespace student.Models
{
    public class AcdPrjProjectType
    {
        public int ProjectTypeId { get; set; }

        public string ProjectTypeName { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
