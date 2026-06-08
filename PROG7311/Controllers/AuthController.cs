using Microsoft.AspNetCore.Mvc;
using PROG7311.Models;
using PROG7311.Services;
using System.Threading.Tasks;

namespace PROG7311.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var resp = await _apiService.PostAsync<LoginResponse>("/api/auth/login", model);
            if (resp == null || string.IsNullOrEmpty(resp.Token))
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return View(model);
            }

            HttpContext.Session.SetString("JwtToken", resp.Token);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var resp = await _apiService.PostAsync<RegisterResponse>("/api/auth/register", model);
            if (resp == null)
            {
                ModelState.AddModelError(string.Empty, "Registration failed");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            return RedirectToAction("Login");
        }
    }
}
