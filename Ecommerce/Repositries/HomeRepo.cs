using Ecommerce.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositries
{
    public class HomeRepo : IHomeRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public HomeRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _dbContext.Genres.ToListAsync();
        }
        public async Task<IEnumerable<Book>> DisplayBooks(string sTerm="", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            var books = await (from book in
                         _dbContext.Books
                         join genre in _dbContext.Genres
                         on book.GenreId equals genre.Id
                         where
                         (
                         string.IsNullOrWhiteSpace(sTerm) ||(book !=null && book.BookName.ToLower().StartsWith(sTerm)) 
                         ) &&
                         (genreId == 0 || book.GenreId == genreId)
                               select new Book
                         {
                             Id = book.Id,
                             Image = book.Image,
                             AuthorName = book.AuthorName,
                             BookName = book.BookName,
                             GenreId = book.GenreId,
                             Price = book.Price,
                             GenreName = genre.GenreName,
                         }
                        ).ToListAsync();
            return books;
        }
    }
}
