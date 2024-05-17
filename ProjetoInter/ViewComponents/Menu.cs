using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoInter.Models;
using ProjetoInter.Services.User;

namespace ProjetoInter.ViewComponents
{
    public class Menu : ViewComponent
    {
        private readonly IUserInterface _userInterface;
        public Menu(IUserInterface userInterface)
        {
            this._userInterface = userInterface;
        }
    
        // vai mostrar a Default.cshtml por padrão
        public async Task<IViewComponentResult> InvokeAsync()
        {

            string userSession = HttpContext.Session.GetString("sessionUserLogged");

            if (string.IsNullOrEmpty(userSession)) return null;

            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);
            UserModel updatedUser = _userInterface.GetUserMarketCarCount(user);
            return View(updatedUser);
        }
    }
}
