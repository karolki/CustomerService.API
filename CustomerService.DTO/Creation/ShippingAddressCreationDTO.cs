using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.DTO.Creation
{
    public class ShippingAddressCreationDTO
    {
        public string StreetAddress { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}
