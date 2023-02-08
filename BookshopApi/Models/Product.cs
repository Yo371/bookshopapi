namespace BookshopApi.Models;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Author { get; set; }
    
    public double Price { get; set; }
    
    public string ImagePath { get; set; }

    protected bool Equals(Product other)
    {
        return Id == other.Id && Name == other.Name && Description == other.Description && Author == other.Author && Price.Equals(other.Price) && ImagePath == other.ImagePath;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Product)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Description, Author, Price, ImagePath);
    }
}