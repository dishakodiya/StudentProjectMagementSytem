using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using student.Models;
using System.Net.Http;
using System.Text;

namespace student.Controllers
{
    public class ProjectGroupController : Controller
    {
        private readonly HttpClient _client;

        public ProjectGroupController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7148/api/");
        }

        // GET: ProjectGroupList
        public async Task<IActionResult> ProjectGroupList()
        {
            var response = await _client.GetAsync("AcdPrjProjectGroup");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to fetch data.";
                return View(new List<AcdPrjProjectGroup>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<AcdPrjProjectGroup>>(json);
            return View(list);
        }

        // GET: ProjectGroupAddEdit
        public async Task<IActionResult> ProjectGroupAddEdit(int? id)
        {
            // Load dropdown data
            await LoadDropdowns();

            if (id == null)
                return View(new AcdPrjProjectGroup());

            var response = await _client.GetAsync($"AcdPrjProjectGroup/{id}");
            if (!response.IsSuccessStatusCode)
                return View(new AcdPrjProjectGroup());

            var json = await response.Content.ReadAsStringAsync();
            var group = JsonConvert.DeserializeObject<AcdPrjProjectGroup>(json);
            return View(group);
        }

        // POST: ProjectGroupAddEdit
        [HttpPost]
        public async Task<IActionResult> ProjectGroupAddEdit(AcdPrjProjectGroup model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(model);
            }

            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            if (model.ProjectGroupId == 0)
                response = await _client.PostAsync("AcdPrjProjectGroup", content);
            else
                response = await _client.PutAsync($"AcdPrjProjectGroup/{model.ProjectGroupId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ProjectGroupList");

            await LoadDropdowns();
            ViewBag.Error = "Something went wrong.";
            return View(model);
        }

        // DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _client.DeleteAsync($"AcdPrjProjectGroup/{id}");
            return RedirectToAction("ProjectGroupList");
        }

        // Helper: Load dropdown data from API
        private async Task LoadDropdowns()
        {
            // Project Types
            var projectTypeRes = await _client.GetAsync("AcdPrjProjectType");
            var staffRes = await _client.GetAsync("AcdStaff");

            var projectTypes = new List<AcdPrjProjectType>();
            var staffList = new List<AcdStaff>();

            if (projectTypeRes.IsSuccessStatusCode)
            {
                var json = await projectTypeRes.Content.ReadAsStringAsync();
                projectTypes = JsonConvert.DeserializeObject<List<AcdPrjProjectType>>(json);
            }

            if (staffRes.IsSuccessStatusCode)
            {
                var json = await staffRes.Content.ReadAsStringAsync();
                staffList = JsonConvert.DeserializeObject<List<AcdStaff>>(json);
            }

            ViewBag.ProjectTypes = projectTypes;
            ViewBag.StaffList = staffList;
        }
        public async Task<IActionResult> ProjectGroupListbyID(int id)
        {
            try
            {
                var response = await _client.GetAsync($"https://localhost:7148/api/AcdPrjProjectGroup/stdent{id}");

                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Unable to fetch project group.";
                    return View();
                }

                var json = await response.Content.ReadAsStringAsync();
                var group = JsonConvert.DeserializeObject<AcdPrjProjectGroup>(json);

                return View(group);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                return View();
            }
        }
    }
}
