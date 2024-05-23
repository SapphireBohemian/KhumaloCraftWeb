namespace KhumaloCraftWeb.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public string UserId { get; set; } // Foreign key to identify the user
        public ApplicationUser User { get; set; } // Navigation property
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
