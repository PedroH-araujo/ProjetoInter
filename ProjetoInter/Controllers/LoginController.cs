using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Helper;
using ProjetoInter.Models;
using ProjetoInter.Services.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjetoInter.Controllers
{
    public class LoginController(IUserInterface userInterface, ISessionInterface session) : Controller
    {
        private readonly IUserInterface _userInterface = userInterface;
        private readonly ISessionInterface _session = session;

        public IActionResult Index()
        {
            // redireciona usuario logado para a home

            if (_session.GetUserSession() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        public IActionResult LogOut()
        {
            _session.RemoveUserSession();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
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
                            _session.CreateUserSession(user);
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
