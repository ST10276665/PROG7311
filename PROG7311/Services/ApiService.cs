using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PROG7311.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient http, IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _httpContextAccessor = httpContextAccessor;
        }

        private void SetAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            SetAuthHeader();
            var resp = await _http.GetAsync(endpoint);
            if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Session expired. Please log in again.");
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                Console.WriteLine($"[API ERROR] GET {endpoint} returned {resp.StatusCode}: {body}");
                return default;
            }

            return await resp.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            SetAuthHeader();
            var resp = await _http.PostAsJsonAsync(endpoint, data);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                Console.WriteLine($"[API ERROR] {endpoint} returned {resp.StatusCode}: {body}");
                return default;
            }

            return await resp.Content.ReadFromJsonAsync<T>();
        }

        public async Task<bool> PutAsync(string endpoint, object data)
        {
            SetAuthHeader();
            var resp = await _http.PutAsJsonAsync(endpoint, data);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            SetAuthHeader();
            var resp = await _http.DeleteAsync(endpoint);
            return resp.IsSuccessStatusCode;
        }
    }
}
