using System.ComponentModel.DataAnnotations;

namespace GoodsMart.Api.Dtos
{
    public class CustomerForRegisterDto
    {
        [Required]
        public string Customername { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage ="Password should be between 4 and 8 characters")]
        public string Password { get; set; }
    }
}