using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.Marshalling;

namespace R508_Revisions.Model.EntityFramework
{
    [PrimaryKey(nameof(idTypeProduit))]
    [Table("t_e_typeproduit")]
    public class TypeProduit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("tpp_id")]
        public int idTypeProduit { get; set; }

        [Column("tpp_nom")]
        public required string nomTypeProduit { get; set; }

        [InverseProperty(nameof(Produit.idTypeProduitNavigation))]
        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();

    }
}
