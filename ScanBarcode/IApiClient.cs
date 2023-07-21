using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScanBarcode.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace ScanBarcode
{
	public interface IApiClient
	{
		Task<ApiResponse> GetApiDataAsync(string apiUrl);
		Task<ApiResponse> PostApiDataAsync(string apiUrl, UserLogin data);
		Task<ApiResponse> GetApiInfoItemAsync(string apiUrl, ParamSearchByBarcode data, string token);
	}

	public class ApiClient : IApiClient
	{
		private readonly HttpClient _httpClient;

		public ApiClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<ApiResponse> GetApiDataAsync(string apiUrl)
		{
			var response = await _httpClient.GetAsync(apiUrl);

			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				// Xử lý nội dung phản hồi từ API (content) theo nhu cầu của bạn
				// Ví dụ: Deserialize JSON thành đối tượng hoặc trả về chuỗi kết quả
				return new ApiResponse(content);
			}

			// Xử lý khi có lỗi từ API, ví dụ: ghi log, trả về kết quả lỗi, ...
			return ApiResponse.Error();
		}
		public async Task<ApiResponse> PostApiDataAsync(string apiUrl, UserLogin data)
		{
			var json = JsonConvert.SerializeObject(data);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(apiUrl, content);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				// Xử lý nội dung phản hồi từ API (responseContent) theo nhu cầu của bạn
				return new ApiResponse(responseContent);
			}

			// Xử lý khi có lỗi từ API, ví dụ: ghi log, trả về kết quả lỗi, ...
			return ApiResponse.Error();
		}
		public async Task<ApiResponse> GetApiInfoItemAsync(string apiUrl, ParamSearchByBarcode data, string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var json = JsonConvert.SerializeObject(data);
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			string encodedJson = HttpUtility.UrlEncode(json);

			string urlWithParams = $"{apiUrl}?param={content}";

			var response = await _httpClient.GetAsync(urlWithParams);

			if (response.IsSuccessStatusCode)
			{
				var responseContent = await response.Content.ReadAsStringAsync();
				// Xử lý nội dung phản hồi từ API (responseContent) theo nhu cầu của bạn
				return new ApiResponse(responseContent);
			}

			// Xử lý khi có lỗi từ API, ví dụ: ghi log, trả về kết quả lỗi, ...
			return ApiResponse.Error();
		}
	}

	public class ApiResponse
	{
		public bool IsSuccess { get; set; }
		public string Content { get; set; }

		public ApiResponse(string content)
		{
			IsSuccess = true;
			Content = content;
		}

		public static ApiResponse Error()
		{
			return new ApiResponse(null)
			{
				IsSuccess = false
			};
		}
		
	}

	

}
