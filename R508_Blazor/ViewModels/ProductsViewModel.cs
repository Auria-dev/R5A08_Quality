using CommunityToolkit.Mvvm.ComponentModel;
using R508_Blazor.Models;
using R508_Blazor.Services;

namespace R508_Blazor.ViewModels;

public sealed partial class ProductsViewModel : ObservableObject
{
    private readonly IService<ProduitDto, ProduitCreateDto> _service;

    public ProductsViewModel(IService<ProduitDto, ProduitCreateDto> service)
    {
        _service = service;
    }

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string? _message;
    [ObservableProperty] private IEnumerable<ProduitDto>? _produits = Array.Empty<ProduitDto>();
    [ObservableProperty] private string _searchName = string.Empty;
    [ObservableProperty] private ProduitCreateDto _nouveauProduit = new();

    public async Task LoadDataAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            Produits = await _service.GetAllAsync() ?? Array.Empty<ProduitDto>();
        }
        catch (Exception ex)
        {
            Message = $"Erreur : {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
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
            Produits = await _service.SearchByNameAsync(SearchName) ?? Array.Empty<ProduitDto>();

            if (!Produits.Any())
                Message = "Aucun produit trouvé avec ce nom.";
        }
        catch (Exception ex)
        {
            Message = $"Erreur : {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task AddProduitAsync()
    {
        IsLoading = true;
        Message = null;
        try
        {
            await _service.AddAsync(NouveauProduit); // throws with the API's error body on failure

            NouveauProduit = new ProduitCreateDto();
            await LoadDataAsync();                   // resets Message, so set ours after
            Message = "Produit ajouté avec succès !";
        }
        catch (Exception ex)
        {
            Message = $"Impossible d'ajouter le produit : {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}