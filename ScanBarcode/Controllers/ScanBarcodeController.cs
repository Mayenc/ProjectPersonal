using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScanBarcode.Models;
using System.Net.Http;

namespace ScanBarcode.Controllers
{
    public class ScanBarcodeController : Controller
    {
		private readonly IApiClient _apiClient;
		public ScanBarcodeController(IApiClient apiClient)
		{
			_apiClient = apiClient;
		}
		//[Authorize(Roles = "Admin")]
		public IActionResult Index()
        {
			string token = Request.Cookies["token"];
			return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchByBarcode(string barcode) 
		{
			ParamSearchByBarcode pram = new ParamSearchByBarcode()
			{
				barcode = barcode
			};

			string token = Request.Cookies["token"];
			string apiUrl = "https://localhost:7047/api/InforStore/searchByBarcode"; // URL của API bạn muốn gọi
			
			ApiResponse response = await _apiClient.GetApiInfoItemAsync(apiUrl, pram, token);

			//ApiResponse response = await _apiClient.GetApiDataAsync(apiUrl);

			if (response.IsSuccess)
			{
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
