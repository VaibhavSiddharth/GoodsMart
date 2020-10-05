using System;
using System.Threading.Tasks;
using GoodsMart.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodsMart.Api.Data
{
    public class Authorization : IAuthorization
    {
        private readonly DataContext _context;
        public Authorization(DataContext context)
        {
            _context = context;

        }
        public  async Task<bool> CustomerExists(string username)
        {
           if( await _context.Customers.AnyAsync( x => x.Customername == username)) 
           return true;

           return false;
        }

        public async Task<Customer> Login(string username, string password)
        {
            //Find the customer from the data store
            var customer = await _context.Customers.FirstOrDefaultAsync( x => x.Customername == username );
            if(customer == null)
            return null;

            if(!VerifyPasswordHash(password, customer.PasswordHash, customer.PasswordSalt))
            return null;

            return customer;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i<computedPasswordHash.Length; i++)
                {
                    if(computedPasswordHash[i] != passwordHash[i])
                    return false;
                }

                return true;
            }
        }

        public async Task<Customer> Register(Customer customer, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }
    }
}