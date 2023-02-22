using System.ComponentModel.DataAnnotations;

namespace Mc2.CrudTest.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string BankAccountNumber { get; set; }
}