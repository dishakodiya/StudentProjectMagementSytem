using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace student.Models
{
    public class ProjectMeeting
    {
        [Key]
        public int ProjectMeetingId { get; set; }

        [Required]
        [Display(Name = "Project Group")]
        public int ProjectGroupId { get; set; }

        [Required]
        [Display(Name = "Guide Staff")]
        public int GuideStaffId { get; set; }

        [Required]
        [Display(Name = "Meeting Date & Time")]
        public DateTime MeetingDateTime { get; set; }

        [Required]
        [Display(Name = "Purpose")]
        public string MeetingPurpose { get; set; } = null!;

        [Display(Name = "Location")]
        public string? MeetingLocation { get; set; }

        [Display(Name = "Notes")]
        public string? MeetingNotes { get; set; }

        [Display(Name = "Status")]
        public string? MeetingStatus { get; set; }

        [Display(Name = "Status Description")]
        public string? MeetingStatusDescription { get; set; }

        [Display(Name = "Status Date & Time")]
        public DateTime? MeetingStatusDatetime { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

    }
    public class ProjectMeetingViewModel
    {
        public int ProjectMeetingId { get; set; }
        public string ProjectGroupName { get; set; } = "N/A";
        public string GuideStaffName { get; set; } = "N/A";
        public DateTime MeetingDateTime { get; set; }
        public string MeetingPurpose { get; set; } = "";
        public string? MeetingLocation { get; set; }
        public string? MeetingStatus { get; set; }
    }
    public partial class AcdPrjProjectMeetingAttendance
    {
        public int ProjectMeetingAttendanceId { get; set; }

        public int ProjectMeetingId { get; set; }       

        public int StudentId { get; set; }

        public bool? IsPresent { get; set; }

        public string? AttendanceRemarks { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        //public virtual AcdPrjProjectMeeting ProjectMeeting { get; set; } = null!;

        //public virtual AcdStudent Student { get; set; } = null!;
    }
}
