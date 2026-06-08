using Microsoft.AspNetCore.Mvc;
using PROG7311.Models;
using PROG7311.Services;
using System.Linq;

namespace PROG7311.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _apiService.GetAsync<System.Collections.Generic.List<Client>>("/api/clients");
            var contracts = await _apiService.GetAsync<System.Collections.Generic.List<Contract>>("/api/contracts");
            var requests = await _apiService.GetAsync<System.Collections.Generic.List<ServiceRequest>>("/api/servicerequests");

            var allClients = clients ?? new System.Collections.Generic.List<Client>();
            var allContracts = contracts ?? new System.Collections.Generic.List<Contract>();
            var allRequests = requests ?? new System.Collections.Generic.List<ServiceRequest>();

            ViewBag.TotalClients = allClients.Count;
            ViewBag.TotalContracts = allContracts.Count;
            ViewBag.TotalServiceRequests = allRequests.Count;
            ViewBag.ActiveContracts = allContracts.Count(c => c.Status == "Active");
            ViewBag.PendingRequests = allRequests.Count(r => r.Status == "Pending");
            ViewBag.RecentContracts = allContracts.OrderByDescending(c => c.ContractId).Take(5).ToList();
            return View();
        }
    }
}