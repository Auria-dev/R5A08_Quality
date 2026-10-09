using System.ComponentModel.DataAnnotations;

namespace R5A08_Client.Models;

public class BrandCreateDto
{
    [Required(ErrorMessage = "Brand name is required.")]
    [StringLength(100, ErrorMessage = "Brand name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;
}

