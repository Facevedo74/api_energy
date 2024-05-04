using System;
using prueba_redarbor.Context;
using prueba_redarbor.Models;

namespace prueba_redarbor.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }


        public int HelperStatus()
        {
            try
            {
                var item = _context.User.FirstOrDefault();
                return item != null ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in HelperStatus", ex);
            }
        }

        public List<User> GetAllUser()
        {
            try
            {
                return _context.User.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllUser", ex);
            }
        }

    }
}

