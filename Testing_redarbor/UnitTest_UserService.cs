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

        Assert.AreEqual(0, actualItemId, "HelperStatus should return 0 when no item exists");
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
        Assert.AreEqual(expectedItemId, actualItemId, "HelperStatus should return 1 when existing one item");
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
        Assert.AreEqual(2, result.Count);
    }



    [TestMethod]
    public void GetAllUser_Returns_Zero_Users()
    {
        var service = new UserService(_context);
        var result = service.GetAllUser();

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }


}
