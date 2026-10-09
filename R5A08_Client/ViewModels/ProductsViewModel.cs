using CommunityToolkit.Mvvm.ComponentModel;
using R5A08_Client.Models;
using R5A08_Client.Services.Interface;

namespace R5A08_Client.ViewModels;

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
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private IEnumerable<ProductDto> _products = Array.Empty<ProductDto>();
    [ObservableProperty] private IEnumerable<BrandDto> _brands = Array.Empty<BrandDto>();
    [ObservableProperty] private IEnumerable<ProductTypeDto> _productTypes = Array.Empty<ProductTypeDto>();
    [ObservableProperty] private string _searchName = string.Empty;

    // Creation state
    [ObservableProperty] private ProductCreateDto _newProduct = new();
    [ObservableProperty] private bool _isAddingNew;

    // Full View / Editing state
    [ObservableProperty] private ProductDto? _selectedProduct;
    [ObservableProperty] private int? _editingProductId;
    [ObservableProperty] private ProductUpdateDto _editingProduct = new();

    // Sorting state
    [ObservableProperty] private string _sortColumn = "Id";
    [ObservableProperty] private bool _sortAscending = true;

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        Message = null;
        ErrorMessage = null;
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
            ApplySorting();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
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

    public void SortBy(string columnName)
    {
        if (SortColumn == columnName)
        {
            SortAscending = !SortAscending;
        }
        else
        {
            SortColumn = columnName;
            SortAscending = true;
        }
        ApplySorting();
    }

    private void ApplySorting()
    {
        if (Products == null || !Products.Any()) return;

        Products = SortColumn switch
        {
            "Id" => SortAscending ? Products.OrderBy(p => p.Id).ToList() : Products.OrderByDescending(p => p.Id).ToList(),
            "Name" => SortAscending ? Products.OrderBy(p => p.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList() : Products.OrderByDescending(p => p.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList(),
            "Description" => SortAscending ? Products.OrderBy(p => p.Description ?? "", StringComparer.OrdinalIgnoreCase).ToList() : Products.OrderByDescending(p => p.Description ?? "", StringComparer.OrdinalIgnoreCase).ToList(),
            "Brand" => SortAscending ? Products.OrderBy(p => p.Brand ?? "", StringComparer.OrdinalIgnoreCase).ToList() : Products.OrderByDescending(p => p.Brand ?? "", StringComparer.OrdinalIgnoreCase).ToList(),
            "ProductType" => SortAscending ? Products.OrderBy(p => p.ProductType ?? "", StringComparer.OrdinalIgnoreCase).ToList() : Products.OrderByDescending(p => p.ProductType ?? "", StringComparer.OrdinalIgnoreCase).ToList(),
            _ => Products
        };
    }

    public string GetSortIcon(string columnName)
    {
        if (SortColumn != columnName) return "↕";
        return SortAscending ? "▲" : "▼";
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
        ErrorMessage = null;
        try
        {
            Products = await _productService.SearchByNameAsync(SearchName) ?? Array.Empty<ProductDto>();
            ApplySorting();

            if (!Products.Any())
                Message = "No product found with this name.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Search error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void SelectProduct(ProductDto product)
    {
        SelectedProduct = product;
        IsAddingNew = false;

        int brandId = product.BrandId;
        if (brandId == 0)
        {
            brandId = Brands.FirstOrDefault(b => b.Name != null && b.Name.Equals(product.Brand, StringComparison.OrdinalIgnoreCase))?.Id 
                      ?? Brands.FirstOrDefault()?.Id ?? 0;
        }

        int typeId = product.ProductTypeId;
        if (typeId == 0)
        {
            typeId = ProductTypes.FirstOrDefault(pt => pt.Name != null && pt.Name.Equals(product.ProductType, StringComparison.OrdinalIgnoreCase))?.Id 
                     ?? ProductTypes.FirstOrDefault()?.Id ?? 0;
        }

        EditingProductId = product.Id;
        EditingProduct = new ProductUpdateDto
        {
            ProductId = product.Id,
            Name = product.Name ?? string.Empty,
            Description = product.Description ?? string.Empty,
            BrandId = brandId,
            ProductTypeId = typeId,
            PhotoName = string.IsNullOrWhiteSpace(product.PhotoName) ? "default.jpg" : product.PhotoName,
            PhotoUri = string.IsNullOrWhiteSpace(product.PhotoUri) ? "/images/default.jpg" : product.PhotoUri,
            CurrentStock = product.CurrentStock,
            MinStock = product.MinStock,
            MaxStock = product.MaxStock
        };

        Message = null;
        ErrorMessage = null;
    }

    public void StartAddNew()
    {
        IsAddingNew = true;
        SelectedProduct = null;
        EditingProductId = null;
        ResetNewProductDefaults();
        Message = null;
        ErrorMessage = null;
    }

    public void BackToList()
    {
        SelectedProduct = null;
        EditingProductId = null;
        IsAddingNew = false;
        Message = null;
        ErrorMessage = null;
    }

    public async Task UpdateProductAsync()
    {
        if (EditingProductId == null) return;

        IsLoading = true;
        Message = null;
        ErrorMessage = null;
        try
        {
            if (string.IsNullOrWhiteSpace(EditingProduct.PhotoName))
                EditingProduct.PhotoName = "default.jpg";
            if (string.IsNullOrWhiteSpace(EditingProduct.PhotoUri))
                EditingProduct.PhotoUri = "/images/default.jpg";

            await _productService.UpdateAsync(EditingProductId.Value, EditingProduct);

            // Update in-memory list representation
            await LoadDataAsync();

            var updatedItem = Products.FirstOrDefault(p => p.Id == EditingProductId.Value);
            if (updatedItem != null)
            {
                SelectedProduct = updatedItem;
            }

            Message = "Product changes saved successfully!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not save product changes: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        IsLoading = true;
        Message = null;
        ErrorMessage = null;
        try
        {
            await _productService.DeleteAsync(id);
            BackToList();
            await LoadDataAsync();
            Message = "Product deleted successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not delete product: {ex.Message}";
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
        ErrorMessage = null;
        try
        {
            if (string.IsNullOrWhiteSpace(NewProduct.PhotoName))
                NewProduct.PhotoName = "default.jpg";
            if (string.IsNullOrWhiteSpace(NewProduct.PhotoUri))
                NewProduct.PhotoUri = "/images/default.jpg";

            await _productService.AddAsync(NewProduct);

            BackToList();
            await LoadDataAsync();
            Message = "Product successfully added!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not add product: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}