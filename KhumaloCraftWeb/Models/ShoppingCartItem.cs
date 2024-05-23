namespace KhumaloCraftWeb.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public int ShoppingCartId { get; set; } // Foreign key to identify the shopping cart
        public ShoppingCart ShoppingCart { get; set; } // Navigation property
        public int ProductId { get; set; } // Foreign key to identify the product
        public Products Product { get; set; } // Navigation property
        public int Quantity { get; set; }
    }
}
