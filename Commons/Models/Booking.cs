namespace Commons.Models;

public class Booking
{
    public int Id { get; set; }
    
    public Product Product { get; set; }
    
    public User User { get; set; }
    
    public string DeliveryAddress { get; set; }
    
    public DateTime DeliveryDate { get; set; }
    
    public DateTime DeliveryTime { get; set; }
    
    public BookingStatus BookingStatus { get; set; }
    
    public int Quantity { get; set; }
}