using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Domain.Entities;
using Mc2.CrudTest.Domain.Repositories;
using Mc2.CrudTest.Domain.Validators;

namespace Mc2.CrudTest.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateCustomer(CustomerDto customerDto)
        {
            var customer = _mapper.Map<Customer>(customerDto);
            var validator = new CustomerValidator(_customerRepository);
            validator.ValidateAndThrow(customer);
            await _customerRepository.AddAsync(customer);
            return customer.Id;
        }

        public async Task<CustomerDto> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task UpdateCustomer(CustomerDto customerDto)
        {
            var customer = await _customerRepository.GetByIdAsync(customerDto.Id);
            if (customer == null)
            {
                throw new ArgumentException($"Customer with id {customerDto.Id} not found.");
            }
            _mapper.Map(customerDto, customer);
            var validator = new CustomerValidator(_customerRepository);
            validator.ValidateAndThrow(customer);
            await _customerRepository.UpdateAsync(customer);
        }

        public async Task DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                throw new ArgumentException($"Customer with id {id} not found.");
            }
            await _customerRepository.DeleteAsync(customer);
        }
    }
}
