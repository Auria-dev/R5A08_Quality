using CommunityToolkit.Mvvm.ComponentModel;
using R5A08_Client.Models;
using R5A08_Client.Services.Interface;

namespace R5A08_Client.ViewModels;

public sealed partial class ProductTypesViewModel : ObservableObject
{
    private readonly IService<ProductTypeDto, ProductTypeCreateDto> _service;

    public ProductTypesViewModel(IService<ProductTypeDto, ProductTypeCreateDto> service)
    {
        _service = service;
    }

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _message;
    [ObservableProperty] private IEnumerable<ProductTypeDto> _productTypes = Array.Empty<ProductTypeDto>();
    [ObservableProperty] private ProductTypeCreateDto _newProductType = new();

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            ProductTypes = await _service.GetAllAsync() ?? Array.Empty<ProductTypeDto>();
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

    public async Task AddProductTypeAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            await _service.AddAsync(NewProductType);
            NewProductType = new ProductTypeCreateDto();
            await LoadDataAsync();
            Message = "Product type successfully added!";
        }
        catch (Exception ex)
        {
            Message = $"Could not add product type: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}

