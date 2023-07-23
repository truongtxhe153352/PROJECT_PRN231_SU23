using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using BusinessObjects.DTO;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace ManagerClient.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoursesController : Controller
    {
        private readonly HttpClient client = null;
        private string AdminApiUrl = "";

        public CoursesController()
        {
            AdminApiUrl = "http://localhost:5041/api/Admin";
            client = new HttpClient();
        }

        // GET: Admin/Courses
        public async Task<IActionResult> Index()
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(AdminApiUrl + "/GetAllCourse");
            string strData = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if(response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = response.StatusCode});
                }
            }
            List<CourseDto> courseDtos = JsonSerializer.Deserialize<List<CourseDto>>(strData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return View(courseDtos);
        }

        // GET: Admin/Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(AdminApiUrl + "/GetCourseById/" + id.ToString());
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = response.StatusCode });
                }
            }
            string strData = await response.Content.ReadAsStringAsync();
            CourseDto courseDto = JsonSerializer.Deserialize<CourseDto>(strData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(courseDto);
        }

        // GET: Admin/Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,CourseCode")] CourseDto courseDto)
        {
            if (ModelState.IsValid)
            {
                Course course = new Course
                {
                    CourseId = courseDto.CourseId,
                    CourseName = courseDto.CourseName,
                    CourseCode = courseDto.CourseCode
                };
                var strData = HttpContext.Request.Cookies["jwtToken"];
                if (string.IsNullOrEmpty(strData))
                {
                    return RedirectToAction("Index", "Login");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
                string jsonData = JsonSerializer.Serialize(course);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var postTask = await client.PostAsync(AdminApiUrl + "/InsertCourse",content);
                if (!postTask.IsSuccessStatusCode)
                {
                    if (postTask.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return RedirectToAction("Error", "Home", new { statusCodes = postTask.StatusCode });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courseDto);
        }

        // GET: Admin/Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(AdminApiUrl + "/GetCourseById/" + id.ToString());
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = response.StatusCode });
                }
            }
            string strData = await response.Content.ReadAsStringAsync();
            CourseDto courseDto = JsonSerializer.Deserialize<CourseDto>(strData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(courseDto);
        }

        // POST: Admin/Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,CourseCode")] CourseDto courseDto)
        {
            if (id != courseDto.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Course course = new Course
                {
                    CourseId = courseDto.CourseId,
                    CourseName = courseDto.CourseName,
                    CourseCode = courseDto.CourseCode
                };
                var strData2 = HttpContext.Request.Cookies["jwtToken"];
                if (string.IsNullOrEmpty(strData2))
                {
                    return RedirectToAction("Index", "Login");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
                string jsonData = JsonSerializer.Serialize(course);
                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var putTask = await client.PutAsync(AdminApiUrl + "/UpdateCourse", content);
                if (!putTask.IsSuccessStatusCode)
                {
                    if (putTask.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        return RedirectToAction("Error", "Home", new { statusCodes = putTask.StatusCode });
                    }
                }
                if (putTask.IsSuccessStatusCode)
                {
                    ViewData["msg"] = "success";
                }
                else
                {
                    ViewData["msg"] = "failed";
                }

                return RedirectToAction(nameof(Index));
            }
            return View(courseDto);
        }

        // GET: Admin/Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(AdminApiUrl + "/GetCourseById/" + id.ToString());
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = response.StatusCode });
                }
            }
            string strData = await response.Content.ReadAsStringAsync();
            CourseDto courseDto = JsonSerializer.Deserialize<CourseDto>(strData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(courseDto);
        }

        // POST: Admin/Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            var deleteTask = await client.DeleteAsync(AdminApiUrl + "/DeleteCourse/" + id.ToString());
            if (!deleteTask.IsSuccessStatusCode)
            {
                if (deleteTask.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = deleteTask.StatusCode });
                }
            }
            if (deleteTask.IsSuccessStatusCode)
            {
                ViewData["msg"] = "success";
            }
            else
            {
                ViewData["msg"] = "failed";
            }
            return RedirectToAction(nameof(Index));
        }

        
    }
}
