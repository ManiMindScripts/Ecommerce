using Ecommerce.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositries
{
    public class CartRepo : ICartRepo
    {
        private readonly ApplicationDbContext _applicationDb;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepo(ApplicationDbContext applicationDb,IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _applicationDb = applicationDb;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public  async Task<int> AddItem(int bookId, int qty)
        {
            string UserId = GetUserId();
            using var transaction = _applicationDb.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(UserId))
                    throw new UnauthorizedAccessException("User is not logged in");
                var cart = await GetCart(UserId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = UserId,
                    };
                    _applicationDb.Add(cart);
                }
                _applicationDb.SaveChanges();
                //Cart Detail start from here
                var cartItem = _applicationDb.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if(cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var book = _applicationDb.Books.Find(bookId);
                    cartItem = new CartDetails
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity= qty,
                        UnitPrice = book.Price
                    };
                    _applicationDb.Add(cartItem);
                }
                _applicationDb.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex)
            {}
            var cartItemCount = await CartCount(UserId);
            return cartItemCount;
        }
        public async Task<int> RemoveItem(int bookId)
        {
            //using var transaction = _applicationDb.Database.BeginTransaction();
            string UserId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(UserId))
                    throw new UnauthorizedAccessException("User is not logged-In");
                var cart = await GetCart(UserId);
                if (cart is null)
                    throw new InvalidOperationException("Cart is Empty");
                _applicationDb.SaveChanges();
                //Cart Detail start from here
                var cartItem = _applicationDb.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if(cartItem is null)
                    throw new InvalidOperationException("No Items in Cart");
              else if (cartItem.Quantity ==1)
                    _applicationDb.CartDetails.Remove(cartItem);
                else
                cartItem.Quantity = cartItem.Quantity - 1;
                _applicationDb.SaveChanges();  
            }
            catch (Exception ex)
            { }
            var cartItemCount = await CartCount(UserId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var UserId = GetUserId();
            if (UserId == null)
                throw new InvalidOperationException("Invalid Userid");
            var shoppingCart = await _applicationDb.shoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Book)
                                  .ThenInclude(a => a.Stock)
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Book)
                                  .ThenInclude(a => a.Genre)
                                  .Where(a => a.UserId == UserId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<ShoppingCart> GetCart(string UserId)
        {
            var cart = await _applicationDb.shoppingCarts.FirstOrDefaultAsync(x => x.UserId == UserId);
            return cart;
        }
        public async Task<int> CartCount(string UserId= "")
        {
            if (string.IsNullOrEmpty(UserId))
            {
                UserId = GetUserId();
            }
            var data = await (from cart in _applicationDb.shoppingCarts
                              join cartDetail in _applicationDb.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == UserId
                              select new { cartDetail.Id }
                              ).ToListAsync();
            return data.Count;
        }
        public async Task<bool> DoCheckout(CheckOutModel model)
        {
            using var transaction = _applicationDb.Database.BeginTransaction();
            try {
                //Logic: move data from carddetail to order,order detail then we will remove card detail
                var UserId = GetUserId();
                if (string.IsNullOrEmpty(UserId))
                    throw new UnauthorizedAccessException("User is not Logges-in");
                var cart = await GetCart(UserId);
                if (cart is null)
                    throw new InvalidOperationException("Cart is empty");
                var cartDetail = _applicationDb.CartDetails
                                 .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is empty");
                var pendingRecord = _applicationDb.OrderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)

                    throw new InvalidOperationException("Order Status does not have pending status");
                var order = new Order
                {
                    UserId = UserId,
                    CreateDate = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    Payment = model.Payment,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatusId = pendingRecord.Id
                };
                _applicationDb.Orders.Add(order);
                _applicationDb.SaveChanges();
                    foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        Unitprice = item.UnitPrice
                    };
                    _applicationDb.OrderDetails.Add(orderDetail);
                    var stock = await _applicationDb.Stocks.FirstOrDefaultAsync(a => a.BookId == item.BookId);
                    if (stock is null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }
                    if(item.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"only {stock.Quantity} item(s) are available in stock");
                    }
                    stock.Quantity = item.Quantity;
                }
                _applicationDb.SaveChanges();
                _applicationDb.CartDetails.RemoveRange(cartDetail);
                _applicationDb.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        private string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            string UserId = _userManager.GetUserId(user);
            return UserId;
        }
    }
}
