using CommunityToolkit.Mvvm.ComponentModel;
using R5A08_Client.Models;
using R5A08_Client.Services.Interface;

namespace R5A08_Client.ViewModels;

public sealed partial class BrandsViewModel : ObservableObject
{
    private readonly IService<BrandDto, BrandCreateDto> _service;

    public BrandsViewModel(IService<BrandDto, BrandCreateDto> service)
    {
        _service = service;
    }

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _message;
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private IEnumerable<BrandDto> _brands = Array.Empty<BrandDto>();
    [ObservableProperty] private BrandCreateDto _newBrand = new();

    // Inline edit state
    [ObservableProperty] private int? _editingBrandId;
    [ObservableProperty] private BrandUpdateDto _editingBrand = new();

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
            var data = await _service.GetAllAsync() ?? Array.Empty<BrandDto>();
            Brands = data;
            ApplySorting();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading brands: {ex.Message}";
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
        if (Brands == null || !Brands.Any()) return;

        Brands = SortColumn switch
        {
            "Id" => SortAscending ? Brands.OrderBy(b => b.Id).ToList() : Brands.OrderByDescending(b => b.Id).ToList(),
            "Name" => SortAscending ? Brands.OrderBy(b => b.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList() : Brands.OrderByDescending(b => b.Name ?? "", StringComparer.OrdinalIgnoreCase).ToList(),
            _ => Brands
        };
    }

    public string GetSortIcon(string columnName)
    {
        if (SortColumn != columnName) return "↕";
        return SortAscending ? "▲" : "▼";
    }

    public void StartEdit(BrandDto brand)
    {
        EditingBrandId = brand.Id;
        EditingBrand = new BrandUpdateDto
        {
            BrandId = brand.Id,
            Name = brand.Name ?? string.Empty
        };
        ErrorMessage = null;
        Message = null;
    }

    public void CancelEdit()
    {
        EditingBrandId = null;
        EditingBrand = new BrandUpdateDto();
    }

    public async Task UpdateBrandAsync()
    {
        if (EditingBrandId == null) return;

        if (string.IsNullOrWhiteSpace(EditingBrand.Name))
        {
            ErrorMessage = "Brand name cannot be empty.";
            return;
        }

        IsLoading = true;
        Message = null;
        ErrorMessage = null;
        try
        {
            await _service.UpdateAsync(EditingBrandId.Value, EditingBrand);
            CancelEdit();
            await LoadDataAsync();
            Message = "Brand successfully updated!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not update brand: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task DeleteBrandAsync(int id)
    {
        IsLoading = true;
        Message = null;
        ErrorMessage = null;
        try
        {
            await _service.DeleteAsync(id);
            if (EditingBrandId == id) CancelEdit();
            await LoadDataAsync();
            Message = "Brand deleted successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not delete brand: {ex.Message}";
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
        ErrorMessage = null;
        try
        {
            await _service.AddAsync(NewBrand);
            NewBrand = new BrandCreateDto();
            await LoadDataAsync();
            Message = "Brand successfully added!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Could not add brand: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
