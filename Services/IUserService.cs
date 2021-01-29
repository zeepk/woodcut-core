using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Services
{
    public interface IUserService
    {
        Task<StatRecord> AddNewStatRecord(User user);
        List<String> AddNewStatRecordForAllUsers(List<User> users);
        Task<User> CreateNewUser(String username);

    }
}