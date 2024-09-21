using System;
using api_energy.Context;
using api_energy.Models;

namespace api_energy.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        public AuthService(AppDbContext context)
        {
            _context = context;
        }


        public bool ValidateUserPass(string Username, string Password)
        {
            try
            {
                var user = _context.User.FirstOrDefault(x => x.Username == Username);
                return user != null ? Password == user.password : false ;
                //return user != null ? BCrypt.Net.BCrypt.Verify(Password, user.password) : false ;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ValidateUserPass", ex);
            }
        }

        public User GetUserRole(string Username)
        {
            try
            {
                return _context.User.FirstOrDefault(x => x.Username == Username);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ValidateUserPass", ex);
            }
        }
    }
}

