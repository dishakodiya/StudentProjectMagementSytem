namespace student.Models
{
    public class ProjectMeeting
    {
        public int ProjectMeetingId { get; set; }

        public int ProjectGroupId { get; set; }

        public int GuideStaffId { get; set; }

        public DateTime MeetingDateTime { get; set; }

        public string MeetingPurpose { get; set; } = null!;

        public string? MeetingLocation { get; set; }

        public string? MeetingNotes { get; set; }

        public string? MeetingStatus { get; set; }

        public string? MeetingStatusDescription { get; set; }

        public DateTime? MeetingStatusDatetime { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public string ProjectGroupName { get; set; }
        public string GuideStaffName { get; set; }
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
