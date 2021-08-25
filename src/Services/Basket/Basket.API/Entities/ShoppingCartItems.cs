namespace Basket.API.Entities
{
    public class ShoppingCartItems
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        
    }
}