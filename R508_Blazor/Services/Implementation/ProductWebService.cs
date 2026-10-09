namespace R508_Blazor.Services.Implementation;

public class ProductWebService : ApiWebService<Models.ProductDto, Models.ProductCreateDto>
{
    public ProductWebService(HttpClient http) : base(http, "api/Products") { }
}

