using System.Net.Http.Json;
using R508_Blazor.Models;

namespace R508_Blazor.Services;

public class WSService : IService<ProduitDto, ProduitCreateDto>
{
    private readonly HttpClient _http;

    public WSService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IEnumerable<ProduitDto>?> GetAllAsync()
        => await _http.GetFromJsonAsync<IEnumerable<ProduitDto>>("api/Produit");

    public async Task<IEnumerable<ProduitDto>?> SearchByNameAsync(string term)
        => await _http.GetFromJsonAsync<IEnumerable<ProduitDto>>(
            $"api/Produit/SearchProduits/{Uri.EscapeDataString(term)}");

    public async Task<bool> AddAsync(ProduitCreateDto entity)
    {
        var response = await _http.PostAsJsonAsync("api/Produit", entity);
        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(await response.Content.ReadAsStringAsync());

        return true;
    }
}