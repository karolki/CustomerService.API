using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.DAL.Entities
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(300)]
        public string Address { get; set; }

        public int PhoneNumber { get; set; }

        public ICollection<ShippingAddress> ShippingAdresses { get; set; }
            = new List<ShippingAddress>();
    }
}
