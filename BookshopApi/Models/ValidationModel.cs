using BookshopApi.Entities;

namespace BookshopApi.Models;

public class ValidationModel
{
    public int Id { get; set; }
    
    public bool IsCredentialsMatched { get; set; }

    public Role Role { get; set; }
}