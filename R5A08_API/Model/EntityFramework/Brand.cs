using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R508_Revisions.Model.EntityFramework
{
    [PrimaryKey(nameof(idMarque))]
    [Table("t_e_marque")]
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("mrq_id")]
        public int idMarque { get; set; }

        [Column("mrq_nom")]
        public required string nomMarque { get; set; }

        [InverseProperty(nameof(Product.idMarqueNavigation))]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

