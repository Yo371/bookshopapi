namespace Commons.Models;

public class Store
{
    public int Id { get; set; }
    
    public Product Product { get; set; }
    
    public int Available { get; set; }
    
    public int Sold { get; set; }
    
    public int Booked { get; set; }
}