using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookshopApi.Models;

public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public Auth Auth { get; set; }

    public string Email { get; set; }
    
    public double Phone { get; set; }
    
    public string Address { get; set; }
    
    public string Login { get; set; }
    
    public string Password { get; set; }

    protected bool Equals(User other)
    {
        return Id == other.Id && Name == other.Name && Auth.Equals(other.Auth) && Email == other.Email && Phone.Equals(other.Phone) && Address == other.Address && Login == other.Login;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Auth, Email, Phone, Address, Login, Password);
    }
}