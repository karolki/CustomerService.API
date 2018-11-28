using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.DAL.Contracts.Repositories;
using CustomerService.DAL.Entities;
using CustomerService.DTO.Creation;
using CustomerService.DTO.Output;
using CustomerService.DTO.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customersEntity = await _customerRepository.GetCustomersAsync();

            var customers = Mapper.Map<IEnumerable<CustomerDTO>>(customersEntity);

            return Ok(customers);
        }

        [HttpGet("{id}",Name ="GetCustomer")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var customerEntity = await _customerRepository.GetCustomerAsync(id);

            if (customerEntity == null)
            {
                return NotFound();
            }

            var customer = Mapper.Map<CustomerDTO>(customerEntity);
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerCreationDTO customer)
        {
            if(customer== null)
            {
                return BadRequest();
            }

            var customerEntity = Mapper.Map<Customer>(customer);

            await _customerRepository.AddCustomerAsync(customerEntity);

            if(!await _customerRepository.SaveAsync())
            {
                throw new Exception("Creating a customer failed on save");
            }

            var customerToReturn = Mapper.Map<CustomerDTO>(customerEntity);

            return CreatedAtRoute("GetCustomer",
                new { id = customerToReturn.Id },
                customerToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockCustomerCreation(Guid id)
        {
            if(_customerRepository.CustomerExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customerEntity = await _customerRepository.GetCustomerAsync(id);
            if (customerEntity == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteCustomerAsync(customerEntity);

            if(!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Deleting customer failed on save");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(Guid id,[FromBody] CustomerUpdateDTO customer)
        {
            var customerEntity = await _customerRepository.GetCustomerAsync(id);
            if (customerEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(customer, customerEntity);

            if (!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Updating customer failed on save");
            }

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateCustomer(Guid id, [FromBody] JsonPatchDocument<CustomerUpdateDTO> patchCustomer)
        {
            if(patchCustomer==null)
            {
                return BadRequest();
            }

            var customerEntity = await _customerRepository.GetCustomerAsync(id);
            if (customerEntity == null)
            {
                return NotFound();
            }

            var customer= Mapper.Map<CustomerUpdateDTO>(customerEntity);

            patchCustomer.ApplyTo(customer);

            Mapper.Map(customerEntity, customer);

            if (!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Updating customer failed on save");
            }

            return Ok();
        }
    }
}