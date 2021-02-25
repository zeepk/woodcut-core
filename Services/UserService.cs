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
        static int _totalSkills = Constants.TotalSkills + 1;
        private async Task<String> OfficialApiCall(String username)
        {
            // API call
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(_address + username);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        private async Task<StatRecord> CreateStatRecord(User user)
        {
            List<Skill> skills = new List<Skill>();
            List<Minigame> minigames = new List<Minigame>();
            var apiData = await OfficialApiCall(user.Username);
            string[] lines = apiData.Split('\n');

            // adding a StatRecord object
            StatRecord newStatRecord = new StatRecord()
            {
                DateCreated = DateTime.Now,
                UserId = user.Id,
                User = user
            };

            // looping through the skills and adding them
            for (int i = 0; i < _totalSkills; i++)
            {
                String[] stat = lines[i].Split(',');
                Skill skill = new Skill()
                {
                    SkillId = i,
                    Xp = Int64.Parse(stat[2]),
                    Level = Int32.Parse(stat[1]),
                    Rank = Int32.Parse(stat[0]),
                    StatRecordId = newStatRecord.Id
                };
                skills.Add(skill);
            }

            // looping through the minigames and adding them
            for (int i = _totalSkills; i < lines.Length - 1; i++)
            {
                String[] stat = lines[i].Split(',');
                Minigame minigame = new Minigame()
                {
                    MinigameId = i,
                    Score = Int32.Parse(stat[1]),
                    Rank = Int32.Parse(stat[0]),
                    StatRecordId = newStatRecord.Id
                };
                minigames.Add(minigame);
            }

            newStatRecord.Skills = skills;
            newStatRecord.Minigames = minigames;

            return newStatRecord;
        }
        public async Task<StatRecord> AddNewStatRecord(User user)
        {
            var newStatRecord = await CreateStatRecord(user);
            user.StatRecords.Add(newStatRecord);
            //List<StatRecord> newList = new List<StatRecord> { newStatRecord };
            //user.StatRecords = newList;
            return newStatRecord;
        }
        public List<String> AddNewStatRecordForAllUsers(List<User> users)
        {
            users.ForEach(async u =>
            {
                await CreateStatRecord(u);
            });

            return users.Select(o => o.Username).ToList();
        }
        public async Task<User> CreateNewUser(String username)
        {
            // if the user is not found in the Official API, this will error out
            // var apiData = await OfficialApiCall(username);

            // creating new user
            User newUser = new User()
            {
                DateCreated = DateTime.Now,
                Username = username,
                DisplayName = username.Replace('+', ' ')
            };

            try
            {
                // creating an initial stat record
                var newStatRecord = await CreateStatRecord(newUser);
                List<StatRecord> newList = new List<StatRecord> { newStatRecord };
                newUser.StatRecords = newList;
            }
            catch
            {
                newUser = null;
            }


            return newUser;
        }
    }
}