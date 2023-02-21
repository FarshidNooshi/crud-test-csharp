using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Domain.Repositories;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer);
    Task<Customer> GetByIdAsync(Guid id);
    Task<Customer> GetByFullNameAndDateOfBirthAsync(string firstName, string lastName, DateTime dateOfBirth);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Customer customer);
}
