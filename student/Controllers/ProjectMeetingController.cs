using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using student.Models;
using System.Net.Http;
using System.Text;
using static student.Models.ProjectMeeting;
namespace student.Controllers
{
     public class ProjectMeetingController : Controller
     {
            private readonly HttpClient _httpClient;
            private readonly string apiBaseUrl = "https://localhost:7148/api/AcdPrjProjectMeeting"; // Replace with your API URL
        private readonly string projectMeetingApi = "https://localhost:7148/api/AcdPrjProjectMeeting";
        private readonly string attendanceApi = "https://localhost:7148/api/AcdPrjProjectMeetingAttendance";
        private readonly string projectGroupApi = "https://localhost:7148/api/AcdPrjProjectGroupMembers";
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
        //[HttpGet]
        //public async Task<IActionResult> AttendenceMark(int meetingId, int projectGroupId)
        //{
        //    try
        //    {
        //        var groupResponse = await _httpClient.GetAsync($"https://localhost:7148/api/AcdPrjProjectGroup/{projectGroupId}");

        //        if (!groupResponse.IsSuccessStatusCode)
        //        {
        //            ViewBag.Error = "Failed to fetch group members.";
        //            return View(new List<AttendanceMarkViewModel>());
        //        }

        //        var json = await groupResponse.Content.ReadAsStringAsync();

        //        // Deserialize into object, not list
        //        var group = JsonConvert.DeserializeObject<ProjectGroupWithMembers>(json);

        //        if (group == null || group.Members == null || !group.Members.Any())
        //        {
        //            ViewBag.Error = "No members found in this group.";
        //            return View(new List<AttendanceMarkViewModel>());
        //        }

        //        // Map members to attendance model
        //        var model = new List<AttendanceMarkViewModel>
        //{
        //    new AttendanceMarkViewModel
        //    {
        //        ProjectMeetingId = meetingId,
        //        Members = group.Members.Select(m => new AttendanceMember
        //        {
        //            StudentId = m.StudentId,
        //            StudentName = m.StudentName,
        //            IsPresent = true,
        //            Remarks = ""
        //        }).ToList()
        //    }
        //};

        //        ViewBag.MeetingId = meetingId;
        //        ViewBag.GroupProjectId = projectGroupId;

        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Error: " + ex.Message;
        //        return View(new List<AttendanceMarkViewModel>());
        //    }
        //}





        //// ✅ POST: Save All Attendance
        //[HttpPost]
        //public async Task<IActionResult> AttendenceMark(List<AttendanceMarkViewModel> model)
        //{
        //    try
        //    {
        //        var allAttendance = new List<AcdPrjProjectMeetingAttendance>();

        //        foreach (var item in model)
        //        {
        //            foreach (var member in item.Members)
        //            {
        //                allAttendance.Add(new AcdPrjProjectMeetingAttendance
        //                {
        //                    ProjectMeetingId = item.ProjectMeetingId,
        //                    StudentId = member.StudentId,
        //                    IsPresent = member.IsPresent,
        //                    AttendanceRemarks = member.Remarks,
        //                    Description = "Marked from Web UI",
        //                    Created = DateTime.Now
        //                });
        //            }
        //        }

        //        var json = JsonConvert.SerializeObject(allAttendance);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.PostAsync("https://localhost:7148/api/AcdPrjProjectMeetingAttendance/SaveAll", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            ViewBag.Message = "✅ Attendance saved successfully!";
        //        }
        //        else
        //        {
        //            ViewBag.Error = "❌ Failed to save attendance.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Error: " + ex.Message;
        //    }

        //    return RedirectToAction("ProjectMeetingList"); // Redirect on exception
        //}


        //[HttpPost]
        //public async Task<IActionResult> AttendenceMark(List<AttendanceMarkViewModel> model)
        //{
        //    try
        //    {
        //        foreach (var item in model)
        //        {
        //            foreach (var member in item.Members)
        //            {
        //                // Check if attendance already exists in API
        //                var existingResponse = await _httpClient.GetAsync($"https://localhost:7148/api/AcdPrjProjectMeetingAttendance/ByMeeting/{item.ProjectMeetingId}");
        //                var existingJson = await existingResponse.Content.ReadAsStringAsync();
        //                var existingAttendance = JsonConvert.DeserializeObject<List<AcdPrjProjectMeetingAttendance>>(existingJson) ?? new List<AcdPrjProjectMeetingAttendance>();

