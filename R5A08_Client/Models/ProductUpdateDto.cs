using System.ComponentModel.DataAnnotations;

namespace R5A08_Client.Models;

public class ProductUpdateDto : IValidatableObject
{
    [Required]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    [MinLength(2, ErrorMessage = "Product name must be at least 2 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
    public string? Description { get; set; }

    public string? PhotoName { get; set; } = "default.jpg";
    public string? PhotoUri { get; set; } = "/images/default.jpg";

    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid product type.")]
    public int ProductTypeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid brand.")]
    public int BrandId { get; set; }

    [Range(0, 100000, ErrorMessage = "Current stock cannot be negative.")]
    public int CurrentStock { get; set; }

    [Range(0, 100000, ErrorMessage = "Min stock cannot be negative.")]
    public int MinStock { get; set; }

    [Range(0, 100000, ErrorMessage = "Max stock cannot be negative.")]
    public int MaxStock { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MinStock > MaxStock)
        {
            yield return new ValidationResult(
                "Minimum stock cannot be greater than maximum stock.",
                new[] { nameof(MinStock), nameof(MaxStock) }
            );
        }
    }
}

