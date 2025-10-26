using AutoMapper;
using TaskPulse.Core.DTOs;
using TaskPulse.Core.Entities;

namespace TaskPulse.Api.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
