using Microsoft.AspNetCore.Mvc;
using StockPricesDashboardAPI.Domain.Model;
using StockPricesDashboardAPI.Exceptions;
using StockPricesDashboardAPI.Services;

namespace StockPricesDashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly StockPriceService _stockService;

        public StockController(StockPriceService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{symbol}")]
        public async Task<ActionResult<IEnumerable<StockPrice>>> GetStockPrices(string symbol)
        {
            try
            {
                var stockPrices = await _stockService.GetStockPricesAsync(symbol);
                return Ok(stockPrices);
            }
            catch (ApiRateLimitExceededException ex)
            {
                return StatusCode(429, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
