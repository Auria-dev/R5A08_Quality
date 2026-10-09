namespace R5A08_Client.Models;

public class ProductDetailDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? PhotoName { get; set; }
    public string? PhotoUri { get; set; }
    public string? ProductType { get; set; }
    public string? Brand { get; set; }
    public int? Stock { get; set; }
    public bool IsRestocking { get; set; }
}

