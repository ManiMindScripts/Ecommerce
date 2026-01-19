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
        public async Task<IActionResult> ManageStock(int bookId)
        {
            var existingStock = await _stockRepo.GetStockByBookId(bookId);
            var stock = new StockDto
            {
                BookId = bookId,
                Qunatity = existingStock != null ? existingStock.Quantity : 0
            };
            return View(stock);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStock(StockDto stock)
        {
            if (!ModelState.IsValid)
                return View(stock);
            try {
                await _stockRepo.ManageStock(stock);
                TempData["msg"] = "Stock Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["errmsg"] = "Somethnig went wrong";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
