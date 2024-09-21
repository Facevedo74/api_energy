using System;
using api_energy.Models;

namespace api_energy.Service
{
    public interface IUserService
    {
        public int HelperStatus();
        public List<User> GetAllUser();
        public User GetUser(string username);
        public User AddUser(User user);
        public void UpdateUser(User user);
        public void DeleteUser(int id);

    }
}

