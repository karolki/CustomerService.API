using Microsoft.EntityFrameworkCore;

namespace CustomerService.DAL.Entities.Contexts
{
    public class CustomersContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-BBOGAFK;Database=CustomerServiceDB;Trusted_Connection=True;");
        }
    }
}
