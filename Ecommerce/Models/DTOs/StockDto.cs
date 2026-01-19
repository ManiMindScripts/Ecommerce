using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.DTOs
{
    public class StockDto
    {
        public int BookId { get;set;}
        [Range(0, int.MaxValue,ErrorMessage ="Quantity must be a non negative number")]
        public int Qunatity { get; set;}
    }
}
