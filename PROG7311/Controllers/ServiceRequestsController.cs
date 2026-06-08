using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROG7311.Models;
using PROG7311.Services;
using PROG7311.Patterns.Observers;
using PROG7311.Patterns.Strategies;

namespace PROG7311.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApiService _apiService;
        private readonly ServiceRequestValidator _validator;
        private readonly ContractNotifier _notifier;
        private readonly ManagerNotifier _managerNotifier;

        public ServiceRequestsController(
            ApiService apiService,
            ServiceRequestValidator validator,
            ContractNotifier notifier,
            ManagerNotifier managerNotifier)
        {
            _apiService = apiService;
            _validator = validator;
            _notifier = notifier;
            _managerNotifier = managerNotifier;
            _notifier.Subscribe(_managerNotifier);
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _apiService.GetAsync<System.Collections.Generic.List<ServiceRequest>>("/api/servicerequests");
            return View(requests ?? new System.Collections.Generic.List<ServiceRequest>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = await _apiService.GetAsync<ServiceRequest>($"/api/servicerequests/{id}");

            if (request == null) return NotFound();
            return View(request);
        }

        public async Task<IActionResult> Create()
        {
            var contractsRaw = await _apiService.GetAsync<System.Collections.Generic.List<Contract>>("/api/contracts");
            var contracts = (contractsRaw ?? new System.Collections.Generic.List<Contract>())
                .Select(c => new SelectListItem
                {
                    Value = c.ContractId.ToString(),
                    Text = $"{c.Client.Name} — {c.ServiceLevel} ({c.Status})"
                });

            ViewBag.Contracts = new SelectList(contracts, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            ModelState.Remove("Contract");

            var contract = await _apiService.GetAsync<Contract>($"/api/contracts/{request.ContractId}");

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Selected contract not found.");
            }
            else
            {
                var (isValid, errorMessage) = _validator.Validate(request, contract);
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                await _apiService.PostAsync<ServiceRequest>("/api/servicerequests", request);
                return RedirectToAction(nameof(Index));
            }

            var contractsRaw = await _apiService.GetAsync<System.Collections.Generic.List<Contract>>("/api/contracts");
            var contracts = (contractsRaw ?? new System.Collections.Generic.List<Contract>())
                .Select(c => new SelectListItem
                {
                    Value = c.ContractId.ToString(),
                    Text = $"{c.Client.Name} — {c.ServiceLevel} ({c.Status})",
                    Selected = c.ContractId == request.ContractId
                });

            ViewBag.Contracts = new SelectList(contracts, "Value", "Text");
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var request = await _apiService.GetAsync<ServiceRequest>($"/api/servicerequests/{id}");
            if (request == null) return NotFound();

            var contractsRaw = await _apiService.GetAsync<System.Collections.Generic.List<Contract>>("/api/contracts");
            var contracts = (contractsRaw ?? new System.Collections.Generic.List<Contract>())
                .Select(c => new SelectListItem
                {
                    Value = c.ContractId.ToString(),
                    Text = $"{c.Client.Name} — {c.ServiceLevel} ({c.Status})",
                    Selected = c.ContractId == request.ContractId
                });

            ViewBag.Contracts = new SelectList(contracts, "Value", "Text");
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceRequest request)
        {
            if (id != request.ServiceRequestId) return NotFound();

            ModelState.Remove("Contract");

            var contract = await _apiService.GetAsync<Contract>($"/api/contracts/{request.ContractId}");

            if (contract == null)
            {
                ModelState.AddModelError("ContractId", "Selected contract not found.");
            }
            else
            {
                var (isValid, errorMessage) = _validator.Validate(request, contract);
                if (!isValid)
                {
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                await _apiService.PutAsync($"/api/servicerequests/{id}", request);
                return RedirectToAction(nameof(Index));
            }

            var contractsRaw = await _apiService.GetAsync<System.Collections.Generic.List<Contract>>("/api/contracts");
            var contracts = (contractsRaw ?? new System.Collections.Generic.List<Contract>())
                .Select(c => new SelectListItem
                {
                    Value = c.ContractId.ToString(),
                    Text = $"{c.Client.Name} — {c.ServiceLevel} ({c.Status})",
                    Selected = c.ContractId == request.ContractId
                });

            ViewBag.Contracts = new SelectList(contracts, "Value", "Text");
            return View(request);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var request = await _apiService.GetAsync<ServiceRequest>($"/api/servicerequests/{id}");
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteAsync($"/api/servicerequests/{id}");
            return RedirectToAction(nameof(Index));
        }
    }
}