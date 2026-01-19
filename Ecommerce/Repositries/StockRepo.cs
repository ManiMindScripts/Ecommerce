using Ecommerce.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositries
{
    public class StockRepo
    {
        private readonly ApplicationDbContext _context;

        public StockRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Stock?> GetStockByBookId(int bookId) =>
            await _context.Stocks.FirstOrDefaultAsync(s => s.BookId == bookId);
        public async Task<IEnumerable<StockDisplayModel>> GetStocks(string sTerm = "")
        {
            var stocks = await (from book in _context.Books
                                join Stock in _context.Stocks
                                on book.Id equals Stock.BookId
                                into book_stock
                                from bookStock in book_stock.DefaultIfEmpty()
                                where string.IsNullOrWhiteSpace(sTerm) || book.BookName.ToLower().Contains(sTerm.ToLower())
                                select new StockDisplayModel
                                {
                                    BookId = book.Id,
                                    BookName = book.BookName,
                                    Quantit = bookStock != null ? 0 : bookStock.Quantity,
                                }).ToListAsync();
            return stocks;
        }
    }
    
}
