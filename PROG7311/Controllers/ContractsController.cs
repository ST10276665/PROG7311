using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG7311.Data;
using PROG7311.Models;
using PROG7311.Services;

namespace PROG7311.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly FileService _fileService;

        public ContractsController(ApplicationDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index(string? status, DateTime? startDate, DateTime? endDate)
        {
            var contracts = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (!string.IsNullOrEmpty(status))
                contracts = contracts.Where(c => c.Status == status);

            if (startDate.HasValue)
                contracts = contracts.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                contracts = contracts.Where(c => c.EndDate <= endDate.Value);

            ViewBag.StatusFilter = status;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(await contracts.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null) return NotFound();
            return View(contract);
        }

        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.Clients, "ClientId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile? signedAgreement)
        {
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
                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();
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

            ViewBag.Clients = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();
            ViewBag.Clients = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
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
                _context.Update(contract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = new SelectList(_context.Clients, "ClientId", "Name", contract.ClientId);
            return View(contract);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.ContractId == id);

            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}