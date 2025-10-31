using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using student.Models;
using System.Text;

namespace student.Controllers
{
    public class ProjectTypeController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl = "https://localhost:7148/api/AcdPrjProjectType";

        public ProjectTypeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7148/");
        }

        // ✅ GET: List all project types
        public async Task<IActionResult> ProjectTypeList()
        {
            List<AcdPrjProjectType> projectTypes = new();

            HttpResponseMessage response = await _client.GetAsync("api/AcdPrjProjectType");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                projectTypes = JsonConvert.DeserializeObject<List<AcdPrjProjectType>>(json);
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to fetch project type data.";
            }

            return View(projectTypes);
        }

        // ✅ GET: Add/Edit Page
        public async Task<IActionResult> ProjectTypeAddEdit(int? id)
        {
            if (id == null)
                return View(new AcdPrjProjectType());

            HttpResponseMessage response = await _client.GetAsync($"api/AcdPrjProjectType/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            string json = await response.Content.ReadAsStringAsync();
            var projectType = JsonConvert.DeserializeObject<AcdPrjProjectType>(json);

            return View(projectType);
        }

        // ✅ POST: Add or Edit
        [HttpPost]
        public async Task<IActionResult> ProjectTypeAddEdit(AcdPrjProjectType projectType)
        {
            if (!ModelState.IsValid)
                return View(projectType);

            if (projectType.ProjectTypeId == 0)
            {
                // Add
                var content = new StringContent(JsonConvert.SerializeObject(projectType), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("api/AcdPrjProjectType", content);

                if (response.IsSuccessStatusCode)
                    TempData["SuccessMessage"] = "Project Type added successfully.";
                else
                    TempData["ErrorMessage"] = "Failed to add Project Type.";
            }
            else
            {
                // Edit
                var content = new StringContent(JsonConvert.SerializeObject(projectType), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"api/AcdPrjProjectType/{projectType.ProjectTypeId}", content);

                if (response.IsSuccessStatusCode)
                    TempData["SuccessMessage"] = "Project Type updated successfully.";
                else
                    TempData["ErrorMessage"] = "Failed to update Project Type.";
            }

            return RedirectToAction("ProjectTypeList");
        }

        // ✅ DELETE
        public async Task<IActionResult> ProjectTypeDelete(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/AcdPrjProjectType/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Cannot delete project type. It may be linked to another table.";
                return RedirectToAction("ProjectTypeList");
            }

            TempData["SuccessMessage"] = "Project Type deleted successfully.";
            return RedirectToAction("ProjectTypeList");
        }
    }
}
