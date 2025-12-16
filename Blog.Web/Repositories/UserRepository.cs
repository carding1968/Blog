using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Blog.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _authDbContext1;
        public UserRepository(AuthDbContext authDbContext)
        {
            _authDbContext1 = authDbContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            return await _authDbContext1.Users.Where(x => x.Email != "SUPERADMIN@CRUD.COM").ToListAsync();
        }
    }
}
