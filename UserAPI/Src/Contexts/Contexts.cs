using Microsoft.EntityFrameworkCore;
using UserAPI.Src.Model;

namespace UserAPI.Src.Contexts
{
    public class UserContext : DbContext
    {
        #region Atributos
        public DbSet<User> Users { get; set; }


        #endregion


        #region Constructors
        public UserContext(DbContextOptions<UserContext> opt) :base(opt)
        {

        }

        #endregion
    }
}
