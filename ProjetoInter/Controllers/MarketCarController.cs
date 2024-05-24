using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoInter.Models;
using ProjetoInter.Services.MarketCar;
using ProjetoInter.Services.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjetoInter.Controllers
{
    public class MarketCarController : Controller
    {
        private readonly IMarketCarInterface _marketCarInterface;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserInterface _userInterface;

        public MarketCarController(IMarketCarInterface marketCarInterface, IHttpContextAccessor httpContext, IUserInterface userInterface)
        {
            _marketCarInterface = marketCarInterface;
            _httpContext = httpContext;
            _userInterface = userInterface;
        }
        public async Task<IActionResult> Index()
        {

            var viewModel = new HomeIndexViewModel();
            string userSession = _httpContext.HttpContext.Session.GetString("sessionUserLogged");
            UserModel user =  JsonConvert.DeserializeObject<UserModel>(userSession);

            var products = await _marketCarInterface.GetProductsInMyMarketCar();
            UserModel userUpdated = _userInterface.GetUserMarketCarTotalValue(user, products);

            viewModel.User = userUpdated;

            viewModel.Products = products;


            return View(viewModel);
        }

        public async Task<IActionResult> AddToMarketCar(Guid id)
        {
            try
            {
                await _marketCarInterface.AddToMarketCar(id);
                TempData["MensagemSuccess"] = $"Produto adicionado ao carrinho com sucesso";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos editar seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> RemoveFromMarketCar(Guid id)
        {
            try
            {
                await _marketCarInterface.RemoveFromMarketCar(id);
                TempData["MensagemSuccess"] = $"Produto removido do carrinho com sucesso";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos editar seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> buyProducts(List<ProductModel> products)
        {

            Console.WriteLine("asdsadsda");
            Console.WriteLine(products);
            try
            {
                await _marketCarInterface.BuyProducts(products);
                TempData["MensagemSuccess"] = $"Compra realizada com sucesso";
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não foi possível realizar a compra, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index", "Home");
            }
        }



    }
}
