namespace R5A08_Client.Services.Implementation;

public class ProductTypeWebService : ApiWebService<Models.ProductTypeDto, Models.ProductTypeCreateDto>
{
    public ProductTypeWebService(HttpClient http) : base(http, "api/ProductTypes") { }
}

