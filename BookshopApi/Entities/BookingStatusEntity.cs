using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookshopApi.Entities;

[Table("Booking_status")]
public class BookingStatusEntity
{
    public int Id { get; set; }
    
    [Column("name")]
    //[JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("status")]
    public Status Status { get; set; }
}