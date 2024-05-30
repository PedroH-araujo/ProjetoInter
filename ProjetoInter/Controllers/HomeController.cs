using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoInter.Models;
using ProjetoInter.Services.MarketCar;
using ProjetoInter.Services.Produto;
using ProjetoInter.Services.User;
using System.Diagnostics;

namespace ProjetoInter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductInterface _productInterface;
        private readonly IMarketCarInterface _marketCarInterface;
        private readonly IHttpContextAccessor _httpContext;

        public HomeController(ILogger<HomeController> logger, IProductInterface productInterface, IMarketCarInterface marketCarInterface, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _productInterface = productInterface;
            _marketCarInterface = marketCarInterface;
            _httpContext = httpContext;
        }

        public async Task<IActionResult> Index(ProductSearch? search)
        {
            var viewModel = new HomeIndexViewModel();
            string userSession = _httpContext.HttpContext.Session.GetString("sessionUserLogged");
            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            viewModel.User = user;

            if (string.IsNullOrEmpty(search.Name) && string.IsNullOrEmpty(search.Status.ToString()) && search.MinimumValue == 0 && search.MaximumValue == 0)
            {
                var products = await _productInterface.GetProducts();
                viewModel.Products = products;
                return View(viewModel);
            }
            else
            {
                var products = await _productInterface.GetFilteredProducts(search);
                viewModel.Products = products;
                viewModel.Search = search;
                return View(viewModel);
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
