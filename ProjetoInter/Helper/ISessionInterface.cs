using ProjetoInter.Models;

namespace ProjetoInter.Helper
{
    public interface ISessionInterface
    {
        void CreateUserSession(UserModel user);
        void RemoveUserSession();
        UserModel GetUserSession();

    }
}
