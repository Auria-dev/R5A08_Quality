using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace R508_Revisions.Model.EntityFramework
{
    [PrimaryKey(nameof(idProduit))]
    [Table("t_e_produit")]
    public class Produit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("prd_produit")]
        public int idProduit { get; set; }

        [Column("prd_nom")]
        [Required]
        [StringLength(100)]
        public required string nomProduit { get; set; }

        [Column("prd_description")]
        [Required]
        [StringLength(100)]
        public required string description { get; set; }

        [Column("prd_nomphoto")]
        [Required]
        [StringLength(100)]
        public required string nomPhoto { get; set; }

        [Column("prd_uriphoto")]
        [Required]
        [StringLength(256)]
        public required string uriPhoto { get; set; }

        [Column("prd_idtype")]
        public int idTypeProduit { get; set; }

        [Column("prd_idmarque")]
        public int idMarque { get; set; }

        [Column("prd_stockreel")]
        public int stockReel { get; set; }

        [Column("prd_stockmin")]
        public int stockMin { get; set; }

        [Column("prd_stockmax")]
        public int stockMax { get; set; }

        [ForeignKey(nameof(idTypeProduit))]
        [InverseProperty(nameof(Marque.Produits))]
        public virtual Marque idMarqueNavigation { get; set; } = null!;

        [ForeignKey(nameof(idMarque))]
        [InverseProperty(nameof(TypeProduit.Produits))]
        public virtual TypeProduit idTypeProduitNavigation { get; set; } = null!;
    }    
}
