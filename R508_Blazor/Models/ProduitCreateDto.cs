using System.ComponentModel.DataAnnotations;

namespace R508_Blazor.Models;

public class ProduitCreateDto
{
    [Required(ErrorMessage = "Le nom du produit est requis.")]
    [StringLength(100, ErrorMessage = "Le nom doit avoir 100 caractères au maximum.")]
    public string NomProduit { get; set; } = string.Empty;

    public string? Description { get; set; }
    public string? NomPhoto { get; set; }
    public string? UriPhoto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Le type de produit est invalide.")]
    public int IdTypeProduit { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La marque est invalide.")]
    public int IdMarque { get; set; }

    public int StockReel { get; set; }
    public int StockMin { get; set; }
    public int StockMax { get; set; }
}