using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models.Entity
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        public int OrderId { get; set;}
        public int Quantity { get; set; }
        public double Unitprice { get; set;}

        public Book Book { get; set; }
        public Order Order { get; set; }
    }
}
