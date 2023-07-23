using BusinessObjects.DTO;
using BusinessObjects.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ManagerClient.Controllers
{
    public class StudentController : Controller
    {

        private readonly HttpClient client = null;
        private string StudentApiUrl = "";

        public StudentController()
        {
            client = new HttpClient();
            StudentApiUrl = "http://localhost:5041/api/Student";
        }

        public async Task<IActionResult> ListCourse()
        {
            int studentId = 0;
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
            HttpResponseMessage responseMessage2 = await client.GetAsync(StudentApiUrl + "/GetStudentByEmail/" + email);
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
            studentId = user.UserId;
            HttpResponseMessage responseMessage = await client.GetAsync(StudentApiUrl + "/GetAllCourses/" + studentId.ToString());
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
            return View(courses);
        }

        public async Task<IActionResult> ListMaterialOfCourse(int id)
        {
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            HttpResponseMessage response = await client.GetAsync(StudentApiUrl + "/GetAllMaterialsByCourse/" + id.ToString());
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
            return View(materialDtos);
        }

        public async Task<IActionResult> DownloadMaterial(int id)
        {
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            HttpResponseMessage response = await client.GetAsync(StudentApiUrl + "/DowloadMaterial/" + id.ToString());
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("ErrorCode", "Home", new { statusCodes = response.StatusCode });
                }
            }
            var stream = await response.Content.ReadAsStreamAsync();
            var contentType = response.Content.Headers.ContentType.ToString();
            var fileName = response.Content.Headers.ContentDisposition.FileName.Trim('\"');
            return File(stream, contentType, fileName);
        }
        public async Task<IActionResult> ListAssignmentsByCourse(int id)
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(StudentApiUrl + "/GetAssignmentsByCourse/" + id.ToString());
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
        public IActionResult SubmitAssignment(int assid)
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
            SubmitAssignmentViewModel model = new SubmitAssignmentViewModel();
            model.AssignmentId = assid;
            model.UploaderId = uploaderId;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UploadSubmitPost(SubmitAssignmentViewModel submitAssignmentViewModel)
        {
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            var content = new MultipartFormDataContent();
            using var stream = submitAssignmentViewModel.SubmitFile.OpenReadStream();
            var streamContent = new StreamContent(stream);
            var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync());
            content.Add(fileContent, "file", submitAssignmentViewModel.SubmitFile.FileName);

            content.Add(new StringContent(submitAssignmentViewModel.AssignmentId.ToString()), "assignmentId");
            content.Add(new StringContent(submitAssignmentViewModel.UploaderId.ToString()), "uploaderId");

            var postTask = await client.PostAsync(StudentApiUrl + "/submitAssignment", content);
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
            return RedirectToAction(nameof(ListCourse));
        }
    }
}
