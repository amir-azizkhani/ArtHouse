using System.ComponentModel.DataAnnotations;

namespace ArtHouse.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<Product>? Products { get; set; }

    }
}

