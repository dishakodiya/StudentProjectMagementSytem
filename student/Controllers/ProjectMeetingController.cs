using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using student.Models;
using System.Text;
using static student.Models.ProjectMeeting;

namespace student.Controllers
{
     public class ProjectMeetingController : Controller
     {
            private readonly HttpClient _httpClient;
            private readonly string apiBaseUrl = "https://localhost:7148/api/AcdPrjProjectMeeting"; // Replace with your API URL

            public ProjectMeetingController(IHttpClientFactory httpClientFactory)
            {
                _httpClient = httpClientFactory.CreateClient();
            }

            // GET: ProjectMeetingList
            public async Task<IActionResult> ProjectMeetingList()
            {
                var response = await _httpClient.GetAsync(apiBaseUrl);
                if (!response.IsSuccessStatusCode)
                    return View(new List<ProjectMeeting>());

                var json = await response.Content.ReadAsStringAsync();

                // Deserialize into strongly typed list
                var meetings = JsonConvert.DeserializeObject<List<ProjectMeeting>>(json);

                return View(meetings);
            }

        public async Task<IActionResult> AddEdit(int? id)
        {
            if (id == null)
                return View(new ProjectMeeting()); // Add new

            var response = await _httpClient.GetAsync($"{apiBaseUrl}/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var meeting = JsonConvert.DeserializeObject<ProjectMeeting>(json);

            return View(meeting); // Edit existing
        }

        // POST: Add/Edit
        [HttpPost]
        public async Task<IActionResult> AddEdit(ProjectMeeting model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Set Created/Modified for new record
            if (model.ProjectMeetingId == 0)
                model.Created = DateTime.Now;

            model.Modified = DateTime.Now;

            var jsonData = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            if (model.ProjectMeetingId == 0)
            {
                // Add new
                await _httpClient.PostAsync(apiBaseUrl, content);
            }
            else
            {
                // Update existing
                await _httpClient.PutAsync($"{apiBaseUrl}/{model.ProjectMeetingId}", content);
            }

            return RedirectToAction(nameof(ProjectMeetingList));
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"{apiBaseUrl}/{id}");
            return RedirectToAction(nameof(ProjectMeetingList));
        }


    }
}













    /////////////////////////////////////////////////////////////////////

    //[HttpPost]
    //public IActionResult MeetingEntry(ProjectMeeting model)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        // TODO: Save to DB (add Created = DateTime.Now)
    //        TempData["Message"] = "Meeting scheduled successfully!";
    //        return RedirectToAction("Attendance");
    //    }

    //    return View(model);
    //}

    //public async Task<IActionResult> History()
    //{
    //    try
    //    {
    //        var client = _httpClientFactory.CreateClient();
    //        var response = await client.GetAsync(_apiBaseUrl);

    //        if (!response.IsSuccessStatusCode)
    //        {
    //            TempData["Error"] = "Unable to fetch meeting history.";
    //            return View(new List<ProjectMeeting>());
    //        }

    //        var json = await response.Content.ReadAsStringAsync();
    //        var meetings = JsonConvert.DeserializeObject<List<ProjectMeeting>>(json);

    //        // Optional: filter only completed or cancelled meetings
    //        var filteredMeetings = meetings?
    //            .Where(m => m.MeetingStatus?.ToLower() == "complete" || m.MeetingStatus?.ToLower() == "cancelled")
    //            .OrderByDescending(m => m.MeetingDateTime)
    //            .ToList();

    //        return View(filteredMeetings);
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["Error"] = "Error: " + ex.Message;
    //        return View(new List<ProjectMeeting>());
    //    }
    //}


    //public async Task<IActionResult> Attendance()
    //{
    //    try
    //    {
    //        var client = _httpClientFactory.CreateClient();
    //        var response = await client.GetAsync(_apiBaseUrl);

    //        if (!response.IsSuccessStatusCode)
    //        {
    //            TempData["Error"] = "Unable to fetch meeting list.";
    //            return View(new List<ProjectMeeting>());
    //        }

    //        var json = await response.Content.ReadAsStringAsync();
    //        var meetings = JsonConvert.DeserializeObject<List<ProjectMeeting>>(json);

    //        // ✅ Show only completed meetings (case-insensitive)
    //        var completedMeetings = meetings?
    //            .Where(m => m.MeetingStatus != null && m.MeetingStatus.Equals("complete", StringComparison.OrdinalIgnoreCase))
    //            .OrderByDescending(m => m.MeetingDateTime)
    //            .ToList();

    //        return View(completedMeetings);
    //    }
    //    catch (Exception ex)
    //    {
    //        TempData["Error"] = "Error fetching meeting data: " + ex.Message;
    //        return View(new List<ProjectMeeting>());
    //    }
    //}


    //[HttpPost]
    //public IActionResult MarkAttendence(int meetingId, List<int> studentIds, List<bool> isPresent, List<string> remarks)
    //{
    //    // Here you’d insert attendance records into ACD_PRJ_ProjectMeetingAttendance table
    //    for (int i = 0; i < studentIds.Count; i++)
    //    {
    //        var attendance = new AcdPrjProjectMeetingAttendance
    //        {
    //            ProjectMeetingId = meetingId,
    //            StudentId = studentIds[i],
    //            IsPresent = isPresent[i],
    //            AttendanceRemarks = remarks[i]
    //        };

    //        // TODO: Save to database
    //    }

    //    TempData["Message"] = "Attendance saved successfully!";
    //    return RedirectToAction("Attendance");
    //}



