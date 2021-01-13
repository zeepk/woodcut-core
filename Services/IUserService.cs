using System;
using System.Threading.Tasks;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Services
{
    public interface IUserService
    {
        Task<StatRecord> AddNewStatRecord(User user);
        Task<User> CreateNewUser(String username);

    }
}