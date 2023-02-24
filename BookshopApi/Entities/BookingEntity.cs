using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopApi.Entities;

[Table("Bookings")]
public class BookingEntity
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("product_id")]
    public ProductEntity Product { get; set; }
    
    [ForeignKey("user_id")]
    public UserEntity User { get; set; }
    
    [Column("delivery_address")]
    public string DeliveryAddress { get; set; }
    
    [DisplayFormat(DataFormatString = "{yyyy-mm-dd}")]
    [Column("delivery_date")]
    public DateTime DeliveryDate { get; set; }
    
    [Column("delivery_time")]
    public DateTime DeliveryTime { get; set; }
    
    [ForeignKey("status_id")]
    public BookingStatusEntity BookingStatus { get; set; }
    
    public int Quantity { get; set; }
}