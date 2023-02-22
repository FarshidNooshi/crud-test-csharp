using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Customer customer)
    {
        await _dbContext.Customers.AddAsync(customer);
        await SaveChanges();
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        return await _dbContext.Customers.FindAsync(id);
    }

    public async Task<Customer> GetByFullNameAndDateOfBirthAsync(string firstName, string lastName, DateTime dateOfBirth)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(c =>
            c.FirstName == firstName && c.LastName == lastName && c.DateOfBirth == dateOfBirth);
    }

    public async Task UpdateAsync(Customer customer)
    {
        _dbContext.Entry(customer).State = EntityState.Modified;
        await SaveChanges();
    }


    public async Task DeleteAsync(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
        await SaveChanges();
    }

    public async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}