        //                var attendance = existingAttendance.FirstOrDefault(a => a.StudentId == member.StudentId);

        //                if (attendance != null)
        //                {
        //                    // Update existing
        //                    attendance.IsPresent = member.IsPresent;
        //                    attendance.AttendanceRemarks = member.Remarks;
        //                    attendance.Description = "Updated from Web UI";
        //                    attendance.Modified = DateTime.Now;

        //                    var updateJson = JsonConvert.SerializeObject(attendance);
        //                    var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
        //                    await _httpClient.PutAsync($"https://localhost:7148/api/AcdPrjProjectMeetingAttendance/{attendance.ProjectMeetingAttendanceId}", updateContent);
        //                }
        //                else
        //                {
        //                    // New attendance
        //                    var newAttendance = new AcdPrjProjectMeetingAttendance
        //                    {
        //                        ProjectMeetingId = item.ProjectMeetingId,
        //                        StudentId = member.StudentId,
        //                        IsPresent = member.IsPresent,
        //                        AttendanceRemarks = member.Remarks,
        //                        Description = "Marked from Web UI",
        //                        Created = DateTime.Now
        //                    };

        //                    var json = JsonConvert.SerializeObject(newAttendance);
        //                    var content = new StringContent(json, Encoding.UTF8, "application/json");
        //                    await _httpClient.PostAsync("https://localhost:7148/api/AcdPrjProjectMeetingAttendance/SaveAll", content);
        //                }
        //            }
        //        }

        //        return RedirectToAction("ProjectMeetingList"); // Redirect after save/update
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Error: " + ex.Message;
        //        return View(model);
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> AttendenceMark(List<AttendanceMarkViewModel> model)
        //{
        //    try
        //    {
        //        var allAttendance = new List<AcdPrjProjectMeetingAttendance>();

        //        foreach (var item in model)
        //        {
        //            foreach (var member in item.Members)
        //            {
        //                allAttendance.Add(new AcdPrjProjectMeetingAttendance
        //                {
        //                    ProjectMeetingId = item.ProjectMeetingId,
        //                    StudentId = member.StudentId,
        //                    IsPresent = member.IsPresent,
        //                    AttendanceRemarks = member.Remarks,
        //                    Description = "Marked from Web UI"
        //                });
        //            }
        //        }

