namespace Ecommerce.Models.DTOs
{
    public class StockDisplayModel
    {
        public int Id { get; set; }
        public int BookId { get; set;}
        public int Quantit { get; set; }
        public string? BookName { get; set;}
    }
}
