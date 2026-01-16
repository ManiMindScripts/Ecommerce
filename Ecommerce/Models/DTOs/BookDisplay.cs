using Ecommerce.Models.Entity;
namespace Ecommerce.Models.DTOs
{
    public class BookDisplay
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Genre> Genres { get; set;}
        public string STerm { get; set; } = "";
        public int GenreId { get; set; } = 0;
    }
}
