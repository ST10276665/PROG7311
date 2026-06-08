using Microsoft.AspNetCore.Mvc;
using PROG7311.Models;
using PROG7311.Services;

namespace PROG7311.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApiService _apiService;

        public ClientsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _apiService.GetAsync<System.Collections.Generic.List<Client>>("/api/clients");
            return View(clients ?? new System.Collections.Generic.List<Client>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _apiService.GetAsync<Client>($"/api/clients/{id}");

            if (client == null) return NotFound();
            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                await _apiService.PostAsync<Client>("/api/clients", client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = await _apiService.GetAsync<Client>($"/api/clients/{id}");
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.ClientId) return NotFound();

            if (ModelState.IsValid)
            {
                await _apiService.PutAsync($"/api/clients/{id}", client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = await _apiService.GetAsync<Client>($"/api/clients/{id}");
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteAsync($"/api/clients/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}