using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;
using dotnet5_webapp.Repos;
using dotnet5_webapp.Utils;

namespace dotnet5_webapp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _UserRepo;
        static string _address = Constants.RunescapeApiBaseUrl;
        static int _totalSkills = Constants.TotalSkills + 1;

        public UserService(IUserRepo userRepo)
        {
            _UserRepo = userRepo;
        }

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
            user.StatRecords.Add(newStatRecord);
            // var updatedUser = await _UserRepo.AddStatRecordToUser(newStatRecord);
            return newStatRecord;
        }

        public async Task<UserSearchResponse> SearchForUser(String username)
        {
            var response = new UserSearchResponse();
            var user = await _UserRepo.GetUserByUsername(username);
            if (user == null)
            {
                user = await CreateNewUser(username);
                response.WasCreated = user != null;
            }
            else
            {
                response.WasCreated = false;
            }
            response.User = user;
            return response;
        }

        public async Task<StatRecord> AddNewStatRecord(User user)
        {
            var newStatRecord = await CreateStatRecord(user);
            return newStatRecord;
        }

        public async Task<List<String>> AddNewStatRecordForAllUsers()
        {
            var users = await _UserRepo.GetAllUsers();
            
            var tasks = users.ToList().Select(u => CreateStatRecord(u));
            await Task.WhenAll(tasks);
            
            var usernames = users.ToList().Select(u => u.Username);

            // don't really need to pass a user, but await doesn't work well with methods which return void
            var user = await _UserRepo.SaveChanges(users.FirstOrDefault());
            return usernames.ToList();
        }

        public async Task<User> CreateNewUser(String username)
        {
            // creating new user
            User newUser = new User()
            {
                DateCreated = DateTime.Now,
                Username = username,
                DisplayName = username.Replace('+', ' '),
                StatRecords = new List<StatRecord>()
            };
            
            // if the user is not found in the Official API, this will error out
            try
            {
                // creating an initial stat record
                var newStatRecord = await CreateStatRecord(newUser);
            }
            catch
            {
                Console.WriteLine($"Error adding initial stat record to new user with username {username}");
                return null;
            }
            
            var user = await _UserRepo.CreateUser(newUser);
            return user;
        }        
        public async Task<User> CurrentGainForUser(String username)
        {
            var user = await _UserRepo.GetUserByUsername(username);
            if (user == null)
            {
                // handle nonexistent user
                return null;
            }
            // get most recent record for day record
            // most recent sunday record
            // most recent 1st of month
            // most recent 1st of jan
            return user;
        }
    }
}