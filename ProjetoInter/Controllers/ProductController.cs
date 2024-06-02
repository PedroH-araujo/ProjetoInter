using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Models;
using ProjetoInter.Services.MarketCar;
using ProjetoInter.Services.Produto;
using ProjetoInter.Services.User;

namespace ProjetoInter.Controllers
{
    public class ProductController(IProductInterface productInterface, IMarketCarInterface marketCarInterface) : Controller
    {
        private readonly IProductInterface _productInterface = productInterface;
        private readonly IMarketCarInterface _marketCarInterface = marketCarInterface;
        public async Task<IActionResult> Index(bool? activeProducts)
        {
            var products = await _productInterface.GetMyProducts(activeProducts ?? true);
            var viewModel = new ProductIndexViewModel
            {
                Products = products,
                ActiveProducts = activeProducts
            };

            return View(viewModel);

        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var product = await _productInterface.GetProductById(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductModel product, IFormFile? image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _productInterface.CreateProduct(product, image);
                    return RedirectToAction("Index");
                }

                return View("Create");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos cadastrar seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Create");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductModel updatedProduct, IFormFile? image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _productInterface.UpdateProduct(updatedProduct, image);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    return View(updatedProduct);
                }

            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos editar seu produto, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Create");
            }
        }

        public async Task<IActionResult> RemoveProduct(Guid id)
        {
            var product = await _productInterface.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productInterface.GetProductById(id);
            return View(product);
        }

        public async Task<IActionResult> MyShopping(Guid id)
        {
            var product = await _marketCarInterface.GetMyPurchasedProducts();
            return View(product);
        }


    }
}
