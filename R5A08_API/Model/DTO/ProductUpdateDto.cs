using System.ComponentModel.DataAnnotations;

namespace R508_Revisions.Model.DTO
{
    public class ProductUpdateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? PhotoName { get; set; }
        public string? PhotoUri { get; set; }
        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }
        public int CurrentStock { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
    }
}

