using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.DTOs
{
    public class GenreDto
    {
        public int id { get; set; }
        [Required,MaxLength(40)]
        public string GenreName { get; set;}
    }
}
