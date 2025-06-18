using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Product;

public abstract class CreateOrUpdateProductDto
{
    [Required]
    [MaxLength(250, ErrorMessage = "Maximum length for Product Name is 250 characters.")]
    public string Name { get; set; } = null!;

    [MaxLength(255, ErrorMessage = "Maximum length for Product Summary is 255 characters.")]
    public string Summary { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
}
