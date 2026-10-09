using CommunityToolkit.Mvvm.ComponentModel;
using R508_Blazor.Models;
using R508_Blazor.Services.Interface;

namespace R508_Blazor.ViewModels;

public sealed partial class ProductsViewModel : ObservableObject
{
    private readonly IService<ProductDto, ProductCreateDto> _productService;
    private readonly IService<BrandDto, BrandCreateDto> _brandService;
    private readonly IService<ProductTypeDto, ProductTypeCreateDto> _productTypeService;

    public ProductsViewModel(
        IService<ProductDto, ProductCreateDto> productService,
        IService<BrandDto, BrandCreateDto> brandService,
        IService<ProductTypeDto, ProductTypeCreateDto> productTypeService)
    {
        _productService = productService;
        _brandService = brandService;
        _productTypeService = productTypeService;
    }

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _message;
    [ObservableProperty] private IEnumerable<ProductDto> _products = Array.Empty<ProductDto>();
    [ObservableProperty] private IEnumerable<BrandDto> _brands = Array.Empty<BrandDto>();
    [ObservableProperty] private IEnumerable<ProductTypeDto> _productTypes = Array.Empty<ProductTypeDto>();
    [ObservableProperty] private string _searchName = string.Empty;
    [ObservableProperty] private ProductCreateDto _newProduct = new();

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            var productsTask = _productService.GetAllAsync();
            var brandsTask = _brandService.GetAllAsync();
            var productTypesTask = _productTypeService.GetAllAsync();

            await Task.WhenAll(productsTask, brandsTask, productTypesTask);

            Products = await productsTask ?? Array.Empty<ProductDto>();
            Brands = await brandsTask ?? Array.Empty<BrandDto>();
            ProductTypes = await productTypesTask ?? Array.Empty<ProductTypeDto>();

            ResetNewProductDefaults();
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

    private void ResetNewProductDefaults()
    {
        NewProduct = new ProductCreateDto
        {
            PhotoName = "default.jpg",
            PhotoUri = "/images/default.jpg",
            BrandId = Brands.FirstOrDefault()?.Id ?? 0,
            ProductTypeId = ProductTypes.FirstOrDefault()?.Id ?? 0
        };
    }

    public async Task ResetAsync()
    {
        SearchName = string.Empty;
        await LoadDataAsync();
    }

    public async Task SearchByNameAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchName))
        {
            await LoadDataAsync();
            return;
        }

        IsLoading = true;
        Message = null;
        try
        {
            Products = await _productService.SearchByNameAsync(SearchName) ?? Array.Empty<ProductDto>();

            if (!Products.Any())
                Message = "No product found with this name.";
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

    public async Task AddProductAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            if (string.IsNullOrWhiteSpace(NewProduct.PhotoName))
                NewProduct.PhotoName = "default.jpg";
            if (string.IsNullOrWhiteSpace(NewProduct.PhotoUri))
                NewProduct.PhotoUri = "/images/default.jpg";

            await _productService.AddAsync(NewProduct);

            ResetNewProductDefaults();
            await LoadDataAsync();
            Message = "Product successfully added!";
        }
        catch (Exception ex)
        {
            Message = $"Could not add product: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}