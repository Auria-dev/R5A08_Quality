using System.ComponentModel.DataAnnotations;

namespace R5A08_Client.Models;

public class ProductTypeUpdateDto
{
    [Required]
    public int ProductTypeId { get; set; }

    [Required(ErrorMessage = "Product type name is required.")]
    [StringLength(100, ErrorMessage = "Product type name cannot exceed 100 characters.")]
    [MinLength(2, ErrorMessage = "Product type name must be at least 2 characters.")]
    public string Name { get; set; } = string.Empty;
}

