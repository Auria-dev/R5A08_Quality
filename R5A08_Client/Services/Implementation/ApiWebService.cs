using System.Net.Http.Json;
using System.Text.Json;
using R5A08_Client.Services.Interface;

namespace R5A08_Client.Services.Implementation;

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

    public virtual async Task<TRead?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<TRead>($"{_endpoint}/{id}");
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
            var errorMsg = await ExtractErrorMessageAsync(response);
            throw new HttpRequestException(errorMsg);
        }

        return true;
    }

    public virtual async Task<bool> UpdateAsync<TUpdate>(int id, TUpdate entity)
    {
        var response = await _http.PutAsJsonAsync($"{_endpoint}/{id}", entity);
        if (!response.IsSuccessStatusCode)
        {
            var errorMsg = await ExtractErrorMessageAsync(response);
            throw new HttpRequestException(errorMsg);
        }

        return true;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"{_endpoint}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var errorMsg = await ExtractErrorMessageAsync(response);
            throw new HttpRequestException(errorMsg);
        }

        return true;
    }

    private static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content))
        {
            return $"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}";
        }

        try
        {
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            if (root.TryGetProperty("errors", out var errorsElement) && errorsElement.ValueKind == JsonValueKind.Object)
            {
                var errorList = new List<string>();
                foreach (var prop in errorsElement.EnumerateObject())
                {
                    if (prop.Value.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var errItem in prop.Value.EnumerateArray())
                        {
                            var msg = errItem.GetString();
                            if (!string.IsNullOrEmpty(msg))
                                errorList.Add(msg);
                        }
                    }
                }
                if (errorList.Count > 0)
                {
                    return string.Join(" ", errorList);
                }
            }

            if (root.TryGetProperty("title", out var titleElement) && titleElement.ValueKind == JsonValueKind.String)
            {
                var title = titleElement.GetString();
                if (!string.IsNullOrWhiteSpace(title)) return title;
            }

            if (root.TryGetProperty("message", out var msgElement) && msgElement.ValueKind == JsonValueKind.String)
            {
                var msg = msgElement.GetString();
                if (!string.IsNullOrWhiteSpace(msg)) return msg;
            }
        }
        catch
        {
            // Not JSON or unparseable
        }

        return content;
    }
}
