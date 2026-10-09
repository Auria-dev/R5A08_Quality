namespace R508_Blazor.Services.Implementation;

public class ProductTypeWebService : ApiWebService<Models.ProductTypeDto, Models.ProductTypeCreateDto>
{
    public ProductTypeWebService(HttpClient http) : base(http, "api/ProductTypes") { }
}

