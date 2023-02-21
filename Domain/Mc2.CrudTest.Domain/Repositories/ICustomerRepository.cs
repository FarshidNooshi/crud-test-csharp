using Mc2.CrudTest.Domain.Entities;

namespace Mc2.CrudTest.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer> GetByFullNameAndDateOfBirthAsync(string firstName, string lastName, DateTime dateOfBirth);
}