using System.ComponentModel.DataAnnotations;

namespace R508_Revisions.Model.DTO
{
    public class ProductTypeCreateDto
    {
        [Required(ErrorMessage = "Product type name is required.")]
        [StringLength(100, ErrorMessage = "Product type name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}

