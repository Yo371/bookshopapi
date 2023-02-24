using System.ComponentModel.DataAnnotations.Schema;
using Commons.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookshopApi.Entities;

[Table("Roles")]
public class AuthEntity
{
    public int Id { get; set; }
    
    [Column("name")]
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Role Role { get; set; }
}