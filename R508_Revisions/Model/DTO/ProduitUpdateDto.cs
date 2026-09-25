namespace R508_Revisions.Model.DTO
{
    public class ProduitUpdateDto
    {
        public int IdProduit { get; set; }
        public string NomProduit { get; set; } = null!;
        public string? Description { get; set; }
        public string? NomPhoto { get; set; }
        public string? UriPhoto { get; set; }
        public int IdTypeProduit { get; set; }
        public int IdMarque { get; set; }
        public int StockReel { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
    }
}