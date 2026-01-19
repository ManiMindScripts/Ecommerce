using Ecommerce.Models.Entity;

namespace Ecommerce.Repositries
{
    public interface IStockRepo
    {
        Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "");
        Task<Stock?> GetStockByBookId(int bookId);
        Task ManageStock(StockDto stockToManage);

    }
}