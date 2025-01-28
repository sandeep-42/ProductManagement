using AutoMapper;
using ProductManagementAPI.Data.Entities;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.AutoMapper
{
    public class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            ProductEntityToProductModelMapping();
            ProductModelToProductEntityMapping();
        }

        private void ProductEntityToProductModelMapping()
        {
            CreateMap<Product, ProductModel>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.StockAvailability, opt => opt.MapFrom(src => src.StockAvailability));
        }

        private void ProductModelToProductEntityMapping()
        {
            CreateMap<ProductModel, Product>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.ProductDescription))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.StockAvailability, opt => opt.MapFrom(src => src.StockAvailability));
        }
    }
}
