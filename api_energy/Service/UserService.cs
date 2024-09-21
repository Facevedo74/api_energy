using System;
using api_energy.Context;
using api_energy.Models;

namespace api_energy.Service
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

        public User GetUser(string username)
        {
            try
            {
                return _context.User.FirstOrDefault(x => x.Username == username);
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
                        //CompanyId = user.CompanyId,
                        //CreatedOn = user.CreatedOn,
                        //DeletedOn = user.DeletedOn,
                        //Email = user.Email,
                        //Fax = user.Fax,
                        //Name = user.Name,
                        //LastLogin = user.LastLogin,
                        //Password = user.Password,
                        //PortalId = user.PortalId,
                        //RoleId = user.RoleId,
                        active = user.active,
                        //Telephone = user.Telephone,
                        //UpdatedOn = user.UpdatedOn,
                        //Username = user.Username,
                    };
                    _context.User.Add(newUser);
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

        public void UpdateUser(User user)
        {
            //try
            //{
            //    var userDb = GetUser(user.CompanyId);
            //    if (userDb != null)
            //    {
            //        _context.Entry(userDb).CurrentValues.SetValues(user);
            //        _context.SaveChanges();
            //        return;
            //    }
            //    throw new Exception("User not found");
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Error in UpdateUser", ex);
            //}

        }

        public void DeleteUser(int id)
        {
            try
            {
                var userToDelete = GetUser(id.ToString());
                if (userToDelete != null)
                {
                    _context.User.Remove(userToDelete);
                    _context.SaveChanges();
                    return;
                }

                throw new Exception("User not found");
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateUser", ex);
            }

        }

        public Boolean ValidateExistingUser(User user)
        {
            return false;
        //    var userExist = _context.User.FirstOrDefault(x => x.CompanyId == user.CompanyId || x.Username == user.Username);
        //    return userExist == null ? true : false;
        }


    }
}

