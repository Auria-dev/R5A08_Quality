namespace R5A08_Client.Services.Implementation;

public class BrandWebService : ApiWebService<Models.BrandDto, Models.BrandCreateDto>
{
    public BrandWebService(HttpClient http) : base(http, "api/Brands") { }
}

