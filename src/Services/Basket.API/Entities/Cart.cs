namespace Basket.API.Entities;

public class Cart
{
    public string UserName { get; set; }

    public IEnumerable<CardItem> Items { get; set; } = new List<CardItem>();

    public Cart()
    {
    }
    public Cart(string username)
    {
        UserName = username;
    }

    public decimal TotalPrice => Items.Sum(item => item.ItemPrice * item.Quantity);
}
