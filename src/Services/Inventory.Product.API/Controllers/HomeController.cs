using Microsoft.AspNetCore.Mvc;

namespace Inventory.Product.API.Controllers;

public class HomeController : ControllerBase
{
    public IActionResult Index() => Redirect("~/swagger");
}
