namespace GoodsMart.Api.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Customername { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}