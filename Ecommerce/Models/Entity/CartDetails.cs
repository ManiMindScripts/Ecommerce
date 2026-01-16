using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models.Entity
{
    [Table("CartDetails")]
    public class CartDetails
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set;}
        public Book Book { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

    }
}
