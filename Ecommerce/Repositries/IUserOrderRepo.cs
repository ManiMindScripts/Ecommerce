using Ecommerce.Models.Entity;

namespace Ecommerce.Repositries;

public interface IUserOrderRepo
{
    Task<IEnumerable<Order>> UserOrders(bool getAll = false);
    Task ChangeOrderStatus(UpdateOrderModelStatus data);
    Task TogglePaymentStatus(int orderId);
    Task<Order> GetOrderById(int id);
    Task<IEnumerable<OrderStatus>> GetOrderStatuses();
}