using BusinessObjects.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ManagerClient.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly HttpClient client = null;
        private string UserApiUrl = "";
        public HomeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5041/");
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }
        public async Task<IActionResult> Index()
        {
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData2))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            UserApiUrl = "http://localhost:5041/api/User";
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = response.StatusCode });
                }
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<UserDto>? listUserDtos = JsonSerializer.Deserialize<List<UserDto>>(strData, options);
            return View(listUserDtos);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //tao select item
            ViewData["RoleId"] = await listRole();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            //tao item
            ViewData["RoleId"] = await listRole();
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid ModelState";
                return View(userDto);
            }
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            HttpResponseMessage getData = await client.PostAsJsonAsync<UserDto>("api/User", userDto);
            if (!getData.IsSuccessStatusCode)
            {
                if (getData.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = getData.StatusCode });
                }
            }
            //getData.EnsureSuccessStatusCode();
            if (getData.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["ErrorMessage"] = "Create fail";
                return View(userDto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int uid)
        {
            ViewData["RoleId"] = await listRole();
            UserDto userDto = await getUserDTOById(uid);
            return View(userDto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDto userDto)
        {
            //tao item
            ViewData["RoleId"] = await listRole();
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "ModelState is invalid";
                return View(userDto);
            }
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            HttpResponseMessage getData = await client.PutAsJsonAsync<UserDto>($"api/User/{userDto.UserId}", userDto);
            if (!getData.IsSuccessStatusCode)
            {
                if (getData.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = getData.StatusCode });
                }
            }
            if (getData.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["ErrorMessage"] = "Edit fail";
                return View(userDto);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int uid)
        {
            string message = "";
            var strData = HttpContext.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(strData))
            {
                return RedirectToAction("Index", "Login");
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            HttpResponseMessage response = await client.DeleteAsync(
            $"api/User/{uid}");

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return RedirectToAction("Error", "Home", new { statusCodes = response.StatusCode });
                }
            }
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                message = "Delete fail";
            }
            return Content(message);
        }
        private async Task<SelectList> listRole()
        {
            //lay list category
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            UserApiUrl = "http://localhost:5041/api/Role/GetAllRoles";
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var roles = JsonSerializer.Deserialize<List<RoleDto>>(strData, options);
            return new SelectList(roles, "RoleId", "RoleName");
        }
        private async Task<UserDto> getUserDTOById(int uid)
        {
            //
            UserApiUrl = $"http://localhost:5041/api/User/{uid}";
            var strData2 = HttpContext.Request.Cookies["jwtToken"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData2);
            HttpResponseMessage response = await client.GetAsync(UserApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            //
            //Deserialize dữ liệu
            try
            {
                UserDto userDto = JsonSerializer.Deserialize<UserDto>(strData, options);
                return userDto;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IActionResult> Error(System.Net.HttpStatusCode statusCodes)
        {
            string status = statusCodes.ToString();
            return View("Error", status);
        }
    }
}
