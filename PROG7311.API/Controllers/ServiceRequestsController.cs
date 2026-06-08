using Microsoft.AspNetCore.Mvc;
using PROG7311.API.Models;
using PROG7311.API.Repositories;
using PROG7311.API.Services;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace PROG7311.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/servicerequests")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestRepository _repo;
        private readonly ServiceRequestService _service;

        public ServiceRequestsController(IServiceRequestRepository repo, ServiceRequestService service)
        {
            _repo = repo;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _repo.GetAllAsync();
            return Ok(all);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            var (success, error, created) = await _service.CreateAsync(request);
            if (!success) return BadRequest(new { error });

            return CreatedAtAction(nameof(GetById), new { id = created!.ServiceRequestId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceRequest request)
        {
            var updated = await _repo.UpdateAsync(id, request);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
