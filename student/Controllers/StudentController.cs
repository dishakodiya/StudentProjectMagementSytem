using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using student.Models;
using System.Text;

namespace student.Controllers
{
    public class StudentController : Controller
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl = "https://localhost:7148/api/AcdStudent";

        public StudentController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7148/");
        }

        // ✅ GET: List
        public async Task<IActionResult> StudentList()
        {
            List<AcdStudent> students = new();

            HttpResponseMessage response = await _client.GetAsync("api/AcdStudent");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                students = JsonConvert.DeserializeObject<List<AcdStudent>>(json);
            }
            else
            {
                ViewBag.Error = "Unable to fetch student data.";
            }

            return View(students);
        }

        // ✅ DELETE
        public async Task<IActionResult> StudentDelete(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/AcdStudent/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Cannot delete student. It may be linked to another table.";
                return RedirectToAction("StudentList");
            }

            TempData["SuccessMessage"] = "Student deleted successfully.";
            return RedirectToAction("StudentList");
        }

        // ✅ GET: Add/Edit
        [HttpGet]
        public async Task<IActionResult> StudentAddEdit(int? id)
        {
            if (id == null)
                return View(new AcdStudent());  // Add mode

            HttpResponseMessage response = await _client.GetAsync($"api/AcdStudent/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<AcdStudent>(json);
                return View(student);
            }

            TempData["ErrorMessage"] = "Unable to load student details.";
            return RedirectToAction("StudentList");
        }

        // ✅ POST: Add or Update
        [HttpPost]
        public async Task<IActionResult> StudentAddEdit(AcdStudent student)
        {
            if (!ModelState.IsValid)
                return View(student);

            var jsonData = JsonConvert.SerializeObject(student);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (student.StudentId == 0)
            {
                // Add
                response = await _client.PostAsync("api/AcdStudent", content);
            }
            else
            {
                // Update
                response = await _client.PutAsync($"api/AcdStudent/{student.StudentId}", content);
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = student.StudentId == 0
                    ? "Student added successfully."
                    : "Student updated successfully.";
                return RedirectToAction("StudentList");
            }

            TempData["ErrorMessage"] = "Failed to save student data.";
            return View(student);
        }
    }
}

