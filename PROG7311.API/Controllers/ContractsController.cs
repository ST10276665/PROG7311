using Microsoft.AspNetCore.Mvc;
using PROG7311.API.Models;
using PROG7311.API.Repositories;

using Microsoft.AspNetCore.Authorization;

namespace PROG7311.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/contracts")]
    public class ContractsController : ControllerBase
    {
        private readonly IContractRepository _repository;

        public ContractsController(IContractRepository repository)
        {
            _repository = repository;
        }

        // GET /api/contracts?status=Active&startDate=2025-01-01
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? status,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var contracts = await _repository.GetAllAsync(status, startDate, endDate);
            return Ok(contracts);
        }

        // GET /api/contracts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _repository.GetByIdAsync(id);
            if (contract == null) return NotFound();
            return Ok(contract);
        }

        // POST /api/contracts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contract contract)
        {
            var created = await _repository.CreateAsync(contract);
            return CreatedAtAction(nameof(GetById), new { id = created.ContractId }, created);
        }

        // PATCH /api/contracts/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var updated = await _repository.UpdateStatusAsync(id, newStatus);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // PUT /api/contracts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Contract contract)
        {
            var updated = await _repository.UpdateAsync(id, contract);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE /api/contracts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}