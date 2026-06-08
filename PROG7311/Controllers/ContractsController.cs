using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROG7311.Models;
using PROG7311.Services;

namespace PROG7311.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApiService _apiService;
        private readonly FileService _fileService;

        public ContractsController(ApiService apiService, FileService fileService)
        {
            _apiService = apiService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(string? status, DateTime? startDate, DateTime? endDate)
        {
            var contracts = await _apiService.GetAsync<System.Collections.Generic.List<Contract>>("/api/contracts");

            var filtered = contracts ?? new System.Collections.Generic.List<Contract>();
            if (!string.IsNullOrEmpty(status))
                filtered = filtered.FindAll(c => c.Status == status);

            if (startDate.HasValue)
                filtered = filtered.FindAll(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                filtered = filtered.FindAll(c => c.EndDate <= endDate.Value);

            ViewBag.StatusFilter = status;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(filtered);
        }

        public async Task<IActionResult> Details(int id)
        {
            var contract = await _apiService.GetAsync<Contract>($"/api/contracts/{id}");

            if (contract == null) return NotFound();
            return View(contract);
        }

        public async Task<IActionResult> Create()
        {
            var clients = await _apiService.GetAsync<System.Collections.Generic.List<Client>>("/api/clients");
            ViewBag.Clients = new SelectList(clients ?? new System.Collections.Generic.List<Client>(), "ClientId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile? signedAgreement)
        {
            // Remove navigation properties from validation so the form can validate only scalar fields
            ModelState.Remove("Client");
            ModelState.Remove("ServiceRequests");

            if (signedAgreement != null)
            {
                try
                {
                    contract.SignedAgreementPath = await _fileService.SaveContractFileAsync(signedAgreement);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("SignedAgreement", ex.Message);
                }
            }

            if (ModelState.IsValid)
            {
                await _apiService.PostAsync<Contract>("/api/contracts", contract);
                return RedirectToAction(nameof(Index));
            }

            // Debug - log what's failing
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                }
            }

            var clients = await _apiService.GetAsync<System.Collections.Generic.List<Client>>("/api/clients");
            ViewBag.Clients = new SelectList(clients ?? new System.Collections.Generic.List<Client>(), "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _apiService.GetAsync<Contract>($"/api/contracts/{id}");
            if (contract == null) return NotFound();
            var clients = await _apiService.GetAsync<System.Collections.Generic.List<Client>>("/api/clients");
            ViewBag.Clients = new SelectList(clients ?? new System.Collections.Generic.List<Client>(), "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract, IFormFile? signedAgreement)
        {
            if (id != contract.ContractId) return NotFound();

            if (signedAgreement != null)
            {
                try
                {
                    contract.SignedAgreementPath = await _fileService.SaveContractFileAsync(signedAgreement);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("SignedAgreement", ex.Message);
                }
            }

            // Remove navigation properties from validation
            ModelState.Remove("Client");
            ModelState.Remove("ServiceRequests");

            if (ModelState.IsValid)
            {
                await _apiService.PutAsync($"/api/contracts/{id}", contract);
                return RedirectToAction(nameof(Index));
            }

            var clients = await _apiService.GetAsync<System.Collections.Generic.List<Client>>("/api/clients");
            ViewBag.Clients = new SelectList(clients ?? new System.Collections.Generic.List<Client>(), "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _apiService.GetAsync<Contract>($"/api/contracts/{id}");
            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteAsync($"/api/contracts/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}