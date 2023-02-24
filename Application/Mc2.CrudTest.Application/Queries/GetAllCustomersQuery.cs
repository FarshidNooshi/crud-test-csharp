using Mc2.CrudTest.Application.Dtos;
using MediatR;

namespace Mc2.CrudTest.Application.Queries;

public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>
{
}
