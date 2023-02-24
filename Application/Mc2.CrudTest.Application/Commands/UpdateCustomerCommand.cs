using Mc2.CrudTest.Application.Dtos;
using MediatR;

namespace Mc2.CrudTest.Application.Commands;

public class UpdateCustomerCommand : IRequest<Unit>
{
    public CustomerDto CustomerDto { get; set; }

    public UpdateCustomerCommand(CustomerDto customerDto)
    {
        CustomerDto = customerDto;
    }
}
