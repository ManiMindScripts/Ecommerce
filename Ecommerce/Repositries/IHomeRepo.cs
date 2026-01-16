using Ecommerce.Models.Entity;

namespace Ecommerce
{
    public interface IHomeRepo
    {
        Task<IEnumerable<Book>> DisplayBooks(string sTerm="", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}