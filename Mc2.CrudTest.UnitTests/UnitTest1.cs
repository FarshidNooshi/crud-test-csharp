using Xunit;

namespace Mc2.CrudTest.UnitTests;

public class CustomerTests
{
    [Fact]
    public void CreateCustomerWithValidData()
    {
        // Arrange
        var customer = new Customer
        {
            Firstname = "John",
            Lastname = "Doe",
            DateOfBirth = new DateTime(1980, 1, 1),
            PhoneNumber = "+1-650-253-0000",
            Email = "johndoe@example.com",
            BankAccountNumber = "1234567890"
        };

        // Act
        var result = Customer.Create(customer);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNull(result.Error);
        Assert.IsNotNull(result.Value);
        Assert.IsTrue(result.Value.Id > 0);
    }

    [Fact]
    public void CreateCustomerWithInvalidData()
    {
        // Arrange
        var customer = new Customer
        {
            Firstname = "John",
            Lastname = "Doe",
            DateOfBirth = new DateTime(1980, 1, 1),
            PhoneNumber = "+1-650-253-0000",
            Email = "invalidemail",
            BankAccountNumber = "1234567890"
        };

        // Act
        var result = Customer.Create(customer);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsNotNull(result.Error);
        Assert.IsTrue(result.Error.Contains("Email is not valid."));
        Assert.IsNull(result.Value);
    }

    [Fact]
    public void RetrieveExistingCustomerById()
    {
        // Arrange
        var customer = new Customer
        {
            Firstname = "John",
            Lastname = "Doe",
            DateOfBirth = new DateTime(1980, 1, 1),
            PhoneNumber = "+1-650-253-0000",
            Email = "johndoe@example.com",
            BankAccountNumber = "1234567890"
        };
        var result = Customer.Create(customer);
        var customerId = result.Value.Id;

        // Act
        var retrievedCustomer = Customer.GetById(customerId);

        // Assert
        Assert.IsNotNull(retrievedCustomer);
        Assert.AreEqual(customerId, retrievedCustomer.Id);
        Assert.AreEqual("John", retrievedCustomer.Firstname);
        Assert.AreEqual("Doe", retrievedCustomer.Lastname);
        Assert.AreEqual(new DateTime(1980, 1, 1), retrievedCustomer.DateOfBirth);
        Assert.AreEqual("+1-650-253-0000", retrievedCustomer.PhoneNumber);
        Assert.AreEqual("johndoe@example.com", retrievedCustomer.Email);
        Assert.AreEqual("1234567890", retrievedCustomer.BankAccountNumber);
    }

    [Fact]
    public void RetrieveNonExistingCustomerById()
    {
        // Arrange
        var customerId = 123;

        // Act
        var retrievedCustomer = Customer.GetById(customerId);

        // Assert
        Assert.IsNull(retrievedCustomer);
    }

    [Fact]
    public void UpdateExistingCustomerWithValidData()
    {
        // Arrange
        var customer = new Customer
        {
            Firstname = "John",
            Lastname = "Doe",
            DateOfBirth = new DateTime(1980, 1, 1),
            PhoneNumber = "+1-650-253-0000",
            Email = "johndoe@example.com",
            BankAccountNumber = "1234567890"
        };
        var result = Customer.Create(customer);
        var customerId = result.Value.Id;
        var updatedCustomer = new Customer
        {
            Id = customerId,
            Firstname = "Jane",
            Lastname = "Doe",
            DateOfBirth = new DateTime(1985, 1, 1),
            PhoneNumber = "+1-650-253-0000",
            Email = "janedoe@example.com",
            BankAccountNumber = "1234567890"
        };

        // Act
        var updateResult = Customer.Update(updatedCustomer);

        // Assert
        Assert.IsTrue(updateResult.IsSuccess);
        Assert.IsNull(updateResult.Error);
        Assert.IsNotNull(updateResult.Value);
        Assert.AreEqual(customerId, updateResult.Value.Id);
        Assert.AreEqual("Jane", updateResult.Value.Firstname);
        Assert.AreEqual("Doe", updateResult.Value.Lastname);
        Assert.AreEqual(new DateTime(1985, 1, 1), updateResult.Value.DateOfBirth);
        Assert.AreEqual("+1-650-253-0000", updateResult.Value.PhoneNumber);
        Assert.AreEqual("janedoe@example.com", updateResult.Value.Email);
        Assert.AreEqual("1234567890", updateResult.Value.BankAccountNumber);
    }
}
