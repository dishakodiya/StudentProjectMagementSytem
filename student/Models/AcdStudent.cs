using System;
using System.Reflection;
using System.Reflection.Emit;
namespace student.Models
{
    public class AcdStudent
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
   

}
