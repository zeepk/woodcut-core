using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Repos
{
    public interface IUserRepo
    {
        Task<Player> GetPlayerByUsername(string username);
        Task<Player> GetPlayerByUsernameLite(string username);
        Task<ApplicationUser> GetUserByUsername(string username);
        Task<Player> GetShallowUserByUsername(string username);
        Task<Player> UpdatePlayerIronStatus(Player player, AccountType accountType);
        Task<Player> AddStatRecordToUser(StatRecord statRecord);
        Task<Player> CreateUser(Player player);
        Task<List<Activity>> CreateActivities(List<Activity> activities);
        Task<Player> SaveChanges(Player player);
        Task<Player> StartTrackingUser(Player player);
        Task<Player> FollowPlayer(Follow follow, ApplicationUser user);
        Task<bool> UpdateRs3Rsn(string username, ApplicationUser user);
        Task<Player> UnfollowPlayer(Player player, ApplicationUser user);
        Task<ICollection<String>> GetFollowedPlayerNames(ApplicationUser user);
        Task<ICollection<String>> GetPlayerNames();
        Task<ICollection<Activity>> GetFollowedPlayerActivities(ApplicationUser user, int size);
        Task<List<Player>> GetAllUsers();
        Task<List<Player>> GetAllTrackableUsers();
        Task<List<Activity>> GetAllActivities();
        Task<List<Activity>> GetLimitedActivities(int size);
        Task<StatRecord> GetYesterdayRecord(int userId);
        Task<StatRecord> GetTwoDaysAgoRecord(int userId);
        Task<StatRecord> GetWeekRecord(int userId);
        Task<StatRecord> GetMonthRecord(int userId);
        Task<StatRecord> GetYearRecord(int userId);
        Task<(StatRecord, StatRecord)> GetDxpRecords(int userId, DateTime startDate, DateTime endDate);
        Task<Activity> GetActivityById(int activityId);
        Task<Activity> LikeActivity(Activity activity, ActivityLike like);
        Task<Activity> UnlikeActivity(Activity activity, ApplicationUser user);
        Task<Player> UpdatePlayerLastChecked(Player player, string recentStats);

    }
}
