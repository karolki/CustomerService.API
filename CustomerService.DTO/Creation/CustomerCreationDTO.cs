using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.DTO.Creation
{
    public class CustomerCreationDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public int PhoneNumber { get; set; }

        public ICollection<ShippingAddressCreationDTO> ShippingAdrresses { get; set; }
            = new List<ShippingAddressCreationDTO>();
    }
}
