namespace Basket.API.Entities;

public class Cart
{
    public string UserName { get; set; }

    public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();

    public Cart()
    {
    }
    public Cart(string username)
    {
        UserName = username;
    }

    public decimal TotalPrice => Items.Sum(item => item.ItemPrice * item.Quantity);
}
