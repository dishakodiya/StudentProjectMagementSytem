using System.ComponentModel.DataAnnotations.Schema;

namespace student.Models
{
    public class ProjectGroupMember
    {
        public int ProjectGroupMemberId { get; set; }

        public int ProjectGroupId { get; set; }

        public int StudentId { get; set; }

        public bool IsGroupLeader { get; set; }

        public decimal StudentCgpa { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
        
        [NotMapped]
        public string? StudentName { get; set; }


        public ProjectGroup? ProjectGroup { get; set; }
        public Student? Student { get; set; }
    }
    public class ProjectGroup
    {
        public int ProjectGroupId { get; set; }
        public string? ProjectGroupName { get; set; }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
    }
}
