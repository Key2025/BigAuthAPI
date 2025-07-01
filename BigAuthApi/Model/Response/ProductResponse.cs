namespace BigAuthApi.Model.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}