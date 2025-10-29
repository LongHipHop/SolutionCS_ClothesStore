using CS_ClothesStore.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CS_ClothesStore.Models.AutoMapper
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
        }
    }
}
