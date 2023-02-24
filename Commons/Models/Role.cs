using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Commons.Models;

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