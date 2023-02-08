using BookshopApi.Entities;

namespace BookshopApi.Models;

public class Booking
{
    public int Id { get; set; }
    
    public ProductEntity ProductEntity { get; set; }
    
    public UserEntity UserEntity { get; set; }
    
    public string DeliveryAddress { get; set; }
    
    public DateTime DeliveryDate { get; set; }
    
    public DateTime DeliveryTime { get; set; }
    
    public BookingStatus BookingStatus { get; set; }
    
    public int Quantity { get; set; }
}