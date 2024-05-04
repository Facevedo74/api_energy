using System;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using prueba_redarbor.Models;

namespace prueba_redarbor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }


        /// <summary>
        /// Generate Token.
        /// </summary>
        /// <remarks>
        /// Use "user" : "userTest" and "password" : "12345"
        /// </remarks>
        /// <returns>This method returns the bearer token</returns>
        [HttpPost]
        public IActionResult Post([FromBody] LoginModel loginRequest)
        {
            if (loginRequest.Username == "userTest" && loginRequest.Password == "12345")
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Issuer"],
                  null,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return Ok("Bearer " + token);
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpGet]
        [Route("Test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok(new { message = "Hello, you are authorized!" });
        }

    }

}

