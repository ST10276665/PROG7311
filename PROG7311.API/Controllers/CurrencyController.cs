using Microsoft.AspNetCore.Mvc;
using PROG7311.API.Services;
using System.Threading.Tasks;

namespace PROG7311.API.Controllers
{
    [ApiController]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _service;

        public CurrencyController(CurrencyService service)
        {
            _service = service;
        }

        [HttpGet("usd-to-zar")]
        public async Task<IActionResult> GetUsdToZarRate()
        {
            var rate = await _service.GetUsdToZarRateAsync();
            return Ok(new { rate });
        }
    }
}
