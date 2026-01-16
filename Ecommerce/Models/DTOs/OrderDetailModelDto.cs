using Ecommerce.Models.Entity;

namespace Ecommerce.Models.DTOs;

public class OrderDetailModelDto
{
    public string DivId { get; set; }
     public IEnumerable<OrderDetail> OrderDetail { get; set; }
}
