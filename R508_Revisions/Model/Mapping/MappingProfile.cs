using AutoMapper;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;

namespace R508_Revisions.Model.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idProduit))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.nomProduit))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.idTypeProduitNavigation != null ? src.idTypeProduitNavigation.nomTypeProduit : null))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.idMarqueNavigation != null ? src.idMarqueNavigation.nomMarque : null));

            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idProduit))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.nomProduit))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.PhotoName, opt => opt.MapFrom(src => src.nomPhoto))
                .ForMember(dest => dest.PhotoUri, opt => opt.MapFrom(src => src.uriPhoto))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.idTypeProduitNavigation != null ? src.idTypeProduitNavigation.nomTypeProduit : null))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.idMarqueNavigation != null ? src.idMarqueNavigation.nomMarque : null))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.stockReel))
                .ForMember(dest => dest.IsRestocking, opt => opt.MapFrom(src => src.stockReel < src.stockMin));

            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.idProduit, opt => opt.Ignore())
                .ForMember(dest => dest.nomProduit, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.nomPhoto, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.PhotoName) ? "default.jpg" : src.PhotoName))
                .ForMember(dest => dest.uriPhoto, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.PhotoUri) ? "/images/default.jpg" : src.PhotoUri))
                .ForMember(dest => dest.idTypeProduit, opt => opt.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.idMarque, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.stockReel, opt => opt.MapFrom(src => src.CurrentStock))
                .ForMember(dest => dest.stockMin, opt => opt.MapFrom(src => src.MinStock))
                .ForMember(dest => dest.stockMax, opt => opt.MapFrom(src => src.MaxStock))
                .ForMember(dest => dest.idMarqueNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.idTypeProduitNavigation, opt => opt.Ignore());

            CreateMap<ProductUpdateDto, Product>()
                .ForMember(dest => dest.idProduit, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.nomProduit, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.nomPhoto, opt => opt.MapFrom(src => src.PhotoName))
                .ForMember(dest => dest.uriPhoto, opt => opt.MapFrom(src => src.PhotoUri))
                .ForMember(dest => dest.idTypeProduit, opt => opt.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.idMarque, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.stockReel, opt => opt.MapFrom(src => src.CurrentStock))
                .ForMember(dest => dest.stockMin, opt => opt.MapFrom(src => src.MinStock))
                .ForMember(dest => dest.stockMax, opt => opt.MapFrom(src => src.MaxStock))
                .ForMember(dest => dest.idMarqueNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.idTypeProduitNavigation, opt => opt.Ignore());

            // Brand
            CreateMap<Brand, BrandDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idMarque))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.nomMarque));

            CreateMap<BrandCreateDto, Brand>()
                .ForMember(dest => dest.idMarque, opt => opt.Ignore())
                .ForMember(dest => dest.nomMarque, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<BrandUpdateDto, Brand>()
                .ForMember(dest => dest.idMarque, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.nomMarque, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            // ProductType
            CreateMap<ProductType, ProductTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idTypeProduit))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.nomTypeProduit));

            CreateMap<ProductTypeCreateDto, ProductType>()
                .ForMember(dest => dest.idTypeProduit, opt => opt.Ignore())
                .ForMember(dest => dest.nomTypeProduit, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<ProductTypeUpdateDto, ProductType>()
                .ForMember(dest => dest.idTypeProduit, opt => opt.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.nomTypeProduit, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}
