using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Models;
using ProjetoInter.Services.MarketCar;

namespace ProjetoInter.Controllers
{
    public class MarketCarController : Controller
    {
        private readonly IMarketCarInterface _marketCarInterface;

        public MarketCarController(IMarketCarInterface marketCarInterface)
        {
            _marketCarInterface = marketCarInterface;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public async Task<IActionResult> AddToMarketCar(Guid id)
        {
            try
            {
                await _marketCarInterface.AddToMarketCar(id);
                return RedirectToAction("Add");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos editar seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Remove()
        {
            return View();
        }

        public async Task<IActionResult> RemoveFromMarketCar(Guid id)
        {
            try
            {
                await _marketCarInterface.RemoveFromMarketCar(id);
                return RedirectToAction("Remove");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos editar seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
