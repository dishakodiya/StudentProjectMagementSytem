using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using student.Models;
using System.Text;

namespace student.Controllers
{
    public class ProjectGroupMemberController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl = "https://localhost:7148/api";

        public ProjectGroupMemberController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        // ✅ List Page
        public async Task<IActionResult> ProjectGroupMemberList()
        {
            var response = await _client.GetAsync("https://localhost:7148/api/AcdPrjProjectGroupMember");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to fetch data";
                return View(new List<ProjectGroupMember>());
            }

            var data = await response.Content.ReadAsStringAsync();
            var members = JsonConvert.DeserializeObject<List<ProjectGroupMember>>(data);
            return View(members);
        }
        // ✅ Add/Edit (GET)
        public async Task<IActionResult> ProjectGroupMemberAddEdit(int? id)
        {
            // For dropdowns
            await LoadDropdowns();

            if (id == null || id == 0)
                return View(new ProjectGroupMember());

            var response = await _client.GetAsync($"{_apiUrl}/AcdPrjProjectGroupMember/{id}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var member = JsonConvert.DeserializeObject<ProjectGroupMember>(json);
                return View(member);
            }

            TempData["ErrorMessage"] = "Unable to load member data.";
            return RedirectToAction("ProjectGroupMemberList");
        }

        // ✅ Add/Edit (POST)
        [HttpPost]
        public async Task<IActionResult> ProjectGroupMemberAddEdit(ProjectGroupMember member)
        {
            await LoadDropdowns();

            if (!ModelState.IsValid)
                return View(member);

            var jsonData = JsonConvert.SerializeObject(member);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (member.ProjectGroupMemberId == 0)
            {
                // Add
                response = await _client.PostAsync($"{_apiUrl}/AcdPrjProjectGroupMember", content);
            }
            else
            {
                // Edit
                response = await _client.PutAsync($"{_apiUrl}/AcdPrjProjectGroupMember/{member.ProjectGroupMemberId}", content);
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Saved successfully.";
                return RedirectToAction("ProjectGroupMemberList");
            }

            TempData["ErrorMessage"] = "Operation failed.";
            return View(member);
        }

        // ✅ Delete
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"{_apiUrl}/AcdPrjProjectGroupMember/{id}");

            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "Deleted successfully.";
            else
                TempData["ErrorMessage"] = "Unable to delete record.";

            return RedirectToAction("ProjectGroupMemberList");
        }

        // ✅ Helper – Load dropdowns
        private async Task LoadDropdowns()
        {
            // Load Project Groups
            var groupResponse = await _client.GetAsync($"{_apiUrl}/AcdPrjProjectGroup");
            if (groupResponse.IsSuccessStatusCode)
            {
                var json = await groupResponse.Content.ReadAsStringAsync();
                var groups = JsonConvert.DeserializeObject<List<AcdPrjProjectGroup>>(json);
                ViewBag.ProjectGroups = groups.Select(g => new { g.ProjectGroupId, g.ProjectGroupName }).ToList();
            }
            else
            {
                ViewBag.ProjectGroups = new List<object>();
            }

            // Load Students
            var studentResponse = await _client.GetAsync($"{_apiUrl}/AcdStudent");
            if (studentResponse.IsSuccessStatusCode)
            {
                var json = await studentResponse.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<List<AcdStudent>>(json);
                ViewBag.Students = students.Select(s => new { s.StudentId, s.StudentName }).ToList();
            }
            else
            {
                ViewBag.Students = new List<object>();
            }
        }
    }
}
