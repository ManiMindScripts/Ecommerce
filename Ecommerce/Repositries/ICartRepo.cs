using Ecommerce.Models.Entity;

namespace Ecommerce.Repositries
{
    public interface ICartRepo
    {
        Task<int> AddItem(int bookId, int qty);
        Task<int> RemoveItem(int bookId);
        Task<ShoppingCart> GetUserCart();
        Task<int> CartCount(string UserId = "");
        Task<ShoppingCart> GetCart(string UserId);
        Task<bool> DoCheckout(CheckOutModel model);

    }
}
