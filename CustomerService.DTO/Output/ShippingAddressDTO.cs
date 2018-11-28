using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.DTO.Output
{
    public class ShippingAddressDTO
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}
