using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R508_Revisions.Model.EntityFramework
{
    [PrimaryKey(nameof(idTypeProduit))]
    [Table("t_e_typeproduit")]
    public class ProductType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tpp_id")]
        public int idTypeProduit { get; set; }

        [Column("tpp_nom")]
        public required string nomTypeProduit { get; set; }

        [InverseProperty(nameof(Product.idTypeProduitNavigation))]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

