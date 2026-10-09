using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using R508_Blazor.Model;

namespace R508_Blazor.ViewModels;

internal sealed partial class WeatherViewModel : ObservableObject
{
    private readonly HttpClient _http;

    public WeatherViewModel(HttpClient http)
    {
        _http = http;
    }

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private WeatherForecast[]? _forecasts = Array.Empty<WeatherForecast>();

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            Forecasts = await _http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
        }
        finally
        {
            IsLoading = false;
        }
    }
}