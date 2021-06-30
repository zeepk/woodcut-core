using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task<Player> GetPlayerByUsername(string username)
        {
            var user = await Context.User.Where(u => u.Username == username)
                .Include(u => u.StatRecords)
                .ThenInclude(r => r.Skills.OrderBy(s => s.SkillId))
                .Include(u => u.StatRecords)
                .ThenInclude(r => r.Minigames.OrderBy(s => s.MinigameId))
                .FirstOrDefaultAsync();
            return user;
        }         
        public async Task<Player> GetPlayerById(int id)
        {
            var user = await Context.User.Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            return user;
        }        
        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            var user = await Context.ApplicationUser
                .Where(au => au.UserName == username)
                .Include(au => au.FollowingPlayers)
                .FirstOrDefaultAsync();
            return user;
        }
        public async Task<Player> GetShallowUserByUsername(string username)
        {
            var user = await Context.User.Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            return user;
        }
        
        public async Task<Player> AddStatRecordToUser(StatRecord statRecord)
        {
            var user = await Context.User.Include(u => u.StatRecords).FirstOrDefaultAsync(u => u.Username == statRecord.Player.Username);
            user.StatRecords.Add(statRecord);
            await Context.SaveChangesAsync();
            return user;
        }               
        public async Task<Player> CreateUser(Player player)
        {
            await Context.User.AddAsync(player);
            await Context.SaveChangesAsync();
            return player;
        }            
        public async Task<List<Activity>> CreateActivities(List<Activity> activities)
        {
            foreach (var activity in activities)
            {
            var doesActivityExist = await Context.Activity
                .AnyAsync(a => a.Title == activity.Title && a.DateRecorded == activity.DateRecorded && a.Player == activity.Player);
            if (!doesActivityExist)
            {
                await Context.Activity.AddAsync(activity);
            }
            }
            await Context.SaveChangesAsync();
            return activities;
        }        
        public async Task<Player> SaveChanges(Player player)
        {
            await Context.SaveChangesAsync();
            return player;
        }
        public async Task<Player> StartTrackingUser(Player player)
        {
            player.IsTracking = true;
            await Context.SaveChangesAsync();
            return player;
        }        
        public async Task<Player> FollowPlayer(Follow follow, ApplicationUser user)
        {
            await Context.Follow.AddAsync(follow);
            await Context.SaveChangesAsync();
            user.FollowingPlayers.Add(follow);
            return follow.Player;
        }        
        public async Task<Player> UnfollowPlayer(Player player, ApplicationUser user)
        {
            var follow = await Context.Follow.Where(f => f.Player == player && f.User == user).FirstOrDefaultAsync();
            // user.FollowingPlayers = user.FollowingPlayers.Where(f => f.Id != follow.Id).ToList();
            // user.FollowingPlayers.Remove()
            Context.Follow.Remove(follow);
            await Context.SaveChangesAsync();
            return player;
        }        
        public async Task<ICollection<String>> GetFollowedPlayerNames(ApplicationUser user)
        {
            // var users = user.FollowingIds.Select(id => GetPlayerById(id));
            // var players = Context.User
            //     .Where(p => user.FollowingIds.Contains(p.Id));
            // return await players.Select(p => p.Username).ToListAsync();

            return await Context.Follow
                .Include(f => f.Player)
                .Select(f => f.Player.Username)
                .ToListAsync();
        }
        public async Task<List<Player>> GetAllUsers()
        {
            return await Context.User
                .Include(u => u.StatRecords)
                .ToListAsync();
        }        
        public async Task<List<Player>> GetAllTrackableUsers()
        {
            return await Context.User
                .Where(u => u.IsTracking)
                .Include(u => u.StatRecords)
                .ToListAsync();
        }
        public async Task<List<Activity>> GetAllActivities()
        {
            return await Context.Activity.OrderByDescending(a => a.DateRecorded)
                .ToListAsync();
        }
        public async Task<StatRecord> GetYesterdayRecord(int userId)
        {
            var record = await Context.StatRecord.Where(r => r.UserId == userId).OrderByDescending(r => r.DateCreated).FirstOrDefaultAsync();
            return record;
        }        
        public async Task<StatRecord> GetWeekRecord(int userId)
        {
            var sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 1);
            var record = await Context.StatRecord.Where(r => r.UserId == userId && r.DateCreated >= sunday)
                .OrderBy(r => r.DateCreated)
                .Include(r => r.Skills.OrderBy(s => s.SkillId))
                .Include(r => r.Minigames.OrderBy(s => s.MinigameId))
                .FirstOrDefaultAsync();
            return record;
        }        
        public async Task<StatRecord> GetMonthRecord(int userId)
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var record = await Context.StatRecord.Where(r => r.UserId == userId && r.DateCreated > firstDayOfMonth)
                .OrderBy(r => r.DateCreated)
                .Include(r => r.Skills.OrderBy(s => s.SkillId))
                .Include(r => r.Minigames.OrderBy(s => s.MinigameId))
                .FirstOrDefaultAsync();
            return record;
        }        
        public async Task<StatRecord> GetYearRecord(int userId)
        {
            var today = DateTime.Today;
            var firstDayOfYear = new DateTime(today.Year, 1, 1);
            var record = await Context.StatRecord.Where(r => r.UserId == userId && r.DateCreated > firstDayOfYear)
                .OrderBy(r => r.DateCreated)
                .Include(r => r.Skills.OrderBy(s => s.SkillId))
                .Include(r => r.Minigames.OrderBy(s => s.MinigameId))
                .FirstOrDefaultAsync();
            return record;
        }
    }
}
