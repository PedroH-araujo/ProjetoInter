using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Helper;
using ProjetoInter.Models;
using ProjetoInter.Services.Produto;
using ProjetoInter.Services.User;

namespace ProjetoInter.Controllers
{
    public class UserController(IUserInterface userInterface, ISessionInterface session, IProductInterface productInterface) : Controller
    {
        private readonly IUserInterface _userInterface = userInterface;
        private readonly ISessionInterface _session = session;
        private readonly IProductInterface _productInterface = productInterface;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Update()
        {
            var user = _userInterface.GetUserById();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel actualUser = _session.GetUserSession();
                    if(user.Role == UserRole.buyer) {
                       var myProducts = await _productInterface.GetMyProducts(true);
                       if(myProducts.Any(p => p.IsActive)) {
                        TempData["MensagemErro"] = $"Para deixar de ser vendedor, você não pode possuir itens à venda. Por favor remova seus itens na aba Meus produtos.";
                        return RedirectToAction("Update");
                       }
                       else {
                            string oldPassword = HttpContext.Request.Form["OldPassword"];
                            if (oldPassword != null)
                            {
                                bool isValidPassword = actualUser.ValidPassword(oldPassword);
                                if (isValidPassword != true || oldPassword == null)
                                {
                                    TempData["MensagemErro"] = $"Senha atual incorreta, tente novamente";
                                    return RedirectToAction("Update");
                                }
                                string newPassword = HttpContext.Request.Form["NewPassword"];
                                user.Password = newPassword;
                                await _userInterface.UpdateUser(user);
                                _session.UpdateUserSession(user);
                                return RedirectToAction("Index", "Home");
                            }
                       }
                    }
                }

                return View("Update");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar seus dados,por favor tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Update");
            }

        }

        [HttpPost]
        public IActionResult CreateUser(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _userInterface.CreateUser(user);
                    _session.CreateUserSession(user);
                    return RedirectToAction("Index", "Home");
                }

                return View("Create");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos cadastrar seus dados, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Create");
            }
        
        }
    }
}
