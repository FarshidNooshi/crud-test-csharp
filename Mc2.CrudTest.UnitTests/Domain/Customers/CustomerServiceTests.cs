using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Application.Mappings;
using Mc2.CrudTest.Application.Services;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Exceptions;
using Mc2.CrudTest.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace Mc2.CrudTest.UnitTests.Domain.Customers;

public class CustomerServiceTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _customerRepository = Substitute.For<ICustomerRepository>();
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CustomerMappingProfile>();
        }));

        // Create a DI container and register the dependencies
        var services = new ServiceCollection()
            .AddSingleton(_customerRepository)
            .AddSingleton(_mapper)
            .AddScoped<CustomerService>();

        // Build the DI container and resolve the CustomerService instance
        var serviceProvider = services.BuildServiceProvider();
        _customerService = serviceProvider.GetService<CustomerService>();
    }

    [Fact]
    public void CreateCustomer_ValidData_ReturnsNewCustomerId()
    {
        // Arrange
        var newCustomerDto = new CustomerDto
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+14155552671",
            Email = "john.doe@example.com",
            BankAccountNumber = "DE89370400440532013000"
        };

        var newCustomerId = newCustomerDto.Id;
        _customerService.CreateCustomer(newCustomerDto).Returns(Task.FromResult(newCustomerId));

        // Act
        var result = _customerService.CreateCustomer(newCustomerDto).Result;

        // Assert
        Assert.Equal(newCustomerId, result);
    }


    [Fact]
    public async Task CreateCustomer_InvalidData_ThrowsValidationException()
    {
        // Arrange
        var newCustomer = new CustomerDto
        {
            FirstName = "",
            LastName = "",
            DateOfBirth = new DateTime(1900, 1, 1),
            PhoneNumber = "invalid phone number",
            Email = "invalid email",
            BankAccountNumber = "invalid bank account number"
        };

        // Act and Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(() => _customerService.CreateCustomer(newCustomer));
        Assert.Contains("Validation failed", ex.Message);

    }

    [Fact]
    public void GetCustomer_ExistingId_ReturnsCustomer()
    {
        // Arrange
        var existingCustomer = new Customer
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+14155552671",
            Email = "john.doe@example.com",
            BankAccountNumber = "DE89370400440532013000"
        };
        _customerRepository.GetByIdAsync(existingCustomer.Id).Returns(existingCustomer);

        // Act
        var result = _customerService.GetCustomerById(existingCustomer.Id).Result;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingCustomer.Id, result.Id);
        Assert.Equal(existingCustomer.FirstName, result.FirstName);
        Assert.Equal(existingCustomer.LastName, result.LastName);
        Assert.Equal(existingCustomer.DateOfBirth, result.DateOfBirth);
        Assert.Equal(existingCustomer.PhoneNumber, result.PhoneNumber);
        Assert.Equal(existingCustomer.Email, result.Email);
        Assert.Equal(existingCustomer.BankAccountNumber, result.BankAccountNumber);
    }

    [Fact]
    public async Task GetCustomer_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();
        _customerRepository.GetByIdAsync(nonExistingId).Returns(Task.FromResult((Customer)null));

        // Act and Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _customerService.GetCustomerById(nonExistingId));
        Assert.Equal("Customer not found.", ex.Message);
    }

    [Fact]
    public async Task UpdateCustomer_ValidData_SuccessfullyUpdates()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customerToUpdate = new Customer
        {
            Id = customerId,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+14155552671",
            Email = "john.doe@example.com",
            BankAccountNumber = "DE89370400440532013000"
        };
        var updatedCustomer = new CustomerDto {
            Id = customerId,
            FirstName = "Jane",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+14155552671",
            Email = "jane.doe@example.com",
            BankAccountNumber = "DE89370400440532013000"
        };
        _customerRepository.GetByIdAsync(customerId).Returns(Task.FromResult(customerToUpdate));

        // Act
        await _customerService.UpdateCustomer(updatedCustomer);

        // Assert
        await _customerRepository.Received(1).UpdateAsync(customerToUpdate);
    }


    [Fact]
    public async Task UpdateCustomer_InvalidData_ThrowsValidationException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customerToUpdate = new Customer {
            Id = customerId,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+14155552671",
            Email = "john.doe@example.com",
            BankAccountNumber = "DE89370400440532013000"
        };
        var updatedCustomer = new CustomerDto {
            Id = customerId,
            FirstName = "",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+14155552671",
            Email = "jane.doe@example.com",
            BankAccountNumber = "DE89370400440532013000"
        };
        _customerRepository.GetByIdAsync(customerId).Returns(Task.FromResult(customerToUpdate));

        // Assert
        await Assert.ThrowsAsync<ValidationException>(async () => {
            // Act
            await _customerService.UpdateCustomer(updatedCustomer);
        });
        
        _customerRepository.DidNotReceive().UpdateAsync(Arg.Any<Customer>());
    }


    [Fact]
    public void DeleteCustomer_ExistingId_DeletesCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
        {
            Id = customerId, 
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com"
        };
        _customerRepository.GetByIdAsync(customerId).Returns(Task.FromResult(customer));

        // Act
        _customerService.DeleteCustomer(customerId);

        // Assert
        _customerRepository.Received(1).DeleteAsync(customer);
    }

    [Fact]
    public async Task DeleteCustomer_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepository.GetByIdAsync(customerId).Returns(Task.FromResult((Customer)null));

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _customerService.DeleteCustomer(customerId));
        await _customerRepository.DidNotReceive().DeleteAsync(Arg.Any<Customer>());
    }
}