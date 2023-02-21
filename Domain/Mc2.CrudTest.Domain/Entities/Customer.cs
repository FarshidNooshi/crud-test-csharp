using System.ComponentModel.DataAnnotations;

namespace Mc2.CrudTest.Domain.Entities;

public class Customer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Bank account number is required.")]
    public string BankAccountNumber { get; set; }
}