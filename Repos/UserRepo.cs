﻿using System;
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

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await Context.User.Where(u => u.Username == username)
                .Include(u => u.StatRecords)
                .ThenInclude(r => r.Skills.OrderBy(s => s.SkillId))
                .Include(u => u.StatRecords)
                .ThenInclude(r => r.Minigames.OrderBy(s => s.MinigameId))
                .FirstOrDefaultAsync();
            return user;
        }
        public async Task<User> GetShallowUserByUsername(string username)
        {
            var user = await Context.User.Where(u => u.Username == username)
                .FirstOrDefaultAsync();
            return user;
        }
        
        public async Task<User> AddStatRecordToUser(StatRecord statRecord)
        {
            var user = await Context.User.Include(u => u.StatRecords).FirstOrDefaultAsync(u => u.Username == statRecord.User.Username);
            user.StatRecords.Add(statRecord);
            await Context.SaveChangesAsync();
            return user;
        }               
        public async Task<User> CreateUser(User user)
        {
            await Context.User.AddAsync(user);
            await Context.SaveChangesAsync();
            return user;
        }            
        public async Task<List<Activity>> CreateActivities(List<Activity> activities)
        {
            foreach (var activity in activities)
            {
            var doesActivityExist = await Context.Activity.AnyAsync(a => a.Title == activity.Title);
            if (!doesActivityExist)
            {
            await Context.Activity.AddAsync(activity);
            }
            }
            await Context.SaveChangesAsync();
            return activities;
        }        
        public async Task<User> SaveChanges(User user)
        {
            await Context.SaveChangesAsync();
            return user;
        }
        public async Task<User> StartTrackingUser(User user)
        {
            user.IsTracking = true;
            await Context.SaveChangesAsync();
            return user;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await Context.User
                .Include(u => u.StatRecords)
                .ToListAsync();
        }        
        public async Task<List<User>> GetAllTrackableUsers()
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
