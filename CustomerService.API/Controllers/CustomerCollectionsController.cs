using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.API.Binders;
using CustomerService.DAL.Contracts.Repositories;
using CustomerService.DAL.Entities;
using CustomerService.DTO.Creation;
using CustomerService.DTO.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerCollectionsController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCollectionsController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerCollection(
            [FromBody] IEnumerable<CustomerCreationDTO> customerCollection)
        {
            if(customerCollection==null)
            {
                return BadRequest();
            }

            var customerEntities = Mapper.Map<IEnumerable<Customer>>(customerCollection);

            foreach(var customer in customerEntities)
            {
                await _customerRepository.AddCustomerAsync(customer);
            }

            if(!await _customerRepository.SaveAsync())
            {
                throw new Exception("Creating a customer collection failed on save.");
            }

            var customersToReturn = Mapper.Map<IEnumerable<CustomerDTO>>(customerEntities);
            var stringId = string.Join(",",
                customersToReturn.Select(x => x.Id));

            return CreatedAtRoute("GetCustomerCollection",
                new { ids = stringId },
                customersToReturn);
        }

        [HttpGet("({ids})",Name ="GetCustomerCollection")]
        public async Task<IActionResult> GetCustomerCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if(ids==null)
            {
                return BadRequest();
            }

            var customerEntities = await _customerRepository.GetCustomersAsync(ids);

            if(ids.Count()!=customerEntities.Count())
            {
                return NotFound();
            }

            var customersToReturn = Mapper.Map<IEnumerable<CustomerDTO>>(customerEntities);
            return Ok(customersToReturn);
        }
    }
}