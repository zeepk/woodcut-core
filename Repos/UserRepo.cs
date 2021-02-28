using System;
using System.Linq;
using System.Threading.Tasks;
using dotnet5_webapp.Data;
using dotnet5_webapp.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet5_webapp.Repos
{
    public class UserRepo : IUserRepo
    {

        private readonly DataContext Context;
        public UserRepo(DataContext context) => Context = context;

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await Context.User.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }
    
    }
}
