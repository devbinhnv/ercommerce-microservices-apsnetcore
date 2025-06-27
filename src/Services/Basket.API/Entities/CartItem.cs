using System.ComponentModel.DataAnnotations;

namespace Basket.API.Entities;

public class CartItem
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The field {0} should greater than 1")]
    public int Quantity { get; set; }

    [Required]
    [Range(0.1, double.PositiveInfinity, ErrorMessage = "The field {0} should greater than 1")]
    public decimal ItemPrice { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string ItemNo { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string ItemName { get; set; }
}
