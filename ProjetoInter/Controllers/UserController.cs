using Microsoft.AspNetCore.Mvc;
using ProjetoInter.Models;
using ProjetoInter.Services.User;

namespace ProjetoInter.Controllers
{
    public class UserController(IUserInterface userInterface) : Controller
    {
        private readonly IUserInterface _userInterface = userInterface;

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
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _userInterface.CreateUser(user);
                    return RedirectToAction("Index");
                }

                return View("Create");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não coneguimos realizar seu login, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Create");
            }
        
        }
    }
}
