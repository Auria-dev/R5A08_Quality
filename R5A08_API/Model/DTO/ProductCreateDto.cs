using System.ComponentModel.DataAnnotations;

namespace R508_Revisions.Model.DTO
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Photo name is required.")]
        [StringLength(100)]
        public string PhotoName { get; set; } = "default.jpg";

        [Required(ErrorMessage = "Photo URI is required.")]
        [StringLength(256)]
        public string PhotoUri { get; set; } = "/images/default.jpg";

        [Range(1, int.MaxValue, ErrorMessage = "Product type ID must be valid.")]
        public int ProductTypeId { get; set; } = 1;

        [Range(1, int.MaxValue, ErrorMessage = "Brand ID must be valid.")]
        public int BrandId { get; set; } = 1;

        public int CurrentStock { get; set; } = 0;
        public int MinStock { get; set; } = 0;
        public int MaxStock { get; set; } = 100;
    }
}

