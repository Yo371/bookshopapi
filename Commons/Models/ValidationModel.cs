namespace Commons.Models;

public class ValidationModel
{
    public int Id { get; set; }
    
    public bool IsCredentialsMatched { get; set; }

    public Role Role { get; set; }
    
    public string Token { get; set; }
}