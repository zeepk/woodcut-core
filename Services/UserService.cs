using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using dotnet5_webapp.Models;
using dotnet5_webapp.Utils;

namespace dotnet5_webapp.Services
{
    public class UserService : IUserService
    {
        static string _address = Constants.RunescapeApiBaseUrl;
        private string result;
        public async Task<StatRecord> AddNewStatRecord(User user)
        {
            // API call
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_address + user.Username);
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadAsStringAsync();
            string[] lines = result.Split('\n');
            string[] stat = lines[0].Split(',');
            // adding a StatRecord object
            Skill skill = new Skill()
            {
                SkillId = 0,
                Xp = Int64.Parse(stat[2]),
                Level = Int32.Parse(stat[1]),
                Rank = Int32.Parse(stat[0])
            };
            StatRecord newStatRecord = new StatRecord()
            {
                DateCreated = DateTime.Now,
                UserId = user.Id,
                Skills = new List<Skill> { skill }
            };
            // List<StatRecord> newList = new List<StatRecord> { newStatRecord };
            // user.StatRecords = newList;
            return newStatRecord;
        }
    }
}