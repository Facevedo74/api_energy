using System;
using api_energy.Models;

namespace api_energy.Service
{
    public interface IAuthService
    {
        public Boolean ValidateUserPass(String Username, String Password);
        public User GetUserRole(String Username);
    }
}

