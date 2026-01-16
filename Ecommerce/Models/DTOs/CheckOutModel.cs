using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models.DTOs
{
    public class CheckOutModel
    {
        [Required,MaxLength(30)]
        public string? Name { get; set;}
        [Required,MaxLength(30),EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? MobileNumber { get; set; }
        [Required,MaxLength(200)]
        public string? Address { get; set;}
        [Required]
        public string? Payment { get; set; }
    }
}
