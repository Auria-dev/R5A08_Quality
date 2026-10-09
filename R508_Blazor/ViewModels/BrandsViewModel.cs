using CommunityToolkit.Mvvm.ComponentModel;
using R508_Blazor.Models;
using R508_Blazor.Services.Interface;

namespace R508_Blazor.ViewModels;

public sealed partial class BrandsViewModel : ObservableObject
{
    private readonly IService<BrandDto, BrandCreateDto> _service;

    public BrandsViewModel(IService<BrandDto, BrandCreateDto> service)
    {
        _service = service;
    }

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _message;
    [ObservableProperty] private IEnumerable<BrandDto> _brands = Array.Empty<BrandDto>();
    [ObservableProperty] private BrandCreateDto _newBrand = new();

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            Brands = await _service.GetAllAsync() ?? Array.Empty<BrandDto>();
        }
        catch (Exception ex)
        {
            Message = $"Error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task AddBrandAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            await _service.AddAsync(NewBrand);
            NewBrand = new BrandCreateDto();
            await LoadDataAsync();
            Message = "Brand successfully added!";
        }
        catch (Exception ex)
        {
            Message = $"Could not add brand: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}

