using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopApi.Entities;

[Table("Book_store")]
public class StoreItemEntity
{
    public int Id { get; set; }

    [ForeignKey("product_id")]
    public ProductEntity? Product { get; set; }
    
    public int Available { get; set; }
    
    public int Sold { get; set; }
    
    public int Booked { get; set; }
}
