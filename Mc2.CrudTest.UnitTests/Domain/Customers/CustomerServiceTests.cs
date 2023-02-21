using FluentValidation;
using Mc2.CrudTest.Domain.Entities;
using NSubstitute;
using Xunit;

namespace Mc2.CrudTest.UnitTests.Domain.Customers;

public class CustomerServiceTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _customerRepository = NSubstitute.Substitute.For<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepository);
    }

    [Fact]
    public void CreateCustomer_ValidData_ReturnsNewCustomer()
    {
        // Arrange
        var newCustomer = new Customer
        {
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+1234567890",
            Email = "john.doe@example.com",
            BankAccountNumber = "1234567890"
        };

        // Act
        var result = _customerService.CreateCustomer(newCustomer);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newCustomer.FirstName, result.Firstname);
        Assert.Equal(newCustomer.LastName, result.Lastname);
        Assert.Equal(newCustomer.DateOfBirth, result.DateOfBirth);
        Assert.Equal(newCustomer.PhoneNumber, result.PhoneNumber);
        Assert.Equal(newCustomer.Email, result.Email);
        Assert.Equal(newCustomer.BankAccountNumber, result.BankAccountNumber);
    }

    [Fact]
    public void CreateCustomer_InvalidData_ThrowsValidationException()
    {
        // Arrange
        var newCustomer = new Customer
        {
            FirstName = "",
            LastName = "",
            DateOfBirth = new DateTime(1900, 1, 1),
            PhoneNumber = "invalid phone number",
            Email = "invalid email",
            BankAccountNumber = "invalid bank account number"
        };

        // Act and Assert
        var ex = Assert.Throws<ValidationException>(() => _customerService.CreateCustomer(newCustomer));
        Assert.Equal("Validation error occurred.", ex.Message);
    }

    [Fact]
    public void GetCustomer_ExistingId_ReturnsCustomer()
    {
        // Arrange
        var existingCustomer = new Customer
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            PhoneNumber = "+1234567890",
            Email = "john.doe@example.com",
            BankAccountNumber = "1234567890"
        };
        _customerRepository.GetById(existingCustomer.Id).Returns(existingCustomer);

        // Act
        var result = _customerService.GetCustomer(existingCustomer.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingCustomer.Id, result.Id);
        Assert.Equal(existingCustomer.FirstName, result.Firstname);
        Assert.Equal(existingCustomer.LastName, result.Lastname);
        Assert.Equal(existingCustomer.DateOfBirth, result.DateOfBirth);
        Assert.Equal(existingCustomer.PhoneNumber, result.PhoneNumber);
        Assert.Equal(existingCustomer.Email, result.Email);
        Assert.Equal(existingCustomer.BankAccountNumber, result.BankAccountNumber);
    }

    [Fact]
    public void GetCustomer_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var nonExistingId = 1;
        _customerRepository.GetById(nonExistingId).Returns((Customer)null);

        // Act and Assert
        var ex = Assert.Throws<NotFoundException>(() => _customerService.GetCustomer(nonExistingId));
        Assert.Equal("Customer not found.", ex.Message);
    }

    [Fact]
    public void UpdateCustomer_ValidData_SuccessfullyUpdates()
    {
        // Arrange
        var customerId = 1;
        var customerToUpdate = new Customer { Id = customerId, FirstName = "John", LastName = "Doe", Age = 35 };
        var updatedCustomer = new Customer { Id = customerId, FirstName = "Jane", LastName = "Doe", Age = 40 };
        _customerRepository.GetById(customerId).Returns(customrToUpdate);

        // Act
        var result = _customerService.Update(updatedCustomer);

        // Assert
        Assert.True(result);
        _customerRepository.Received(1).SaveChanges();
    }

    [Fact]
    public void UpdateCustomer_InvalidData_ReturnsFalse()
    {
        // Arrange
        var customerId = 1;
        var customerToUpdate = new Customer { Id = customerId, FirstName = "John", LastName = "Doe", Age = 35 };
        var updatedCustomer = new Customer { Id = customerId, FirstName = "", LastName = "Doe", Age = 40 };
        _customerRepository.GetById(customerId).Returns(customerToUpdate);


        // Act
        var result = _customerService.Update(updatedCustomer);

        // Assert
        Assert.False(result);
        _customerRepository.DidNotReceive().SaveChanges();
    }

    [Fact]
    public void DeleteCustomer_ExistingId_DeletesCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new Customer
            { Id = customerId, FirstName = "John", LastName = "Doe", Email = "johndoe@example.com" };
        _customerRepository.GetById(customerId).Returns(customer);

        // Act
        _customerService.DeleteCustomer(customerId);

        // Assert
        _customerRepository.Received(1).Delete(customer);
    }

    [Fact]
    public void DeleteCustomer_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _customerRepository.GetById(customerId).Returns((Customer)null);

        // Act and Assert
        Assert.Throws<NotFoundException>(() => _customerService.DeleteCustomer(customerId));
        _customerRepository.DidNotReceive().Delete(Arg.Any<Customer>());
    }
}