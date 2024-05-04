using System.ComponentModel.Design;
using Microsoft.EntityFrameworkCore;
using prueba_redarbor.Context;
using prueba_redarbor.Models;
using prueba_redarbor.Service;

namespace Testing_redarbor;

[TestClass]
public class UnitTest_UserService
{
    private AppDbContext _context;
    private DbContextOptions<AppDbContext> _options;

    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(_options);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.User.RemoveRange(_context.User);
        _context.SaveChanges();
        _context.Dispose();
    }


    private User CreateUser(string username, int companyId)
    {
        return new User
        {
            CompanyId = companyId,
            CreatedOn = DateTime.Now,
            DeletedOn = DateTime.Now,
            Email = "string@correo.com",
            Fax = "Numero de fax",
            Name = "Nombre_1",
            LastLogin = DateTime.Now,
            Password = "Cpass_sec",
            PortalId = 0,
            RoleId = 1,
            StatusId = 1,
            Telephone = "312312",
            UpdatedOn = DateTime.Now,
            Username = username
        };
    }

    [TestMethod]
    public void HelperStatus_Returns_Zero_When_NoItemExists()
    {
        var service = new UserService(_context);
        var actualItemId = service.HelperStatus();

        Assert.AreEqual(0, actualItemId, "HelperStatus: should return 0 when no item exists");
    }


    [TestMethod]
    public void HelperStatus_Returns_ItemId_When_ItemExists()
    {
        var expectedItemId = 1;
        var testData = CreateUser("Username1", 1);

        _context.User.Add(testData);
        _context.SaveChanges();

        var service = new UserService(_context);
        var actualItemId = service.HelperStatus();
        Assert.AreEqual(expectedItemId, actualItemId, "HelperStatus: should return 1 when existing one item");
    }


    [TestMethod]
    public void GetAllUser_Returns_All_Users()
    {
        var users = new List<User>
        {
        CreateUser("Username1", 1),
        CreateUser("Username2", 2)
        };

        _context.User.AddRange(users);
        _context.SaveChanges();

        var service = new UserService(_context);
        var result = service.GetAllUser();

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count , "GetAllUser: Should return 2 rows in user");
    }



    [TestMethod]
    public void GetAllUser_Returns_Zero_Users()
    {
        var service = new UserService(_context);
        var result = service.GetAllUser();

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count, "GetAllUser: Should return 0 rows in all users");
    }

    [TestMethod]
    public void GetUser_Returns_User_When_UserExists()
    {
        var companyId = 1;
        var testData = CreateUser("Username1", companyId);


        _context.User.Add(testData);
        _context.SaveChanges();

        var service = new UserService(_context);
        var result = service.GetUser(companyId);

        Assert.IsNotNull(result);
        Assert.AreEqual(companyId, result.CompanyId, "GetAllUser: It should return the user id when it exists");
    }

    [TestMethod]
    public void GetUser_Returns_EmptyUser_When_UserDoesNotExist()
    {

        var service = new UserService(_context);
        var result = service.GetUser(1); 

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Id, "It should return null the user when not exists");
    }

    [TestMethod]
    public void AddUser_Returns_NewUser_When_UserIsValid()
    {
        var userService = new UserService(_context);
        var user = CreateUser("Username1", 1);

        var result = userService.AddUser(user);

        Assert.IsNotNull(result);
        Assert.AreNotEqual(0, result.Id, "It should validate that an ID has been assigned to the new user"); 
    }


    [TestMethod]
    public void AddUser_Throws_Exception_When_UserAlreadyExists()
    {
        var userService = new UserService(_context);
        var existingUser = CreateUser("Username1", 1);

        _context.User.Add(existingUser);
        _context.SaveChanges();

        Assert.ThrowsException<Exception>(() => userService.AddUser(existingUser), "It should throw an exception when trying to add an existing user");
    }


    [TestMethod]
    public void UpdateUser_Updates_User_When_UserExists()
    {
        var companyId = 1;
        var username = "Update_1";
        var existingUser = CreateUser("ExistingUser", companyId);
        _context.User.Add(existingUser);
        _context.SaveChanges();

        var newUser = new User
        {
            Id = existingUser.Id,
            CompanyId = companyId,
            CreatedOn = DateTime.Now,
            DeletedOn = DateTime.Now,
            Email = "string@correo.com",
            Fax = "Numero de fax",
            Name = "Nombre_1",
            LastLogin = DateTime.Now,
            Password = "Cpass_sec",
            PortalId = 0,
            RoleId = 1,
            StatusId = 1,
            Telephone = "312312",
            UpdatedOn = DateTime.Now,
            Username = username
        };

        var service = new UserService(_context);

        service.UpdateUser(newUser);
        var result = service.GetUser(companyId);

        Assert.IsNotNull(result);
        Assert.AreEqual(companyId, result.CompanyId, "It should return the user id when it exists");
        Assert.AreEqual(existingUser.Username, username, "It should update the user's Username");
    }

    [TestMethod]
    public void DeleteUser_Removes_User_When_UserExists()
    {
        var companyId = 1;
        var existingUser = CreateUser("ExistingUser", companyId);
        _context.User.Add(existingUser);
        _context.SaveChanges();
        var service = new UserService(_context);

        service.DeleteUser(existingUser.CompanyId);
        var result = service.GetUser(existingUser.CompanyId);

        Assert.AreEqual(0, result.Id, "It should return null the user when not exists");
    }


}
