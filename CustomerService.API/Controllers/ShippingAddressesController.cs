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
    [Route("api/[controller]/{customerId}/shippingaddresses")]
    [ApiController]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public ShippingAddressesController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetShippingAdressesForCustomer(Guid customerId)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var shippingAddressesForCustomerEntities = await _customerRepository.GetShippingAddressesForCustomerAsync(customerId);

            var shippingAddressesForCustomer = Mapper.Map<IEnumerable<ShippingAddressDTO>>(shippingAddressesForCustomerEntities);

            return Ok(shippingAddressesForCustomer);
        }

        [HttpGet("{id}", Name = "GetShippingAddressForCustomer")]
        public async Task<IActionResult> GetShippingAdressesForCustomer(Guid customerId, Guid id)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var shippingAddressForCustomerEntity = await _customerRepository.GetShippingAddressForCustomerAsync(customerId, id);
            if (shippingAddressForCustomerEntity == null)
            {
                return NotFound();
            }

            var shippingAddressForCustomer = Mapper.Map<ShippingAddressDTO>(shippingAddressForCustomerEntity);

            return Ok(shippingAddressForCustomer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShippingAddressForCustomer(Guid customerId, [FromBody]ShippingAddressCreationDTO shippingAddress)
        {
            if (shippingAddress == null)
            {
                return BadRequest();
            }
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var shippingAddressEntity = Mapper.Map<ShippingAddress>(shippingAddress);

            await _customerRepository.AddShippingAddresForCustomerAsync(customerId, shippingAddressEntity);

            if (!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Creating an address for customer {customerId} failed on save");
            }

            var shippingAddressToReturn = Mapper.Map<ShippingAddressDTO>(shippingAddressEntity);

            return CreatedAtRoute("GetShippingAddressForCustomer",
                new { customerId = customerId, id = shippingAddressToReturn.Id },
                shippingAddressToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingAddressForCustomer(Guid customerId,Guid id)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var shippingAddressForCustomerEntity = await _customerRepository.GetShippingAddressForCustomerAsync(customerId, id);
            if (shippingAddressForCustomerEntity == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteShippingAddresAsync(shippingAddressForCustomerEntity);

            if(!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Deleting an address for customer {customerId} failed on save");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShippingAddress(Guid customerId,Guid id, [FromBody] ShippingAddressUpdateDTO shippingAddress)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var shippingAddressEntity = await _customerRepository.GetShippingAddressForCustomerAsync(customerId,id);
            if (shippingAddressEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(shippingAddress, shippingAddressEntity);

            if (!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Updating shipping address {id} for customer {customerId} failed on save");
            }

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateShippingAddress(Guid customerId, Guid id, 
            [FromBody] JsonPatchDocument<ShippingAddressUpdateDTO> patchShippingAddress)
        {
            if (patchShippingAddress == null)
            {
                return BadRequest();
            }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var shippingAddressEntity = await _customerRepository.GetShippingAddressForCustomerAsync(customerId, id);
            if (shippingAddressEntity == null)
            {
                return NotFound();
            }

            var shippingAddress = Mapper.Map<ShippingAddressUpdateDTO>(shippingAddressEntity);

            patchShippingAddress.ApplyTo(shippingAddress);

            Mapper.Map(shippingAddressEntity, shippingAddress);

            if (!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Updating shipping address {id} for customer {customerId} failed on save");
            }

            return Ok();
        }
    }
}