namespace Mc2.CrudTest.UnitTests.Domain.Customers;

using System;
using System.Collections.Generic;
using Mc2.CrudTest.Domain.Customers;
using NSubstitute;
using Xunit;

namespace Mc2.CrudTest.UnitTests.Domain.Customers
{
    public class CustomerServiceTests
    {
        [Fact]
        public void CreateCustomer_ValidCustomer_CallsRepositoryAdd()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerService = new CustomerService(customerRepository);
            var customer = new Customer("John", "Doe", new DateTime(1980, 1, 1), "+1234567890", "johndoe@example.com", "1234567890");

            // Act
            customerService.CreateCustomer(customer);

            // Assert
            customerRepository.Received(1).Add(customer);
        }

        [Fact]
        public void CreateCustomer_NullCustomer_ThrowsArgumentNullException()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerService = new CustomerService(customerRepository);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => customerService.CreateCustomer(null));
        }

        [Fact]
        public void GetCustomer_ExistingCustomerId_ReturnsCustomer()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerService = new CustomerService(customerRepository);
            var customer = new Customer("John", "Doe", new DateTime(1980, 1, 1), "+1234567890", "johndoe@example.com", "1234567890");
            customerRepository.GetById(customer.Id).Returns(customer);

            // Act
            var result = customerService.GetCustomer(customer.Id);

            // Assert
            Assert.Equal(customer, result);
        }

        [Fact]
        public void GetCustomer_NonExistingCustomerId_ReturnsNull()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerService = new CustomerService(customerRepository);
            var customerId = Guid.NewGuid();
            customerRepository.GetById(customerId).Returns((Customer)null);

            // Act
            var result = customerService.GetCustomer(customerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllCustomers_ReturnsAllCustomers()
        {
            // Arrange
            var customerRepository = Substitute.For<ICustomerRepository>();
            var customerService = new CustomerService(customerRepository);
            var customer1 = new Customer("John", "Doe", new DateTime(1980, 1, 1), "+1234567890", "johndoe@example.com", "1234567890");
            var customer2 = new Customer("Jane", "Doe", new DateTime(1985, 1, 1), "+2345678901", "janedoe@example.com", "2345678901");
            var customers = new List<Customer> { customer1, customer2 };
            customerRepository.GetAll().Returns(customers);

            // Act
            var result = customerService.GetAllCustomers();

            // Assert
            Assert.Equal(customers, result);
        }
    }
}
