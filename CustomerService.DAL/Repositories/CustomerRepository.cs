using CustomerService.DAL.Contracts.Repositories;
using CustomerService.DAL.Entities;
using CustomerService.DAL.Entities.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.DAL.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private CustomersContext _context;

        public CustomerRepository(CustomersContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            await _context.Customers.AddAsync(customer);

            if (customer.ShippingAdresses.Any())
            {
                foreach (var shippingAddress in customer.ShippingAdresses)
                {
                    shippingAddress.Id = Guid.NewGuid();
                }
            }
        }

        public async Task AddShippingAddresForCustomerAsync(Guid customerId, ShippingAddress shippingAddress)
        {
            var customer = await GetCustomerAsync(customerId);
            if (customer != null)
            {
                if (customer.Id == Guid.Empty)
                {
                    customer.Id = Guid.NewGuid();
                }
                customer.ShippingAdresses.Add(shippingAddress);
            }
        }

        public bool CustomerExists(Guid customerId)
        {
            return _context.Customers.Any(a => a.Id == customerId);
        }

        public Task DeleteCustomerAsync(Customer customer)
        {
            return Task.Run(() => _context.Customers.Remove(customer));
        }

        public Task DeleteShippingAddresAsync(ShippingAddress shippingAddress)
        {
            return Task.Run(() => _context.ShippingAddresses.Remove(shippingAddress));
        }

        public Task<Customer> GetCustomerAsync(Guid customerId)
        {
            return _context.Customers.FindAsync(customerId);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync(IEnumerable<Guid> customerIds)
        {
            return await _context.Customers
                .Where(a => customerIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToListAsync();
        }

        public Task<ShippingAddress> GetShippingAddressForCustomerAsync(Guid customerId, Guid shippingAddressId)
        {
            return Task.Run(()=> _context.ShippingAddresses
             .Where(sa => sa.Id == shippingAddressId && sa.CustomerId == customerId).FirstOrDefault());
        }

        public async Task<IEnumerable<ShippingAddress>> GetShippingAddressesForCustomerAsync(Guid customerId)
        {
            return await _context.ShippingAddresses
                     .Where(sa => sa.CustomerId == customerId).ToListAsync();
        }
        public async Task<IEnumerable<ShippingAddress>> GetShippingAddresses(IEnumerable<Guid> ids)
        {
            return await _context.ShippingAddresses
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
        }
        public Task<bool> SaveAsync()
        {
            return Task.Run(()=> (_context.SaveChanges() >= 0));
        }

        public void UpdateCustomerAsync(Customer customer)
        {

        }

        public void UpdateShippingAddresForCustomerAsync(ShippingAddress shippingAddress)
        {

        }
    }
}
