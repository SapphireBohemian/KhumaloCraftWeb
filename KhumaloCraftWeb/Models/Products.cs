using System.ComponentModel.DataAnnotations;

namespace KhumaloCraftWeb.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public bool Availability { get; set; }
    }
}
