using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepo _stockRepo;

        public StockController(IStockRepo stockRepo)
        {
            _stockRepo = stockRepo;
        }
        public async Task<IActionResult> Index(string sTerm ="")
        {
            var stocks = await _stockRepo.GetStocks(sTerm);
            return View(stocks);
        }
    }
}
