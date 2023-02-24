using BookshopApi.DataAccess;
using BookshopApi.Entities;
using Commons.Models;
using Moq;
using Moq.EntityFrameworkCore;
using Tests.Models;

namespace Tests.UnitTests;

public class BaseTest
{
    protected BookShopContext BookShopContext;

    protected User User = UserFactory.GetPredefinedUser();
    
    [OneTimeSetUp]
    public void Setup()
    {
        var bookShopContextMock = new Mock<BookShopContext>();
        bookShopContextMock.Setup(e => e.AuthModels).ReturnsDbSet(new List<AuthEntity>()
        {
            new AuthEntity()
            {
                Id = 1,
                Role = Role.Admin
            },
            new AuthEntity()
            {
                Id = 2,
                Role = Role.Manager
            },
            new AuthEntity()
            {
                Id = 3,
                Role = Role.Customer
            },
        });
        bookShopContextMock.Setup(e => e.Users).ReturnsDbSet(new List<UserEntity>()
        {
            new UserEntity()
            {
                Id = 1,
                Name = "John",
                Address = "Street 1",
                Email = "john@mail.com",
                Auth = new AuthEntity()
                {
                    Id = 1,
                    Role = Role.Admin
                },
                Login = "john",
                Password = "123",
                Phone = 43534532
            },
            new UserEntity()
            {
                Id = 2,
                Name = "John_Manager",
                Address = "Street 2",
                Email = "john@mail.com",
                Auth = new AuthEntity()
                {
                    Id = 2,
                    Role = Role.Manager
                },
                Login = "john_m",
                Password = "123",
                Phone = 43534532
            },
            new UserEntity()
            {
                Id = 3,
                Name = "John_Customer",
                Address = "Street 3",
                Email = "john@mail.com",
                Auth = new AuthEntity()
                {
                    Id = 3,
                    Role = Role.Customer
                },
                Login = "john_c",
                Password = "123",
                Phone = 43534532
            },
        });

        BookShopContext = bookShopContextMock.Object;
    }
}