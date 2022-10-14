using System.Threading.Tasks;
using UserAPI.Src.Model;

namespace UserAPI.Src.Repositories
{
    public interface IUser
    {
        Task NovoUsuarioAsync(User user);
        Task<User> PegarUsuarioPeloEmailAsync(string email);
        Task AtualizarUsuarioAsync(User user);
        Task DeletarUsuarioAsync(int id);

    }
}
