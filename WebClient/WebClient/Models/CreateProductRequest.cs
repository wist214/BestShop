using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
    public class CreateProductRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public int Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be non-negative.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Category Id is required.")]
        public int CategoryId { get; set; }
    }
}
