using System.ComponentModel.DataAnnotations.Schema;
using BookshopApi.Models;

namespace BookshopApi.Entities;

[Table("Bookings")]
public class BookingEntity
{
    public int Id { get; set; }
    
    [ForeignKey("product_id")]
    public ProductEntity ProductEntity { get; set; }
    
    [ForeignKey("user_id")]
    public UserEntity UserEntity { get; set; }
    
    [Column("delivery_address")]
    public string DeliveryAddress { get; set; }
    
    [Column("delivery_date")]
    public DateTime DeliveryDate { get; set; }
    
    [Column("delivery_time")]
    public DateTime DeliveryTime { get; set; }
    
    [ForeignKey("status_id")]
    public BookingStatusEntity BookingStatus { get; set; }
    
    public int Quantity { get; set; }
}