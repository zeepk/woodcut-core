using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Services
{
    public interface IUserService
    {
        Task CreateStatRecord(User user);
        Task<List<String>> AddNewStatRecordForAllUsers();
        Task<User> CreateNewUser(String username);
        Task<UserSearchResponse> SearchForUser(String username);
        Task<ResponseWrapper<CurrentGainForUserServiceResponse>> CurrentGainForUser(String username);
        Task<ResponseWrapper<PlayerDetailsServiceResponse>> GetPlayerDetails(String username);
        Task<ResponseWrapper<PlayerMetricsServiceResponse>> GetPlayerMetrics(String username);
        Task<ResponseWrapper<PlayerQuestsServiceResponse>> GetPlayerQuests(String username);
        Task<ResponseWrapper<Boolean>> TrackUser(String username);
        Task<int> CurrentPlayerCount();
        Task<List<Activity>> GetAllActivities();
    }
}