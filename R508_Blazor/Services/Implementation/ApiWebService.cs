
using System.Net.Http.Json;
using R508_Blazor.Services.Interface;

namespace R508_Blazor.Services.Implementation;
public class ApiWebService<TRead, TCreate> : IService<TRead, TCreate>
    where TRead : class
    where TCreate : class 
{
    protected readonly HttpClient _http;
    protected readonly string _endpoint;

    public ApiWebService(HttpClient http, string endpoint)
    {
        _http = http;
        _endpoint = endpoint.TrimEnd('/');
    }

    public virtual async Task<IEnumerable<TRead>?> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<IEnumerable<TRead>>(_endpoint);
    }

    public virtual async Task<IEnumerable<TRead>?> SearchByNameAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term)) return await GetAllAsync();
        return await _http.GetFromJsonAsync<IEnumerable<TRead>>($"{_endpoint}/search/{Uri.EscapeDataString(term)}");
    }

    public virtual async Task<bool> AddAsync(TCreate entity)
    {
        var response = await _http.PostAsJsonAsync(_endpoint, entity);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content);
        }

        return true;
    }
}
