using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.DAL.Entities
{
    public class ShippingAddress
    {
        [Key]
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string StreetAddress { get; set; }

        [Required]
        [MaxLength(30)]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [ForeignKey("AuthorId")]
        public Customer Customer { get; set; }
    }
}
