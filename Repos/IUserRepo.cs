using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Repos
{
    public interface IUserRepo
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetShallowUserByUsername(string username);
        Task<User> AddStatRecordToUser(StatRecord statRecord);
        Task<User> CreateUser(User user);
        Task<List<Activity>> CreateActivities(List<Activity> activities);
        Task<User> SaveChanges(User user);
        Task<List<User>> GetAllUsers();
        Task<List<Activity>> GetAllActivities();
        Task<StatRecord> GetYesterdayRecord(int userId);
        Task<StatRecord> GetWeekRecord(int userId);
        Task<StatRecord> GetMonthRecord(int userId);
        Task<StatRecord> GetYearRecord(int userId);
    }
}
