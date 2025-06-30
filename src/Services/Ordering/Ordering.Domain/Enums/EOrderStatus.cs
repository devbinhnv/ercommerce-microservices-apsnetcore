using System.ComponentModel;

namespace Ordering.Domain.Enums;

public enum EOrderStatus
{
    [Description("Order is new")]
    New = 1,
    [Description("Order is pending, not any activities for a period time")]
    Pending = 2,
    [Description("Order is paid")]
    Paid = 3,
    [Description("Order is on the shipping")]
    Shipping =4,
    [Description("Order is on the Fulfilled")]
    Fulfilled = 5
}
