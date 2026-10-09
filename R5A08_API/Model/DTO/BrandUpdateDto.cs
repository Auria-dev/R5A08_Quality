using System.ComponentModel.DataAnnotations;

namespace R508_Revisions.Model.DTO
{
    public class BrandUpdateDto
    {
        [Required]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}

