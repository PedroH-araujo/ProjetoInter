using Microsoft.AspNetCore.Mvc;

namespace ProjetoInter.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
