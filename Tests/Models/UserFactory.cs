using BookshopApi.Entities;
using BookshopApi.Models;

namespace Tests.Models;

public static class UserFactory
{
    public static User GetPredefinedUser()
    {
        return new User()
        {
            Name = "TestUser",
            Address = "Street 3",
            Email = "TestUser@mail.com",
            Auth = new Auth()
            {
                Role = Role.Customer
            },
            Login = "TestUser",
            Password = "123",
            Phone = 43534532
        };
    }
}