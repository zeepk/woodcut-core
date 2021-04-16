using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Services
{
    public interface IUserService
    {
        Task<StatRecord> AddNewStatRecord(User user);
        Task<List<String>> AddNewStatRecordForAllUsers();
        Task<User> CreateNewUser(String username);
        Task<UserSearchResponse> SearchForUser(String username);
        Task<User> CurrentGainForUser(String username);
    }
}