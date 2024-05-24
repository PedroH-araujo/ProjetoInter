using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Models;
using ProjetoInter.Services.MarketCar;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjetoInter.Controllers
{
    public class MarketCarController : Controller
    {
        private readonly IMarketCarInterface _marketCarInterface;

        public MarketCarController(IMarketCarInterface marketCarInterface)
        {
            _marketCarInterface = marketCarInterface;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _marketCarInterface.GetProductsInMyMarketCar();
            return View(products);
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

    }
}
