using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using student.Models;
using System.Net.Http;

namespace student.Areas.Faculty.Controllers
{
    public class ProjectMeetingController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7148/api/AcdPrjProjectMeeting";

        public ProjectMeetingController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public IActionResult MeetingEntry(ProjectMeeting model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Save to DB (add Created = DateTime.Now)
                TempData["Message"] = "Meeting scheduled successfully!";
                return RedirectToAction("Attendance");
            }

            return View(model);
        }

        public async Task<IActionResult> History()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(_apiBaseUrl);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Unable to fetch meeting history.";
                    return View(new List<ProjectMeeting>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var meetings = JsonConvert.DeserializeObject<List<ProjectMeeting>>(json);

                // Optional: filter only completed or cancelled meetings
                var filteredMeetings = meetings?
                    .Where(m => m.MeetingStatus?.ToLower() == "complete" || m.MeetingStatus?.ToLower() == "cancelled")
                    .OrderByDescending(m => m.MeetingDateTime)
                    .ToList();

                return View(filteredMeetings);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return View(new List<ProjectMeeting>());
            }
        }

       
        public async Task<IActionResult> Attendance()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(_apiBaseUrl);

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Unable to fetch meeting list.";
                    return View(new List<ProjectMeeting>());
                }

                var json = await response.Content.ReadAsStringAsync();
                var meetings = JsonConvert.DeserializeObject<List<ProjectMeeting>>(json);

                // ✅ Show only completed meetings (case-insensitive)
                var completedMeetings = meetings?
                    .Where(m => m.MeetingStatus != null && m.MeetingStatus.Equals("complete", StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(m => m.MeetingDateTime)
                    .ToList();

                return View(completedMeetings);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error fetching meeting data: " + ex.Message;
                return View(new List<ProjectMeeting>());
            }
        }


        [HttpPost]
        public IActionResult MarkAttendence(int meetingId, List<int> studentIds, List<bool> isPresent, List<string> remarks)
        {
            // Here you’d insert attendance records into ACD_PRJ_ProjectMeetingAttendance table
            for (int i = 0; i < studentIds.Count; i++)
            {
                var attendance = new AcdPrjProjectMeetingAttendance
                {
                    ProjectMeetingId = meetingId,
                    StudentId = studentIds[i],
                    IsPresent = isPresent[i],
                    AttendanceRemarks = remarks[i]
                };

                // TODO: Save to database
            }

            TempData["Message"] = "Attendance saved successfully!";
            return RedirectToAction("Attendance");
        }
    }
}