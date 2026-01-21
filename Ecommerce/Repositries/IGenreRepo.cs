using Ecommerce.Models.Entity;

namespace Ecommerce.Repositries
{
    public interface IGenreRepo
    {
        Task AddGenre(Genre genre);
        Task UpdateGenre(Genre genre);
        Task<Genre> GetGenreById(int Id);
        Task DeleteGenre(Genre genre);
        Task<IEnumerable<Genre>> GetGenres();
    }
}