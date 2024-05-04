using System;
using prueba_redarbor.Models;

namespace prueba_redarbor.Service
{
    public interface IUserService
    {
        public int HelperStatus();
        public List<User> GetAllUser();

    }
}

