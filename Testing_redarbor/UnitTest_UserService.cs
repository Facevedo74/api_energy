using Microsoft.EntityFrameworkCore;
using prueba_redarbor.Context;
using prueba_redarbor.Models;
using prueba_redarbor.Service;

namespace Testing_redarbor;

[TestClass]
public class UnitTest_UserService
{
    private AppDbContext _context;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
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
        var testData = new User {
            CompanyId = 1,
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
            Username = "Username1"
        };

        _context.User.Add(testData);
        _context.SaveChanges();

        var service = new UserService(_context);
        var actualItemId = service.HelperStatus();

        Assert.AreEqual(expectedItemId, actualItemId, "HelperStatus should return 1 when existing one item");
    }


    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

}
