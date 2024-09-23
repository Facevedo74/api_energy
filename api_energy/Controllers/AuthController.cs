using System;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using api_energy.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using api_energy.Service;
using DocumentFormat.OpenXml.InkML;
using api_energy.Context;
using Microsoft.EntityFrameworkCore;


namespace api_energy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private IAuthService authService;
        private readonly ILogger<AuthController> _logger;
        //
        private readonly AppDbContext _context;
        // 

        public AuthController(IConfiguration config, IAuthService authService, ILogger<AuthController> logger, AppDbContext context)
        {
            this.authService = authService;
            this._logger = logger;
            _config = config;
            //
            _context = context; // Inyección del contexto

            //

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
            // Validar usuario y contraseña
            var result = authService.ValidateUserPass(loginRequest.Username, loginRequest.Password);
            if (result)
            {
                var userRole = authService.GetUserRole(loginRequest.Username);

                // Verificamos si el username y el rol están presentes
                if (loginRequest.Username != null && userRole.Username != null)
                {

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, loginRequest.Username),  // Username
                       // new Claim(ClaimTypes.NameIdentifier, userRole.Username),        // Rol del usuario
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _config["Jwt:Issuer"],
                        audience: _config["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(120),
                        signingCredentials: credentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new { token = tokenString,role = new { id = 1, name = "Admin" },username = loginRequest.Username });
                }
                else
                {
                    return BadRequest("El username o el rol es nulo.");
                }
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

