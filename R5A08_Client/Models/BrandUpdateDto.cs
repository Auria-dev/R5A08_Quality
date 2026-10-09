using System.ComponentModel.DataAnnotations;

namespace R5A08_Client.Models;

public class BrandUpdateDto
{
    [Required]
    public int BrandId { get; set; }

    [Required(ErrorMessage = "Brand name is required.")]
    [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters.")]
    [MinLength(2, ErrorMessage = "Brand name must be at least 2 characters.")]
    public string Name { get; set; } = string.Empty;
}

