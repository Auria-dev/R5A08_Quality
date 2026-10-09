using System.ComponentModel.DataAnnotations;

namespace R508_Revisions.Model.DTO
{
    public class ProductTypeUpdateDto
    {
        [Required]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Product type name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}

