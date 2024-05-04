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

        public User GetUser(int id)
        {
            try
            {
                var user = _context.User.FirstOrDefault(x => x.CompanyId == id);
                if (user != null) return user;
                return new User();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetUser", ex);
            }
        }

        public User AddUser(User user)
        {
            try
            {
                if (ValidateExistingUser(user))
                {
                    var newUser = new User
                    {
                        CompanyId = user.CompanyId,
                        CreatedOn = user.CreatedOn,
                        DeletedOn = user.DeletedOn,
                        Email = user.Email,
                        Fax = user.Fax,
                        Name = user.Name,
                        LastLogin = user.LastLogin,
                        Password = user.Password,
                        PortalId = user.PortalId,
                        RoleId = user.RoleId,
                        StatusId = user.StatusId,
                        Telephone = user.Telephone,
                        UpdatedOn = user.UpdatedOn,
                        Username = user.Username
                    };
                    _context.User.Add(user);
                    _context.SaveChanges();
                    return newUser;
                }
                throw new Exception("User exist");
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddUser", ex);
            }
        }

        public Boolean ValidateExistingUser(User user)
        {
            var userExist = _context.User.FirstOrDefault(x => x.CompanyId == user.CompanyId || x.Username == user.Username);
            return userExist == null ? true : false;
        }
    }
}

