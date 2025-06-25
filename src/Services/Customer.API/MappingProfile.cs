using AutoMapper;
using Customer.API.Entities;
using Infrastructure.Mappings;
using Shared.DTOs.Customer;

namespace Customer.API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerEntity, CustomerDto>();
        CreateMap<CreateCustomerDto, CustomerEntity>();
        CreateMap<UpdateCustomerDto, CustomerEntity>()
            .IgnoreAllNonExisting() //[Extension method]: Ignore fields non existing when map
            .IgnoreNullProperties(); //[Extension method]: Ignore fields that has null value
    }
}
