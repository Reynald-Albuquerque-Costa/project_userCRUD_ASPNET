using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UserAPI.Src.Contexts;
using UserAPI.Src.Model;

namespace UserAPI.Src.Repositories.Implements
{
    public class UserRepository : IUser
    {
        #region Attributes

        private readonly UserContext _context;

        #endregion


        #region Constructors
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        #endregion


        #region Methods
        public async Task NovoUsuarioAsync(User user)
        {
            await _context.Users.AddAsync(
                new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password

                });
            await _context.SaveChangesAsync();
        }
        public async Task<User> PegarUsuarioPeloEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task AtualizarUsuarioAsync(User user)
        {
            var Existingtheme = await PegarUsuarioPeloEmailAsync(user.Email);
            Existingtheme.Email = user.Email;
            _context.Users.Update(Existingtheme);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarUsuarioAsync(int id)
        {
            var aux = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            _context.Users.Remove(aux);
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
