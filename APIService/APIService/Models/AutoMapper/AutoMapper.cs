using APIService.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

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

            CreateMap<Products, ProductDTO>();
            CreateMap<ProductDTO, ProductCUDTO>();
            CreateMap<ProductCUDTO, Products>();
            CreateMap<Products, ProductCUDTO>();

            CreateMap<Categories, CategoryDTO>();

            CreateMap<Payments, PaymentCUDTO>();

            CreateMap<Orders, OrderDTO>();
            CreateMap<OrderDTO, Orders>();
        }
    }
}
