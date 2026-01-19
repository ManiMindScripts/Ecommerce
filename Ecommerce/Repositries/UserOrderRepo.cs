using Ecommerce.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositries
{
    public class UserOrderRepo : IUserOrderRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserOrderRepo(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task ChangeOrderStatus(UpdateOrderModelStatus data)
        {
            var order = await _db.Orders.FindAsync(data.orderId);
            if(order == null)
            {
                throw new InvalidOperationException($"Order with id: {data.orderId} does not found");
            }
            order.OrderStatusId = data.OrderStatusId;
            await _db.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _db.OrderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if(order == null)
            {
                throw new InvalidOperationException($"Order with id:{orderId} does not exist");
            }
            order.IsPaid = !order.IsPaid;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _db.Orders
                        .Include(x => x.OrderStatus)
                        .Include(x => x.OrderDetails)
                        .ThenInclude(x => x.Book)
                        .ThenInclude(x => x.Genre).AsQueryable();
            if (!getAll)
            {
                var UserId = GetUserId();
                if (string.IsNullOrEmpty(UserId))
                    throw new Exception("User is not Logges-in");
                orders = orders.Where(a => a.UserId == UserId);
                return await orders.ToListAsync();
            }
            return await orders.ToListAsync();
        }

        private string GetUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            string UserId = _userManager.GetUserId(user);
            return UserId;
        }
    }
}
