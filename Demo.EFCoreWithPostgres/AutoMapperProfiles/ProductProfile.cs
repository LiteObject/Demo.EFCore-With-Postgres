using AutoMapper;
using Demo.EFCoreWithPostgres.Domain;
using Demo.EFCoreWithPostgres.DTOs;

namespace Demo.EFCoreWithPostgres.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
