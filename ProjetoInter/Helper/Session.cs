using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjetoInter.Data;
using ProjetoInter.Models;

namespace ProjetoInter.Helper
{
    public class Session : ISessionInterface
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppDbContext _dbContext;
        public Session(IHttpContextAccessor httpContext, AppDbContext dbContext)
        {
            this._httpContext = httpContext;
            this._dbContext = dbContext;
        }
        public async void CreateUserSession(UserModel user)
        {
            // Transformar o objeto usuario em string
            string userString = JsonConvert.SerializeObject(user);

            _httpContext.HttpContext.Session.SetString("sessionUserLogged", userString);
        }

        public UserModel? GetUserSession()
        {
            string userSession = _httpContext.HttpContext.Session.GetString("sessionUserLogged");

            if (string.IsNullOrEmpty(userSession)) return null;

            // tranforma a string de usuario em objeto
            return JsonConvert.DeserializeObject<UserModel>(userSession);
        }

        public void RemoveUserSession()
        {
            _httpContext.HttpContext.Session.Remove("sessionUserLogged");
        }

        public void UpdateUserSession(UserModel user)
        {
            RemoveUserSession();

            CreateUserSession(user);
        }
    }
}
