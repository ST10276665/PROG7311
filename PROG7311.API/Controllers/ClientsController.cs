using Microsoft.AspNetCore.Mvc;
using PROG7311.API.Models;
using PROG7311.API.Repositories;

using Microsoft.AspNetCore.Authorization;

namespace PROG7311.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _repository;

        public ClientsController(IClientRepository repository)
        {
            _repository = repository;
        }

        // GET /api/clients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _repository.GetAllAsync();
            return Ok(clients);
        }

        // GET /api/clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        // POST /api/clients
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            var created = await _repository.CreateAsync(client);
            return CreatedAtAction(nameof(GetById), new { id = created.ClientId }, created);
        }

        // PUT /api/clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            var updated = await _repository.UpdateAsync(id, client);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE /api/clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}