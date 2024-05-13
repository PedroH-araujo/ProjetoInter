using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoInter.Models;

namespace ProjetoInter.ViewComponents
{
    public class Menu : ViewComponent
    {
        // vai mostrar a Default.cshtml
        public async Task<IViewComponentResult> InvokeAsync()
        {

            string userSession = HttpContext.Session.GetString("sessionUserLogged");

            if (string.IsNullOrEmpty(userSession)) return null;

            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            return View(user);
        }
    }
}
