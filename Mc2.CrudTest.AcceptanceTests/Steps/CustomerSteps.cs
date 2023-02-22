using System;
using Mc2.CrudTest.Domain.Entities;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests.Steps;

[Binding]
public class CustomerManagementSteps
{
    private Customer _customer;
    private Customer _updatedCustomer;
    private ApiResponse<Customer> _apiResponse;
    private readonly ApiHelper _apiHelper;
    private int _invalidCustomerId;


    public CustomerManagementSteps()
    {
        _apiHelper = new ApiHelper();
    }

    [Given(@"the following customer exists in the system:")]
    public void GivenTheFollowingCustomerExistsInTheSystem(Table table)
    {
        _customer = table.CreateInstance<Customer>();
        _apiHelper.CreateCustomer(_customer);
    }

    [Given(@"I have entered valid customer data")]
    public void GivenIHaveEnteredValidCustomerData()
    {
        // Set the fields of the updated customer
        _updatedCustomer = new Customer { Id = Guid.NewGuid(), FirstName = "Jane", Email = "jane@test.com" };
    }

    [Given(@"I have entered invalid customer data")]
    public void GivenIHaveEnteredInvalidCustomerData()
    {
        _customer = new Customer { FirstName = "Jane", Email = "invalidemail" };
    }

    [Given(@"I have the ID of an existing customer")]
    public void GivenIHaveTheIDOfAnExistingCustomer()
    {
        _customer = new Customer { Id = Guid.NewGuid() };
    }

    [Given(@"I have an invalid customer ID")]
    public void GivenIHaveAnInvalidCustomerID()
    {
        // Assign an invalid customer ID that does not exist in the system
        _invalidCustomerId = 9999;
    }

    [When(@"I create a new customer")]
    public void WhenICreateANewCustomer()
    {
        _apiResponse = _apiHelper.CreateCustomer(_customer);
    }

    [When(@"I retrieve the customer by their ID")]
    public void WhenIRetrieveTheCustomerByTheirID()
    {
        _apiResponse = _apiHelper.GetCustomerById(_customer.Id);
    }

    [When(@"I update the customer")]
    public void WhenIUpdateTheCustomer()
    {
        _apiResponse = _apiHelper.UpdateCustomer(_customer.Id, _customer);
    }

    [Then(@"the customer should be saved in the system")]
    public void ThenTheCustomerShouldBeSavedInTheSystem()
    {
        Assert.True(_apiResponse.IsSuccess);
        Assert.NotNull(_apiResponse.Data);
    }

    [Then(@"the customer should not be saved in the system")]
    public void ThenTheCustomerShouldNotBeSavedInTheSystem()
    {
        Assert.False(_apiResponse.IsSuccess);
        Assert.Null(_apiResponse.Data);
    }

    [Then(@"the customer details should be returned")]
    public void ThenTheCustomerDetailsShouldBeReturned()
    {
        Assert.True(_apiResponse.IsSuccess);
        Assert.NotNull(_apiResponse.Data);
    }

    [Then(@"the customer details should not be returned")]
    public void ThenTheCustomerDetailsShouldNotBeReturned()
    {
        Assert.False(_apiResponse.IsSuccess);
        Assert.Null(_apiResponse.Data);
    }

    [Then(@"the details should match the data in the system")]
    public void ThenTheDetailsShouldMatchTheDataInTheSystem()
    {
        var customerInSystem = _apiHelper.GetCustomerById(_customer.Id);
        Assert.Equal(_customer.Name, customerInSystem.Data.Name);
        Assert.Equal(_customer.Email, customerInSystem.Data.Email);
    }

    [Then(@"the customer details should be updated in the system")]
    public void ThenTheCustomerDetailsShouldbeUpdatedInTheSystem()
    {
// Check if the customer has been updated in the system
        var updatedCustomer = _apiHelper.GetCustomerById(_customer.Id).Data;
        Assert.NotNull(updatedCustomer);
        Assert.Equal(_updatedCustomer.FirstName, updatedCustomer.FirstName);
        Assert.Equal(_updatedCustomer.Email, updatedCustomer.Email);
    }

    [Then(@"I should be able to retrieve the updated customer by their ID")]
    public void ThenIShouldBeAbleToRetrieveTheUpdatedCustomerByTheirID()
    {
        // Retrieve the updated customer by ID
        var retrievedCustomer = _apiHelper.GetCustomerById(_customer.Id).Data;

        Assert.NotNull(retrievedCustomer);
        Assert.Equal(_updatedCustomer.FirstName, retrievedCustomer.FirstName);
        Assert.Equal(_updatedCustomer.Email, retrievedCustomer.Email);
    }

    [When(@"I delete the customer by their ID")]
    public void WhenIDeleteTheCustomerByTheirID()
    {
        _apiResponse = _apiHelper.DeleteCustomerById(_customer.Id);
    }

    [Then(@"the customer should be deleted from the system")]
    public void ThenTheCustomerShouldBeDeletedFromTheSystem()
    {
        // Check if the customer has been deleted from the system
        var deletedCustomer = _apiHelper.GetCustomerById(_customer.Id).Data;

        Assert.Null(deletedCustomer);
    }

    [Then(@"I should not be able to retrieve the customer by their ID")]
    public void ThenIShouldNotBeAbleToRetrieveTheCustomerByTheirID()
    {
        // Retrieve the deleted customer by ID
        var retrievedCustomer = _apiHelper.GetCustomerById(_customer.Id).Data;

        Assert.Null(retrievedCustomer);
    }

    [When(@"I attempt to delete the customer by their ID")]
    public void WhenIAttemptToDeleteTheCustomerByTheirID()
    {
        _apiResponse = _apiHelper.DeleteCustomerById(_invalidCustomerId);
    }

    [Then(@"the customer should not be deleted from the system")]
    public void ThenTheCustomerShouldNotBeDeletedFromTheSystem()
    {
        // Check if the customer with invalid ID has not been deleted from the system
        var invalidCustomer = _apiHelper.GetCustomerById(_invalidCustomerId).Data;

        Assert.Null(invalidCustomer);
    }

    [Then(@"I should receive an error message")]
    public void ThenIShouldReceiveAnErrorMessage()
    {
        Assert.NotNull(_apiResponse.ErrorMessage);
    }
}