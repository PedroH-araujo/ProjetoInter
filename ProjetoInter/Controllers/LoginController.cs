using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Models;
using ProjetoInter.Services.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjetoInter.Controllers
{
    public class LoginController(IUserInterface userInterface) : Controller
    {
        private readonly IUserInterface _userInterface = userInterface;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UserModel user = _userInterface.SearchByUsername(username: loginModel.Login);

                    if (user != null) 
                    {
                        if (user.ValidPassword(loginModel.Password))
                        {

                            return RedirectToAction("Index", "Home");
                        }

                        TempData["MensagemErro"] = $"Senha inválida. Por favor tente novamente";

                        return View("Index");

                    }

                    TempData["MensagemErro"] = $"Usuário e/ou senha inválido(s). Por favor tente novamente";
                }

                return View("Index");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos realizar seu login, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
