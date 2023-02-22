using AutoMapper;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Application.Mappings
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
        }
    }
}