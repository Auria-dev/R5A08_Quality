namespace R508_Blazor.Services.Implementation;

public class BrandWebService : ApiWebService<Models.BrandDto, Models.BrandCreateDto>
{
    public BrandWebService(HttpClient http) : base(http, "api/Brands") { }
}

