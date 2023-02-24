using Mc2.CrudTest.Application.Dtos;
using MediatR;
using System;

namespace Mc2.CrudTest.Application.Queries;

public class GetCustomerQuery : IRequest<CustomerDto>
{
    public Guid Id { get; set; }

    public GetCustomerQuery(Guid id)
    {
        Id = id;
    }
}
