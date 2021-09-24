using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using dotnet5_webapp.Data;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;
using dotnet5_webapp.Utils;
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
                .AsSplitQuery()
                .FirstOrDefaultAsync();
            return user;
        }    
        public async Task<Player> GetPlayerByUsernameLite(string username)
        {
            var user = await Context.User.Where(u => u.Username == username)
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
        public async Task<Player> UpdatePlayerLastChecked(Player player, string recentStats)
        {
            player.RecentStats = recentStats;
            player.LastChecked = DateTime.Now;
            await Context.SaveChangesAsync();
            return player;
        }               
        public async Task<Player> CreateUser(Player player)
        {
            await Context.User.AddAsync(player);
            await Context.SaveChangesAsync();
            return player;
        }   
        public async Task<Player> UpdatePlayerIronStatus(Player player, AccountType accountType)
        {
            player.IronmanStatus = accountType;
            await Context.SaveChangesAsync();
            return player;
        }    
        public async Task<List<Activity>> CreateActivities(List<Activity> activities)
        {
            var updatedActivities = new List<Activity>();
            foreach (var activity in activities)
            {
                var doesActivityExist = await Context.Activity
                .AnyAsync(a => a.Title == activity.Title && a.DateRecordedStamp == activity.DateRecordedStamp && a.Player == activity.Player);
                if (!doesActivityExist)
                {
                    await Context.Activity.AddAsync(activity);
                    updatedActivities.Add(activity);
                }
            }
            await Context.SaveChangesAsync();
            return updatedActivities;
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
        public async Task<bool> UpdateRs3Rsn(string username, ApplicationUser user)
        {
            user.Rs3Rsn = username;
            await Context.SaveChangesAsync();
            return true;
        }        
        public async Task<ICollection<String>> GetFollowedPlayerNames(ApplicationUser user)
        {
            return await Context.Follow
                .Where(f => f.User == user)
                .Include(f => f.Player)
                .Select(f => f.Player.Username)
                .ToListAsync();
        }            
        public async Task<ICollection<String>> GetPlayerNames()
        {
            return await Context.User
                .Select(p => p.Username)
                .ToListAsync();
        }        
        public async Task<ICollection<Activity>> GetFollowedPlayerActivities(ApplicationUser user, int size)
        {
            // does the same thing, just another option if DbContext issues arise
            
            // var names = await GetFollowedPlayerNames(user);
            // var activities = await Context.Activity
            //     .Include(a => a.Player)
            //     .Where(a => names.Contains(a.Player.DisplayName))
            //     .OrderByDescending(a => a.DateRecorded)
            //     .ToListAsync();
            
            var activities = await Context.Follow
                .Where(f => f.User == user)
                .Include(f => f.Player)
                .Join(Context.Activity.Include(a => a.Player).Include(a => a.Likes), f => f.Player.Id, a => a.Player.Id, 
                    (f, a) => a)
                .OrderByDescending(a => a.DateRecorded)
                .ToListAsync();
                
            return activities
                .Where(a => isImportantActivity(a.Title, a.Details))
                .Take(size)
                .ToList();

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
            return await Context.Activity.OrderByDescending(a => a.DateRecorded).ToListAsync();
        }        
        public async Task<List<Activity>> GetLimitedActivities(int size)
        {
            var activities = await Context.Activity
                .OrderByDescending(a => a.DateRecorded)
                .Include(a => a.Player)
                .Include(a => a.Likes)
                .ToListAsync();
            return activities
                .Where(a => isImportantActivity(a.Title, a.Details))
                .Take(size)
                .ToList();
        }
        public async Task<StatRecord> GetYesterdayRecord(int userId)
        {
            var record = await Context.StatRecord.Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DateCreated)
                .Include(r => r.Skills)
                .Include(r => r.Minigames)                
                .FirstOrDefaultAsync();
            return record;
        }            
        public async Task<StatRecord> GetTwoDaysAgoRecord(int userId)
        {
            var record = await Context.StatRecord
                .Where(r => r.UserId == userId)
                .Include(r => r.Skills)
                .Include(r => r.Minigames)
                .OrderByDescending(r => r.DateCreated)
                .Skip(1)
                .FirstOrDefaultAsync();
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
        public async Task<(StatRecord, StatRecord)> GetDxpRecords(int userId, DateTime startDate, DateTime endDate)
        {
            var startRecord = await Context.StatRecord.Where(r => r.UserId == userId && r.DateCreated >= startDate)
                .OrderBy(r => r.DateCreated)
                .Include(r => r.Skills.OrderBy(s => s.SkillId))
                .Include(r => r.Minigames.OrderBy(s => s.MinigameId))
                .FirstOrDefaultAsync();            
            var endRecord = await Context.StatRecord.Where(r => r.UserId == userId && r.DateCreated >= endDate)
                .OrderBy(r => r.DateCreated)
                .Include(r => r.Skills.OrderBy(s => s.SkillId))
                .Include(r => r.Minigames.OrderBy(s => s.MinigameId))
                .FirstOrDefaultAsync();
            return (startRecord, endRecord);
        }    
        
        public async Task<Activity> GetActivityById(int activityId){
            return await Context.Activity.Where(a => a.Id == activityId)
                .FirstOrDefaultAsync();
        }
        public async Task<Activity> LikeActivity(Activity activity, ActivityLike like){
            await Context.ActivityLike.AddAsync(like);
            var foundActivity = await Context.Activity
                .Where(a => a.Id == activity.Id)
                .Include(a => a.Likes)
                .FirstOrDefaultAsync();
            foundActivity.Likes.Add(like);
            await Context.SaveChangesAsync();
            return activity;
        }
        public async Task<Activity> UnlikeActivity(Activity activity, ApplicationUser user){
            var foundActivity = await Context.Activity
                .Where(a => a.Id == activity.Id)
                .Include(a => a.Likes)
                .ThenInclude(l => l.User)
                .FirstOrDefaultAsync();
            foundActivity.Likes = foundActivity.Likes.Where(l => l.User != user).ToList();
            await Context.SaveChangesAsync();
            return activity;
        }
        
        private bool isImportantActivity(string title, string details)
        {
            var milestones = new List<string>()
            {
                "10",
                "20",
                "30",
                "40",
                "50",
                "60",
                "70",
                "80",
                "90",
                "99",
            };
            if (title.Contains("all skills over") && !milestones.Contains(title.Substring(title.Length - 2)))
            {
                return false;
            }            
            if (details.Contains("am now level") && !milestones.Contains(details.Substring(details.Length - 3, 2)))
            {
                return false;
            }             
            if (details.Contains("000000 experience") && !details.Contains("0000000 experience"))
            {
                return false;
            }            
            if (details.Contains("tetracompass.") || 
                details.Contains("triskelion") || 
                details.Contains("Citadel") || 
                details.Contains("treasure trail.") || 
                details.Contains("mystery") || 
                title.Contains("I found something") || 
                details.Contains("defeated the") || 
                title.Contains("Quest complete") || 
                title.Contains("archaeological mystery") || 
                title.Contains("songs unlocked") || 
                title.Contains("I killed") || 
                title.Contains("Dungeon floor") || 
                title.Contains("ancient effigy") || 
                title.Contains("artefacts") || 
                title.Contains("max cape") || 
                title.Contains("Forcae") || 
                details.Contains("Fealty")
            )
            {
                return false;
            }

            return true;
        }
    }
}
