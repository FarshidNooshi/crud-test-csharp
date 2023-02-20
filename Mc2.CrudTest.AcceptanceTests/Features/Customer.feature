Feature: Customer Management
  In order to manage customers
  As a salesperson
  I want to create, retrieve, update, and delete customers

  Background:
    Given the following customer exists in the system:
      | Id | Name | Email           |
      | 1  | John | john@test.com   |

  Scenario: Create a new customer with valid data
    Given I have entered valid customer data
    When I create a new customer
    Then the customer should be saved in the system
    And I should be able to retrieve the customer by their ID

  Scenario: Create a new customer with invalid data
    Given I have entered invalid customer data
    When I create a new customer
    Then the customer should not be saved in the system
    And I should receive an error message

  Scenario: Retrieve an existing customer by their ID
    Given I have the ID of an existing customer
    When I retrieve the customer by their ID
    Then the customer details should be returned
    And the details should match the data in the system

  Scenario: Attempt to retrieve a customer with an invalid ID
    Given I have an invalid customer ID
    When I attempt to retrieve the customer by their ID
    Then the customer details should not be returned
    And I should receive an error message

  Scenario: Update an existing customer with valid data
    Given I have the ID of an existing customer
    And I have entered valid customer data
    When I update the customer
    Then the customer details should be updated in the system
    And I should be able to retrieve the updated customer by their ID

  Scenario: Update a customer with invalid data
    Given I have the ID of an existing customer
    And I have entered invalid customer data
    When I update the customer
    Then the customer details should not be updated in the system
    And I should receive an error message

  Scenario: Delete an existing customer by their ID
    Given I have the ID of an existing customer
    When I delete the customer by their ID
    Then the customer should be deleted from the system
    And I should not be able to retrieve the customer by their ID

  Scenario: Attempt to delete a customer with an invalid ID
    Given I have an invalid customer ID
    When I attempt to delete the customer by their ID
    Then the customer should not be deleted from the system
    And I should receive an error message
