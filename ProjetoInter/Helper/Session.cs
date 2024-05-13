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
          //  Task<List<MarketCarModel>> userMarketCar = GetUserMarketCar(user);
          //  string userMarketCarString = JsonConvert.SerializeObject(userMarketCar);

            _httpContext.HttpContext.Session.SetString("sessionUserLogged", userString);
           // _httpContext.HttpContext.Session.SetString("sessionUserMarketCar", userMarketCarString);
        }

       /* private async Task<List<MarketCarModel>> GetUserMarketCar(UserModel user)
        {
            try
            {
                return await _dbContext.MarketCars.Where(marketCar => marketCar.UserId.ToString() == user.Id.ToString()).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }*/

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
    }
}
