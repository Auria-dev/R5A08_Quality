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
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private IEnumerable<ProductTypeDto> _productTypes = Array.Empty<ProductTypeDto>();
    [ObservableProperty] private ProductTypeCreateDto _newProductType = new();

    // Inline edit state
    [ObservableProperty] private int? _editingProductTypeId;
    [ObservableProperty] private ProductTypeUpdateDto _editingProductType = new();

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
            var data = await _service.GetAllAsync() ?? Array.Empty<ProductTypeDto>();
            ProductTypes = data;
            ApplySorting();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading product types: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
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
        if (ProductTypes == null || !ProductTypes.Any()) return;

        ProductTypes = SortColumn switch
        {
            "Id" => SortAscending ? ProductTypes.OrderBy(pt => pt.Id).ToList() : ProductTypes.OrderByDescending(pt => pt.Id).ToList(),
            "Name" => SortAscending ? ProductTypes.OrderBy(pt => pt.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList() : ProductTypes.OrderByDescending(pt => pt.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList(),
            _ => ProductTypes
        };
    }

    public string GetSortIcon(string columnName)
    {
        if (SortColumn != columnName) return "↕";
        return SortAscending ? "▲" : "▼";
    }

    public void StartEdit(ProductTypeDto type)
    {
        EditingProductTypeId = type.Id;
        EditingProductType = new ProductTypeUpdateDto
        {
            ProductTypeId = type.Id,
            Name = type.Name ?? string.Empty
        };
        ErrorMessage = null;
        Message = null;
    }

    public void CancelEdit()
    {
        EditingProductTypeId = null;
        EditingProductType = new ProductTypeUpdateDto();
    }

    public async Task UpdateProductTypeAsync()
    {
        if (EditingProductTypeId == null) return;

        if (string.IsNullOrWhiteSpace(EditingProductType.Name))
        {
            ErrorMessage = "Product type name cannot be empty.";
            return;
        }

        IsLoading = true;
        Message = null;
        ErrorMessage = null;
        try
        {
            await _service.UpdateAsync(EditingProductTypeId.Value, EditingProductType);
            CancelEdit();
            await LoadDataAsync();
            Message = "Product type successfully updated!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not update product type: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task DeleteProductTypeAsync(int id)
    {
        IsLoading = true;
        Message = null;
        ErrorMessage = null;
        try
        {
            await _service.DeleteAsync(id);
            if (EditingProductTypeId == id) CancelEdit();
            await LoadDataAsync();
            Message = "Product type deleted successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not delete product type: {ex.Message}";
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
        ErrorMessage = null;
        try
        {
            await _service.AddAsync(NewProductType);
            NewProductType = new ProductTypeCreateDto();
            await LoadDataAsync();
            Message = "Product type successfully added!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not add product type: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
