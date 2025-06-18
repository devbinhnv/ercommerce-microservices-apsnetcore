using AutoMapper;
using Infrastructure.Mappings;
using Product.API.Entities;
using Shared.DTOs.Product;

namespace Product.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CatalogProduct, ProductDto>();
        CreateMap<CreateProductDto, CatalogProduct>();
        CreateMap<UpdateProductDto, CatalogProduct>()
            .IgnoreAllNonExisting() //[Extension method]: Ignore fields non existing when map
            .IgnoreNullProperties(); //[Extension method]: Ignore fields that has null value
    }
}
