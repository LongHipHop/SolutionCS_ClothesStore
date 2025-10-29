using APIService.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIService.Models.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Accounts, AccountDTO>();
            CreateMap<AccountDTO, AccountCUDTO>();
            CreateMap<AccountCUDTO, Accounts>();

            CreateMap<Products, ProductDTO>();
            CreateMap<ProductDTO, ProductCUDTO>();
            CreateMap<ProductCUDTO, Products>();

            CreateMap<Categories, CategoryDTO>();

            CreateMap<Payments, PaymentCUDTO>();

            CreateMap<Orders, OrderDTO>();
            CreateMap<OrderDTO, Orders>();
        }
    }
}
