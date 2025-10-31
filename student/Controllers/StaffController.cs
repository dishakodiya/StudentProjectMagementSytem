 using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using student.Models;
using System.Text;

namespace student.Controllers
{
    public class StaffController : Controller
    {
        //private readonly HttpClient _client;
        //private readonly string _apiUrl = "https://localhost:7148/api/AcdStaff";

        //public StaffController(IHttpClientFactory httpClientFactory)
        //{
        //    _client = httpClientFactory.CreateClient();
        //}
        private readonly HttpClient _client;
        private readonly string _apiUrl = "https://localhost:7148/api/AcdStudent";

        public StaffController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7148/");
        }

        // ✅ GET: Staff List
        public async Task<IActionResult> StaffList()
        {
            List<AcdStaff> staffList = new();

            HttpResponseMessage response = await _client.GetAsync("api/AcdStaff");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                staffList = JsonConvert.DeserializeObject<List<AcdStaff>>(json);
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to fetch staff data.";
            }

            return View(staffList);
        }

        public async Task<IActionResult> StaffDelete(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/AcdStaff/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Cannot delete staff. It may be linked to another table.";
                return RedirectToAction("StaffList");
            }

            TempData["SuccessMessage"] = "Staff deleted successfully.";
            return RedirectToAction("StaffList");
        }

        // ✅ GET: Add/Edit page
        public async Task<IActionResult> StaffAddEdit(int? id)
        {
            if (id == null)
                return View(new AcdStaff()); // Add new staff

            // Fetch existing staff data
            HttpResponseMessage response = await _client.GetAsync($"api/AcdStaff/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var staff = JsonConvert.DeserializeObject<AcdStaff>(json);

            return View(staff);
        }

        // ✅ POST: Add or Edit
        [HttpPost]
        public async Task<IActionResult> StaffAddEdit(AcdStaff staff)
        {
            if (!ModelState.IsValid)
                return View(staff);

            if (staff.StaffId == 0)
            {
                // Add
                var content = new StringContent(JsonConvert.SerializeObject(staff), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("api/AcdStaff", content);

                if (response.IsSuccessStatusCode)
                    TempData["SuccessMessage"] = "Staff added successfully.";
                else
                    TempData["ErrorMessage"] = "Failed to add staff.";
            }
            else
            {
                // Edit
                var content = new StringContent(JsonConvert.SerializeObject(staff), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"api/AcdStaff/{staff.StaffId}", content);

                if (response.IsSuccessStatusCode)
                    TempData["SuccessMessage"] = "Staff updated successfully.";
                else
                    TempData["ErrorMessage"] = "Failed to update staff.";
            }

            return RedirectToAction("StaffList");
        }
    }
}
