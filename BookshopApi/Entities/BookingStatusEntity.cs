using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookshopApi.Entities;

[Table("Booking_status")]
public class BookingStatusEntity
{
    public int Id { get; set; }
    
    [Column("name")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Status Status { get; set; }
}

public enum Status
{
    [EnumMember(Value = "Submitted")]
    Submitted = 1,
    [EnumMember(Value = "Rejected")]
    Rejected, 
    [EnumMember(Value = "Approved")]
    Approved, 
    [EnumMember(Value = "Cancelled")]
    Cancelled, 
    [EnumMember(Value = "InDelivery")]
    InDelivery, 
    [EnumMember(Value = "Completed")]
    Completed
}