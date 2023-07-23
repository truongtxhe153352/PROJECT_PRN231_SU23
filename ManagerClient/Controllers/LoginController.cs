using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace ManagerClient.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private string UserApiUrl = "";
        public LoginController()
        {
            client = new HttpClient();
            UserApiUrl = "http://localhost:5041/api/User/Login";
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("logout")]

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            HttpResponseMessage response = await client.GetAsync(UserApiUrl + "/" + email + "/" + password);
            if (!response.IsSuccessStatusCode)
            {
                return View();
            }
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddMinutes(5);
            // lưu trữ và gửi lại cho server
            Response.Cookies.Append("jwtToken", strData, cookieOptions);

            // xử lí jwt
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtHandler.ReadJwtToken(strData);
            string role = "";
            foreach (var claim in jwtToken.Claims)
            {
                var type = claim.Type;
                if (claim.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role"))
                {
                    role = claim.Value;
                }
            }
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", strData);
            //HttpResponseMessage httpResponse2 = await client.GetAsync("http://localhost:5000/api/Student/TestApi");
            //var test = httpResponse2.IsSuccessStatusCode;
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //[Route("process")]
        //public IActionResult Process(string username, string myPassword)
        //{
        //    //TODO: connect with database
        //    if (username != null && myPassword != null && username.Equals("admin") && myPassword.Equals("123"))
        //    {
        //        HttpContext.Session.SetString("username", username);
        //        return View("Welcome");
        //    }
        //    else
        //    {
        //        ViewBag.error = "Invalid";
        //        return View("Index");
        //    }
        //}
    }
}
