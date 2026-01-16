using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepo _cartRepo;

        public CartController(ICartRepo cartRepo)
        {
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddItem(int bookId, int qty=1, int redirect = 0)
        {
            var cartCout = await _cartRepo.AddItem(bookId, qty);
            if(redirect == 0)
            return Ok(cartCout);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepo.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItem()
        {
            var cartItem = await _cartRepo.CartCount();
            return Ok(cartItem);
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckeOut = await _cartRepo.DoCheckout(model);
            if (!isCheckeOut)
                return RedirectToAction(nameof(OrderFailed));
            return RedirectToAction(nameof(OrderSuccess));
        }
        public IActionResult OrderSuccess()
        { 
            return View();
        }
        public IActionResult OrderFailed()
        {
            return View();
        }
    }
}
