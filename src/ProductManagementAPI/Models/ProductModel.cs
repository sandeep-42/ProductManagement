namespace ProductManagementAPI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        public required string ProductName { get; set; }

        public string? ProductDescription { get; set; }

        public decimal Price { get; set; }

        public int StockAvailability { get; set; }
    }
}
