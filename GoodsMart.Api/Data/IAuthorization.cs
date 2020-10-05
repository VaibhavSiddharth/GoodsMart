using System.Threading.Tasks;
using GoodsMart.Api.Models;

namespace GoodsMart.Api.Data
{
    public interface IAuthorization
    {
        //Method for Register
         Task<Customer> Register(Customer customer, string password);
         
         //Method for Login
         Task<Customer> Login(string username, string password);

         //Method to check whether Customer with username exists or not
         Task<bool> CustomerExists(string username);

    }
}