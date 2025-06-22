using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    public class ProductAPIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
