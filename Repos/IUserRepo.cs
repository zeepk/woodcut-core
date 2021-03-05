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
        Task<User> AddStatRecordToUser(StatRecord statRecord);
        Task<List<User>> GetAllUsers();
    }
}
