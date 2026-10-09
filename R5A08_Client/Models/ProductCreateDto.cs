using System.ComponentModel.DataAnnotations;

namespace R5A08_Client.Models;

public class ProductCreateDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    public string PhotoName { get; set; } = "default.jpg";
    public string PhotoUri { get; set; } = "/images/default.jpg";

    [Range(1, int.MaxValue, ErrorMessage = "Please select a product type.")]
    public int ProductTypeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a brand.")]
    public int BrandId { get; set; }

    public int CurrentStock { get; set; } = 0;
    public int MinStock { get; set; } = 0;
    public int MaxStock { get; set; } = 100;
}

