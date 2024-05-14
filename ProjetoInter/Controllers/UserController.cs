using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Helper;
using ProjetoInter.Models;
using ProjetoInter.Services.Produto;
using ProjetoInter.Services.User;

namespace ProjetoInter.Controllers
{
    public class UserController(IUserInterface userInterface, ISessionInterface session) : Controller
    {
        private readonly IUserInterface _userInterface = userInterface;
        private readonly ISessionInterface _session = session;

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
                    await _userInterface.UpdateUser(user);
                    _session.UpdateUserSession(user);
                    return RedirectToAction("Index", "Home");
                }

                return View("Update");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar seus dados,por favor tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Create");
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
