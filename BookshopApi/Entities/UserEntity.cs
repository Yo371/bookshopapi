using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopApi.Entities;

[Table("Users")]
public class UserEntity
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [ForeignKey("role_id")]
    public AuthEntity Auth { get; set; }

    public string Email { get; set; }
    
    public double Phone { get; set; }
    
    public string Address { get; set; }
    
    [Required]
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public byte[] Salt { get; set; }
}

