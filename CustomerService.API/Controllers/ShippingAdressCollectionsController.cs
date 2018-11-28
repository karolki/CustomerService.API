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
    [Route("api/[controller]/{customerId}/shippingaddresscollections")]
    [ApiController]
    public class ShippingAdressCollectionsController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public ShippingAdressCollectionsController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShippingAddressCollection(Guid customerId,
            [FromBody] IEnumerable<ShippingAddressCreationDTO> shippingAddressCollection)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            if (shippingAddressCollection == null)
            {
                return BadRequest();
            }

            var shippingAddressEntities = Mapper.Map<IEnumerable<ShippingAddress>>(shippingAddressCollection);

            foreach (var shippingAddress in shippingAddressEntities)
            {
                await _customerRepository.AddShippingAddresForCustomerAsync(customerId, shippingAddress);
            }

            if (!await _customerRepository.SaveAsync())
            {
                throw new Exception($"Creating a shipping address collection for customer {customerId} failed on save.");
            }

            var shippingAddressesToReturn = Mapper.Map<IEnumerable<ShippingAddressDTO>>(shippingAddressEntities);
            var stringId = string.Join(",",
                shippingAddressesToReturn.Select(x => x.Id));

            return CreatedAtRoute("GetShippingAddressCollection",
                new { ids = stringId },
                shippingAddressesToReturn);
        }

        [HttpGet("({ids})", Name = "GetShippingAddressCollection")]
        public async Task<IActionResult> GetShippingAddressCollection(Guid customerId,
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            if (ids == null)
            {
                return BadRequest();
            }

            var shippingAddressEntities = await _customerRepository.GetShippingAddresses(ids);

            if (ids.Count() != shippingAddressEntities.Count())
            {
                return NotFound();
            }

            var customersToReturn = Mapper.Map<IEnumerable<ShippingAddressDTO>>(shippingAddressEntities);

            return Ok(customersToReturn);
        }
    }
}