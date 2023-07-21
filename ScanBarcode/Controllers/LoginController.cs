using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScanBarcode.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ScanBarcode.Controllers
{
    public class LoginController : Controller
    {
	
		private readonly IApiClient _apiClient;
		public LoginController(IApiClient apiClient)
		{
			_apiClient = apiClient;
		}
		public IActionResult Index()
        {
            return View();
        }
		[HttpPost("login")]
		public async Task<IActionResult> Index(string Username, string Password)
		{
			UserLogin data = new UserLogin
			{
				username = Username,
				password = Password,
				device = "3e71ccf4bf18847c"
				// Các thuộc tính khác của đối tượng MyData
			};
			string apiUrl = "https://localhost:7047/api/Authenticate/login"; // URL của API bạn muốn gọi

			ApiResponse response = await _apiClient.PostApiDataAsync(apiUrl, data);

			//ApiResponse response = await _apiClient.GetApiDataAsync(apiUrl);

			if (response.IsSuccess)
			{
				var jsonObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
				string Token = jsonObject["token"];
				string RefeshToken = jsonObject["refeshToken"];
				string Expiration = jsonObject["expiration"];
				string DefaultLocation = jsonObject["defaultLocation"];
				Response.Cookies.Append("token", Token, new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.Strict
				});
				//string[] userRoles = jsonObject["userRoles"];
				//HttpContext.Session.SetString("token", Token);
				//HttpContext.Session.SetString("refeshToken", RefeshToken);
				//HttpContext.Session.SetString("expiration", Expiration);
				//HttpContext.Session.SetString("defaultLocation", DefaultLocation);
				//HttpContext.Session.SetString("userRoles", userRoles);
				// Xử lý kết quả thành công từ API
				return LocalRedirect("~/ScanBarcode/Index");
			}
			else
			{
				// Xử lý lỗi từ API
				return View();
			}
		}
	}
}