        //        var json = JsonConvert.SerializeObject(allAttendance);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        // Call the new ReplaceAll endpoint
        //        var response = await _httpClient.PostAsync("https://localhost:7148/api/AcdPrjProjectMeetingAttendance/ReplaceAll", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction("ProjectMeetingList"); // Redirect to list after save
        //        }
        //        else
        //        {
        //            ViewBag.Error = "❌ Failed to save attendance.";
        //            return View(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = "Error: " + ex.Message;
        //        return View(model);
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> AttendenceMark(List<AttendanceMarkViewModel> model)
        {
            try
            {
                var allAttendance = new List<AcdPrjProjectMeetingAttendance>();

                foreach (var item in model)
                {
                    foreach (var member in item.Members)
                    {
                        allAttendance.Add(new AcdPrjProjectMeetingAttendance
                        {
                            ProjectMeetingId = item.ProjectMeetingId,
                            StudentId = member.StudentId,
                            IsPresent = member.IsPresent,
                            AttendanceRemarks = member.Remarks,
                            Description = "Marked from Web UI"
                        });
                    }
                }

                var json = JsonConvert.SerializeObject(allAttendance);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Call PUT API instead of POST
                var response = await _httpClient.PutAsync("https://localhost:7148/api/AcdPrjProjectMeetingAttendance/UpdateAll", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "✅ Attendance updated successfully!";
                }
                else
                {
                    TempData["Error"] = "❌ Failed to update attendance.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("ProjectMeetingList");
        }


        [HttpGet]
        public async Task<IActionResult> AttendenceMark(int meetingId, int projectGroupId)
        {
            try
            {
                // Step 1: Get group members
                var groupResponse = await _httpClient.GetAsync($"https://localhost:7148/api/AcdPrjProjectGroup/{projectGroupId}");
                if (!groupResponse.IsSuccessStatusCode)
                {
                    ViewBag.Error = "Failed to fetch group members.";
                    return View(new List<AttendanceMarkViewModel>());
                }

                var json = await groupResponse.Content.ReadAsStringAsync();
                var group = JsonConvert.DeserializeObject<ProjectGroupWithMembers>(json);

                if (group == null || group.Members == null || !group.Members.Any())
                {
                    ViewBag.Error = "No members found in this group.";
                    return View(new List<AttendanceMarkViewModel>());
                }

                // Step 2: Get existing attendance
                var attendanceResponse = await _httpClient.GetAsync($"https://localhost:7148/api/AcdPrjProjectMeetingAttendance/ByMeeting/{meetingId}");
                List<dynamic> existingAttendance = new();
                if (attendanceResponse.IsSuccessStatusCode)
                {
                    var attendanceJson = await attendanceResponse.Content.ReadAsStringAsync();
                    existingAttendance = JsonConvert.DeserializeObject<List<dynamic>>(attendanceJson) ?? new List<dynamic>();
                }

                // Step 3: Merge group members with existing attendance
                var model = new List<AttendanceMarkViewModel>
        {
            new AttendanceMarkViewModel
            {
                ProjectMeetingId = meetingId,
                Members = group.Members.Select(m =>
                {
                    var att = existingAttendance.FirstOrDefault(a => a.studentId == m.StudentId);
                    return new AttendanceMember
                    {
                        StudentId = m.StudentId,
                        StudentName = m.StudentName,
                        IsPresent = att != null ? (bool)att.isPresent : true,
                        Remarks = att != null ? (string)att.attendanceRemarks : ""
                    };
                }).ToList()
            }
        };

                ViewBag.MeetingId = meetingId;
                ViewBag.GroupProjectId = projectGroupId;

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error: " + ex.Message;
                return View(new List<AttendanceMarkViewModel>());
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> AttendenceMark(List<AttendanceMarkViewModel> model)
        //{
        //    try
        //    {
        //        foreach (var item in model)
        //        {
        //            foreach (var member in item.Members)
        //            {
        //                // Call API to check if record exists
        //                var response = await _httpClient.GetAsync($"https://localhost:7148/api/AcdPrjProjectMeetingAttendance/ByMeeting/{item.ProjectMeetingId}");
        //                var json = await response.Content.ReadAsStringAsync();
        //                var existingAttendance = JsonConvert.DeserializeObject<List<AcdPrjProjectMeetingAttendance>>(json);

        //                var attendanceRecord = existingAttendance?.FirstOrDefault(a => a.StudentId == member.StudentId);

        //                if (attendanceRecord != null)
        //                {
        //                    // Edit existing record
        //                    attendanceRecord.IsPresent = member.IsPresent;
        //                    attendanceRecord.AttendanceRemarks = member.Remarks;
        //                    attendanceRecord.Modified = DateTime.Now;

        //                    var updateJson = JsonConvert.SerializeObject(attendanceRecord);
        //                    var content = new StringContent(updateJson, Encoding.UTF8, "application/json");
        //                    await _httpClient.PutAsync($"https://localhost:7148/api/AcdPrjProjectMeetingAttendance/{attendanceRecord.ProjectMeetingAttendanceId}", content);
        //                }
        //                else
        //                {
        //                    // Create new record if not exist
        //                    var newAttendance = new AcdPrjProjectMeetingAttendance
        //                    {
        //                        ProjectMeetingId = item.ProjectMeetingId,
        //                        StudentId = member.StudentId,
        //                        IsPresent = member.IsPresent,
        //                        AttendanceRemarks = member.Remarks,
        //                        Description = "Marked from Web UI",
        //                        Created = DateTime.Now
        //                    };

        //                    var newJson = JsonConvert.SerializeObject(newAttendance);
        //                    var content = new StringContent(newJson, Encoding.UTF8, "application/json");
        //                    await _httpClient.PostAsync("https://localhost:7148/api/AcdPrjProjectMeetingAttendance/SaveAll", content);
        //                }
        //            }
        //        }

        //        TempData["Message"] = "✅ Attendance saved successfully!";
        //        return RedirectToAction("ProjectMeetingList", "ProjectMeeting");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = "Error: " + ex.Message;
        //        return RedirectToAction("AttendenceMark");
        //    }
        //}


        // ✅ View Attendance
        //public async Task<IActionResult> ViewAttendance(int meetingId)
        //{
        //    var response = await _httpClient.GetAsync($"{attendanceApi}/ByMeeting/{meetingId}");
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        ViewBag.Error = "Unable to load attendance data.";
        //        return View(new List<AcdPrjProjectMeetingAttendance>());
        //    }

        //    var data = await response.Content.ReadAsStringAsync();
        //    var attendance = JsonConvert.DeserializeObject<List<AcdPrjProjectMeetingAttendance>>(data);
        //    return View(attendance);
        //}
        //public async Task<IActionResult> History()
        //{
        //    try
        //    {
        //        //var client = _httpClient.CreateClient();
        //        var response = await _httpClient.GetAsync(apiBaseUrl);

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
        // }

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


























//public async Task<IActionResult> AttendenceMark(int meetingId, int projectGroupId)
//{
//    // ✅ Call the new combined API
//    var response = await _httpClient.GetAsync($"{attendanceApi}/ByProjectAndMeeting/{projectGroupId}/{meetingId}");
//    if (!response.IsSuccessStatusCode)
//    {
//        ViewBag.Error = "Unable to load project members.";
//        return View(new AttendanceMarkViewModel
//        {
//            ProjectMeetingId = meetingId,
//            Members = new List<AttendanceMember>()
//        });
//    }

//    var data = await response.Content.ReadAsStringAsync();

//    // Deserialize into a dynamic structure (anonymous object)
//    var membersData = JsonConvert.DeserializeObject<List<dynamic>>(data);

//    // Map response to view model
//    var members = membersData.Select(m => new AttendanceMember
//    {
//        StudentId = (int)m.studentId,
//        StudentName = (string)m.studentName,
//        IsPresent = m.isPresent != null ? (bool)m.isPresent : false,
//        Remarks = m.attendanceRemarks != null ? (string)m.attendanceRemarks : ""
//    }).ToList();

//    var viewModel = new AttendanceMarkViewModel
//    {
//        ProjectMeetingId = meetingId,
//        Members = members
//    };

//    return View(viewModel);
//}

//[HttpPost]
//public async Task<IActionResult> AttendenceMark(AttendanceMarkViewModel model)
//{
//    if (model == null || model.Members == null || !model.Members.Any())
//    {
//        TempData["Error"] = "No attendance data provided.";
//        return RedirectToAction(nameof(ProjectMeetingList));
//    }

//    bool allSuccess = true;

//    foreach (var member in model.Members)
//    {
//        var attendance = new AcdPrjProjectMeetingAttendance
//        {
//            ProjectMeetingId = model.ProjectMeetingId,
//            StudentId = member.StudentId,
//            IsPresent = member.IsPresent,
//            AttendanceRemarks = member.Remarks,
//            Description = "Attendance updated from MVC",
//            Modified = DateTime.Now,
//            Created = DateTime.Now
//        };

//        var json = JsonConvert.SerializeObject(attendance);
//        var content = new StringContent(json, Encoding.UTF8, "application/json");

//        //// Try update first
//        var putUrl = $"{attendanceApi}/ByMeeting/{model.ProjectMeetingId}/{member.StudentId}";
//        var putResponse = await _httpClient.PutAsync(putUrl, content);

//        if (!putResponse.IsSuccessStatusCode)
//        {
//            // If update fails (record not found), try add new
//            var postResponse = await _httpClient.PostAsync($"{attendanceApi}/SaveAttendance", content);
//            if (!postResponse.IsSuccessStatusCode)
//                allSuccess = false;
//        }
//    }

//    TempData["Message"] = allSuccess ? "Attendance saved successfully!" : "Some records failed to save.";
//    return RedirectToAction(nameof(ProjectMeetingList));
//}