using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Services
{
    public interface IUserService
    {
        Task CreateStatRecord(Player player);
        Task<List<String>> AddNewStatRecordForAllUsers();
        Task<List<Activity>> AddNewActivitiesForAllUsers();
        Task<Player> CreateNewUser(String username);
        Task<UserSearchResponse> SearchForPlayer(String username);
        Task<ApplicationUser> SearchForUser(String username);
        Task<ResponseWrapper<CurrentGainForUserServiceResponse>> CurrentGainForUser(String username);
        Task<ResponseWrapper<PlayerDetailsServiceResponse>> GetPlayerDetails(String username);
        Task<ResponseWrapper<PlayerMetricsServiceResponse>> GetPlayerMetrics(String username);
        Task<ResponseWrapper<PlayerQuestsServiceResponse>> GetPlayerQuests(String username);
        Task<ResponseWrapper<Boolean>> TrackUser(String username);
        Task<ResponseWrapper<Boolean>> FollowPlayer(String username, ApplicationUser user);
        Task<ResponseWrapper<Boolean>> UnfollowPlayer(String username, ApplicationUser user);
        Task<ResponseWrapper<string>> UpdateRs3Rsn(String username, ApplicationUser user);
        Task<ResponseWrapper<ICollection<String>>> GetFollowedPlayerNames(ApplicationUser user);
        Task<int> CurrentPlayerCount();
        Task<List<Activity>> GetAllActivities(int size);
    }
}