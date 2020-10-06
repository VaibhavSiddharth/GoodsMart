using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GoodsMart.Api.Data;
using GoodsMart.Api.Dtos;
using GoodsMart.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GoodsMart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthorization _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthorization repo, IConfiguration config)
        {
            _config = config;

            _repo = repo;


        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(CustomerForRegisterDto customerForRegisterDto)
        {
            //Validate user

            customerForRegisterDto.Customername = customerForRegisterDto.Customername.ToLower();
            //Check whether user with username exists or not
            if (await _repo.CustomerExists(customerForRegisterDto.Customername))
                return BadRequest("Username already exists");

            //Create the user without hashed password to create
            var customerToCreate = new Customer
            {
                Customername = customerForRegisterDto.Customername
            };

            var customer = await _repo.Register(customerToCreate, customerForRegisterDto.Password);
            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(CustomerForLoginDto customerForLoginDto)
        {
            
            var customerFromRepo = await _repo.Login(customerForLoginDto.Customername.ToLower(),
             customerForLoginDto.Password);
            if (customerFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customerFromRepo.CustomerId.ToString()),
                new Claim(ClaimTypes.Name, customerFromRepo.Customername)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok( new { token = tokenHandler.WriteToken(token)});

        }

    }
}