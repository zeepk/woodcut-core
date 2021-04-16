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

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await Context.User.Where(u => u.Username == username).Include(u => u.StatRecords).ThenInclude(r => r.Skills.OrderBy(s => s.SkillId)).FirstOrDefaultAsync();
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
        public async Task<User> SaveChanges(User user)
        {
            await Context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await Context.User
                .Include(u => u.StatRecords)
                .ToListAsync();
        }
    
    }
}
