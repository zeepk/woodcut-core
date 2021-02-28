using System;
using System.Threading.Tasks;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Repos
{
    public interface IUserRepo
    {
        Task<User> GetUserByUsername(string username);
    }
}
