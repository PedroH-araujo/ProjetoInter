using Newtonsoft.Json;
using ProjetoInter.Models;

namespace ProjetoInter.Services.Shared
{
    public class SharedMethods : ISharedMethodsInterface
    {
        private readonly IHttpContextAccessor _httpContext;
        public SharedMethods(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public UserModel GetUser()
        {
            string userSession = _httpContext.HttpContext.Session.GetString("sessionUserLogged");
            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            return user;
        }
    }
}
