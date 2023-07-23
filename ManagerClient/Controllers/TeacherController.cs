using BusinessObjects.DTO;
using BusinessObjects.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ManagerClient.Controllers
{
    public class TeacherController : Controller
    {
        private readonly HttpClient client = null;
        private string TeacherApiUrl = "";

        public TeacherController()
        {
            client = new HttpClient();
            TeacherApiUrl = "http://localhost:5041/api/Teacher";
        }
        public async Task<IActionResult> ListIndex()
        {
            int teacherId = 0;
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(strData);
            string email = "";
            foreach (var claim in jwtToken.Claims)
            {
                var type = claim.Type;
                if (claim.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"))
                {
                    email = claim.Value;
                }
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            HttpResponseMessage responseMessage2 = await client.GetAsync(TeacherApiUrl + "/getByEmail/" + email);
            if (!responseMessage2.IsSuccessStatusCode)
            {
                if (responseMessage2.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = responseMessage2.StatusCode });
                }
            }
            string strData2 = await responseMessage2.Content.ReadAsStringAsync();
            UserDto user = JsonSerializer.Deserialize<UserDto>(strData2, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            teacherId = user.UserId;
            HttpResponseMessage responseMessage = await client.GetAsync(TeacherApiUrl + "/Courses/" + teacherId.ToString());
            if (!responseMessage.IsSuccessStatusCode)
            {
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = responseMessage.StatusCode });
                }
            }
            string courseJson = await responseMessage.Content.ReadAsStringAsync();
            List<CourseDto> courses = JsonSerializer.Deserialize<List<CourseDto>>(courseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
            ViewData["teacherId"] = teacherId;
            return View(courses);
        }

        public async Task<IActionResult> ListMaterialOfCourse(int id, string msg)
        {
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            HttpResponseMessage response = await client.GetAsync(TeacherApiUrl + "/Materials/" + id.ToString());
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = response.StatusCode });
                }
            }
            string materialJson = await response.Content.ReadAsStringAsync();
            List<MaterialDto> materialDtos = JsonSerializer.Deserialize<List<MaterialDto>>(materialJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
            ViewData["courseId"] = id;
            ViewData["msg"] = msg;
            return View(materialDtos);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMaterial(List<IFormFile> materials, int courseId)
        {
            int uploaderId = 0;
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            if (!string.IsNullOrEmpty(strData))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(strData);

                foreach (var claim in jwtToken.Claims)
                {
                    var type = claim.Type;
                    if (claim.Type.Equals("nameid"))
                    {
                        uploaderId = int.Parse(claim.Value);
                    }
                }
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            if (materials == null || materials.Count <= 0)
            {
                return RedirectToAction("ListMaterialOfCourse", "Teacher", new { id = courseId });
            }


            var content = new MultipartFormDataContent();

            foreach (var material in materials)
            {
                using var stream = material.OpenReadStream();
                var streamContent = new StreamContent(stream);
                var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
                content.Add(fileContent, "files", material.FileName);
            }
            content.Add(new StringContent(courseId.ToString()), "courseId");
            content.Add(new StringContent(uploaderId.ToString()), "uploaderId");

            var postTask = await client.PostAsync(TeacherApiUrl + "/Materials", content);
            if (!postTask.IsSuccessStatusCode)
            {
                if (postTask.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = postTask.StatusCode });
                }
            }
            string msg = "";
            if (postTask.IsSuccessStatusCode)
            {
                msg = "success";
            }
            else
            {
                msg = "failed";
            }
            return RedirectToAction("ListMaterialOfCourse", "Teacher", new { id = courseId, msg = msg });
        }

        public async Task<IActionResult> DeleteMaterial(int materialId, int courseId)
        {
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            var deleteTask = await client.DeleteAsync(TeacherApiUrl + "/Materials/" + materialId.ToString());
            if (!deleteTask.IsSuccessStatusCode)
            {
                if (deleteTask.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = deleteTask.StatusCode });
                }
            }
            return RedirectToAction("ListMaterialOfCourse", "Teacher", new { id = courseId });
        }
        [HttpGet]
        public async Task<IActionResult> UploadAssignment()
        {
            int uploaderId = 0;
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            if (!string.IsNullOrEmpty(strData))
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtHandler.ReadJwtToken(strData);

                foreach (var claim in jwtToken.Claims)
                {
                    var type = claim.Type;
                    if (claim.Type.Equals("nameid"))
                    {
                        uploaderId = int.Parse(claim.Value);
                    }
                }
            }
            // co uploaderId
            //tao item
            ViewData["CourseId"] = await listCourseByUploaderId(uploaderId);
            UploadAssignmentViewModel model = new UploadAssignmentViewModel();
            model.UploaderId = uploaderId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UploadAssignmentPost(UploadAssignmentViewModel uploadAssignmentViewModel)
        {
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            var content = new MultipartFormDataContent();
            using var stream = uploadAssignmentViewModel.Assignment.OpenReadStream();
            var streamContent = new StreamContent(stream);
            var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
            content.Add(fileContent, "file", uploadAssignmentViewModel.Assignment.FileName);

            content.Add(new StringContent(uploadAssignmentViewModel.CourseId.ToString()), "courseId");
            content.Add(new StringContent(uploadAssignmentViewModel.UploaderId.ToString()), "uploaderId");

            var postTask = await client.PostAsync(TeacherApiUrl + "/Assignments", content);
            if (!postTask.IsSuccessStatusCode)
            {
                if (postTask.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = postTask.StatusCode });
                }
            }
            string msg = "";
            if (postTask.IsSuccessStatusCode)
            {
                msg = "success";
            }
            else
            {
                msg = "failed";
            }
            return RedirectToAction(nameof(Index));
        }
        private async Task<SelectList> listCourseByUploaderId(int uploaderId)
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage responseMessage = await client.GetAsync(TeacherApiUrl + "/Courses/" + uploaderId.ToString());
            string strData = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var courses = JsonSerializer.Deserialize<List<CourseDto>>(strData, options);
            return new SelectList(courses, "CourseId", "CourseName");
        }
        public async Task<IActionResult> ListAssignmentByCourse(int tid, int cid)
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(TeacherApiUrl + $"/Assignments/{tid}/{cid}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = response.StatusCode });
                }
            }
            var stream = await response.Content.ReadAsStreamAsync();
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<AssigmentDto> assigmentDtos = JsonSerializer.Deserialize<List<AssigmentDto>>(strData, options);
            return View(assigmentDtos);
        }

        public async Task<IActionResult> ListSubmitAssignmentByAssId(int assId)
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(TeacherApiUrl + $"/Assignments/{assId}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = response.StatusCode });
                }
            }
            var stream = await response.Content.ReadAsStreamAsync();
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<SubmitAssignmentDto> subAssigmentDtos = JsonSerializer.Deserialize<List<SubmitAssignmentDto>>(strData, options);
            return View(subAssigmentDtos);
        }
    }
}
