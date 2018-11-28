using CustomerService.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.DAL.Contracts.Repositories
{
    public interface ICustomerRepository
    {
        Task <IEnumerable<Customer>> GetCustomersAsync();
        Task <Customer> GetCustomerAsync(Guid customerId);
        Task <IEnumerable<Customer>> GetCustomersAsync(IEnumerable<Guid> customerIds);
        Task AddCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Customer customer);
        void UpdateCustomerAsync(Customer customer);
        bool CustomerExists(Guid customerId);
        Task <IEnumerable<ShippingAddress>> GetShippingAddressesForCustomerAsync(Guid customerId);
        Task <IEnumerable<ShippingAddress>> GetShippingAddresses(IEnumerable<Guid> customerId);
        Task <ShippingAddress> GetShippingAddressForCustomerAsync(Guid customerId, Guid shippingAddressId);
        Task AddShippingAddresForCustomerAsync(Guid customerId, ShippingAddress shippingAddress);
        void UpdateShippingAddresForCustomerAsync(ShippingAddress shippingAddress);
        Task DeleteShippingAddresAsync(ShippingAddress shippingAddress);
        Task<bool> SaveAsync();
    }
}
