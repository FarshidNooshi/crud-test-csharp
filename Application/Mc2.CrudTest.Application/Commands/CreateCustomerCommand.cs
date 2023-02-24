using MediatR;
using System;
using Mc2.CrudTest.Application.Dtos;

namespace Mc2.CrudTest.Application.Commands;

public class CreateCustomerCommand : IRequest<Guid>
{
    public CustomerDto CustomerDto { get; set; }

    public CreateCustomerCommand(CustomerDto customerDto)
    {
        CustomerDto = customerDto;
    }
}