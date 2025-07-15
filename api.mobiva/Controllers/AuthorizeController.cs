using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace api.mobiva.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthorizeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Get([FromBody] AuthorizeUserInfo user)
        {
            Token token = new Token();
            var result = false;

            switch (user.Username)
            {
                case "ama":
                    {
                        if (user.Password == "ama")
                        {
                            result = true;
                        }
                    }
                    break;
            }
            if (result)
            {
                token = CreateToken(_configuration);
                
                return Ok(token);
            }
            else
            {
                token.Message = "Username or password is incorrect.";
                return BadRequest(token);
            }
        }

        private Token CreateToken(IConfiguration configuration)
        {
            Token token = new Token();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.Now.AddHours(2);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: configuration["Token:Issuer"], audience: configuration["Token:Audience"],
                expires: token.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: credentials
            );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
            byte[] numbers = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(numbers);
            token.RefreshToken = Convert.ToBase64String(numbers);
            token.Message = "İşlem Başarılı";
            return token;
        }
    }

    public class AuthorizeUserInfo
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string Message { get; set; }
    }

}
