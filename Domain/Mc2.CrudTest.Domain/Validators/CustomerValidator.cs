using FluentValidation;
using IbanNet;
using libphonenumber;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;

namespace Mc2.CrudTest.Domain.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        private readonly ICustomerRepository _repository;

        public CustomerValidator(ICustomerRepository repository)
        {
            _repository = repository;
        
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.").EmailAddress()
                .WithMessage("Email must be a valid email address.");
            RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required.");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required.")
                .Must(BeAValidMobileNumber).WithMessage("Phone number must be a valid mobile number.");
            RuleFor(x => x.BankAccountNumber).NotEmpty().WithMessage("Bank account number is required.")
                .Must(BeAValidBankAccountNumber).WithMessage("A valid bank account number is required.");
            RuleFor(x => x).MustAsync(BeUniqueCustomer)
                .WithMessage("A customer with the same first name, last name, and date of birth already exists.");
        }

        private bool BeAValidBankAccountNumber(Customer customer, string bankAccountNumber,
            ValidationContext<Customer> context)
        {
            var ibanValidator = new IbanValidator();
            var validationResult = ibanValidator.Validate(bankAccountNumber);
            if (validationResult.IsValid)
            {
                return true;
            }

            context.AddFailure("BankAccountNumber", "Invalid bank account number.");
            return false;
        }

        private async Task<bool> BeUniqueCustomer(Customer customer, CancellationToken cancellationToken)
        {
            var existingCustomer =
                await _repository.GetByFullNameAndDateOfBirthAsync(customer.FirstName, customer.LastName,
                    customer.DateOfBirth);
            return existingCustomer == null || existingCustomer.Id == customer.Id;
        }

        private bool BeAValidMobileNumber(Customer costumer, string phoneNumber, ValidationContext<Customer> context)
        {
            var phoneNumberUtil = PhoneNumberUtil.Instance;
            const string
                regionCode =
                    "US"; // Replace with your country's region code if you want to validate phone numbers for other countries
            try
            {
                var numberProto = phoneNumberUtil.Parse(phoneNumber, regionCode);
                if (!numberProto.IsValidNumber)
                {
                    context.AddFailure("PhoneNumber", "Phone number is invalid.");
                    return false;
                }

                if (!numberProto.IsPossibleNumber)
                {
                    context.AddFailure("PhoneNumber", "Phone number is not possible.");
                    return false;
                }

                if (numberProto.NumberType != PhoneNumberUtil.PhoneNumberType.FIXED_LINE_OR_MOBILE)
                {
                    context.AddFailure("PhoneNumber", "Phone number must be a valid mobile number.");
                    return false;
                }
            }
            catch (NumberParseException)
            {
                context.AddFailure("PhoneNumber", "Phone number is invalid.");
                return false;
            }

            return true;
        }
    }
}
