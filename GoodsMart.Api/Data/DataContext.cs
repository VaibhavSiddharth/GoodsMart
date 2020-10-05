using GoodsMart.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsMart.Api.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<Customer> Customers { get; set;}
        
    }
}