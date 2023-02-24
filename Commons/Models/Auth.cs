namespace Commons.Models;

public class Auth
{
    public Role Role { get; set; }

    protected bool Equals(Auth other)
    {
        return Role == other.Role;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Auth)obj);
    }

    public override int GetHashCode()
    {
        return (int)Role;
    }
}