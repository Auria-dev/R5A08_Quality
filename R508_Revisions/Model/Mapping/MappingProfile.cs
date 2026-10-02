using AutoMapper;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;

namespace R508_Revisions.Model.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produit, ProduitDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idProduit))
                .ForMember(dest => dest.Nom,opt => opt.MapFrom(src => src.nomProduit))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.idTypeProduitNavigation != null ? src.idTypeProduitNavigation.nomTypeProduit : null))
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.idMarqueNavigation != null ? src.idMarqueNavigation.nomMarque : null)
            );

            CreateMap<Produit, ProduitDetailDto>()
                .ForMember(dest => dest.Id,opt => opt.MapFrom(src => src.idProduit))
                .ForMember(dest => dest.Nom,opt => opt.MapFrom(src => src.nomProduit))
                .ForMember(dest => dest.Nomphoto,opt => opt.MapFrom(src => src.nomPhoto))
                .ForMember(dest => dest.Uriphoto,opt => opt.MapFrom(src => src.uriPhoto))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.idTypeProduitNavigation != null ? src.idTypeProduitNavigation.nomTypeProduit : null) )
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.idMarqueNavigation != null ? src.idMarqueNavigation.nomMarque : null) )
                .ForMember(dest => dest.Stock,opt => opt.MapFrom(src => src.stockReel))
                .ForMember(dest => dest.EnReappro, opt => opt.MapFrom(src => src.stockReel < src.stockMin));

            CreateMap<ProduitUpdateDto, Produit>()
                .ForMember(dest => dest.idProduit,opt => opt.MapFrom(src => src.IdProduit))
                .ForMember(dest => dest.nomProduit,opt => opt.MapFrom(src => src.NomProduit))
                .ForMember(dest => dest.description,opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.nomPhoto,opt => opt.MapFrom(src => src.NomPhoto))
                .ForMember(dest => dest.uriPhoto,opt => opt.MapFrom(src => src.UriPhoto))
                .ForMember(dest => dest.idTypeProduit,opt => opt.MapFrom(src => src.IdTypeProduit))
                .ForMember(dest => dest.idMarque,opt => opt.MapFrom(src => src.IdMarque))
                .ForMember(dest => dest.stockReel,opt => opt.MapFrom(src => src.StockReel))
                .ForMember(dest => dest.stockMin,opt => opt.MapFrom(src => src.StockMin))
                .ForMember(dest => dest.stockMax,opt => opt.MapFrom(src => src.StockMax));
        }
    }
}
