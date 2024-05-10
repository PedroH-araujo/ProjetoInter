using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Models;
using ProjetoInter.Services.Produto;
using ProjetoInter.Services.User;

namespace ProjetoInter.Controllers
{
    public class MyProductController(IProductInterface productInterface) : Controller
    {
        private readonly IProductInterface _productInterface = productInterface;
        public async Task<IActionResult> Index()
        {
            var products = await _productInterface.GetProducts();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel product, IFormFile image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productInterface.CreateProduct(product, image);
                    return RedirectToAction("Index", "Home");
                }

                return View("Create");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos cadastrar seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Create");
            }
            
        }
    }
}
