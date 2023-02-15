using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopApi.Entities;

[Table("Products")]
public class ProductEntity
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string Author { get; set; }
    
    public decimal Price { get; set; }
    
    [Column("image_path")]
    public string ImagePath { get; set; }
}