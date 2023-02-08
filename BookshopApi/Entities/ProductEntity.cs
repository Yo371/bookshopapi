using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopApi.Entities;

[Table("Products")]
public class ProductEntity
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Author { get; set; }
    
    public double Price { get; set; }
    
    [Column("image_path")]
    public string ImagePath { get; set; }
}