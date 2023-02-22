using System;
using System.Threading.Tasks;
using Mc2.CrudTest.Application.Dtos;
using Mc2.CrudTest.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.Presentation.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id)
    {
        var customer = await _customerService.GetCustomerById(id);
        if (customer == null)
        {
            return NotFound();
        }

        return customer;
    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer([FromBody] CustomerDto customerDto)
    {
        var createdCustomer = _customerService.CreateCustomer(customerDto);

        return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCustomer(Guid id, [FromBody] CustomerDto customerDto)
    {
        if (id != customerDto.Id)
        {
            return BadRequest();
        }

        _customerService.UpdateCustomer(customerDto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(Guid id)
    {
        _customerService.DeleteCustomer(id);

        return NoContent();
    }
}