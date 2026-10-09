namespace R508_Revisions.Model.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ProductType { get; set; }
        public string? Brand { get; set; }
        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }
        public string? PhotoName { get; set; }
        public string? PhotoUri { get; set; }
        public int CurrentStock { get; set; }
        public int MinStock { get; set; }
        public int MaxStock { get; set; }
    }
}


