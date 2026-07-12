using System.ComponentModel.DataAnnotations;

namespace ArtHouse.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

