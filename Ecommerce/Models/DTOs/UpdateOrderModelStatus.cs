using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.DTOs;

public class UpdateOrderModelStatus
{
    public int orderId { get; set; }
    [Required]
    public int OrderStatusId { get; set; }

    public IEnumerable<SelectListItem>? OrderStatusList { get; set; }
}
