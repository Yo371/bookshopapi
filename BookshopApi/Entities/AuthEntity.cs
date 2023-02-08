using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
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


[JsonConverter(typeof(StringEnumConverter))]
public enum Role
{
    [EnumMember(Value = "Admin")]
    Admin=1, 
    [EnumMember(Value = "Manager")]
    Manager, 
    [EnumMember(Value = "Customer")]
    Customer
}