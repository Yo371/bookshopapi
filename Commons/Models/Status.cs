using System.Runtime.Serialization;

namespace Commons.Models;

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