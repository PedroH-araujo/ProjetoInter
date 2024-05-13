using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Models;
using ProjetoInter.Services.MarketCar;
using ProjetoInter.Services.Produto;
using System.Diagnostics;

namespace ProjetoInter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductInterface _productInterface;
        private readonly IMarketCarInterface _marketCarInterface;

        public HomeController(ILogger<HomeController> logger, IProductInterface productInterface, IMarketCarInterface marketCarInterface)
        {
            _logger = logger;
            _productInterface = productInterface;
            _marketCarInterface = marketCarInterface;
        }

        public async Task<IActionResult> Index(string? search)
        {
            if (search == null)
            {
                var products = await _productInterface.GetProducts();
                return View(products);
            }
            else
            {
                var products = await _productInterface.GetFilteredProducts(search);
                return View(products);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
