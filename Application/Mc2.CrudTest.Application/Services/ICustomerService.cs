using Mc2.CrudTest.Application.Dtos;

namespace Mc2.CrudTest.Application.Services;

public interface ICustomerService
{
    Task<Guid> CreateCustomer(CustomerDto customerDto);
    Task<CustomerDto> GetCustomerById(Guid customerId);
    Task UpdateCustomer(CustomerDto customerDto);
    Task DeleteCustomer(Guid customerId);
}