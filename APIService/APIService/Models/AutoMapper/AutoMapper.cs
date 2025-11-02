using APIService.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Drawing;

namespace APIService.Models.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Accounts, AccountCUDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null))
                .ReverseMap();

            CreateMap<Accounts, AccountDTO>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null)).ReverseMap(); 
            CreateMap<AccountDTO, AccountCUDTO>();
            CreateMap<AccountCUDTO, Accounts>();


            CreateMap<Products, ProductDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ReverseMap();
            CreateMap<Products, ProductCUDTO>()
                     .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                     .ReverseMap();

            CreateMap<ProductVariants, ProductVariantDTO>()
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size != null ? src.Size.SizeName : null))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorName : null))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null))
                .ReverseMap();
            CreateMap<ProductVariants, ProductVariantCUDTO>()
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size != null ? src.Size.SizeName : null))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color != null ? src.Color.ColorName : null))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null))
                .ReverseMap();
            CreateMap<ProductVariantCUDTO, ProductVariants>();

            CreateMap<Colors, ColorDTO>();
            CreateMap<Colors, ColorCUDTO>();
            CreateMap<ColorCUDTO, Colors>();
            CreateMap<ColorDTO, Colors>();

            CreateMap<Sizes, SizeDTO>();
            CreateMap<Sizes, SizeCUDTO>();
            CreateMap<SizeCUDTO, Sizes>();
            CreateMap<SizeDTO, Sizes>();

            CreateMap<Categories, CategoryDTO>().ReverseMap();
            CreateMap<Categories, CategoryCUDTO>().ReverseMap();

            CreateMap<Payments, PaymentCUDTO>();

            CreateMap<Orders, OrderDTO>();
            CreateMap<OrderDTO, Orders>();
        }
    }
}
