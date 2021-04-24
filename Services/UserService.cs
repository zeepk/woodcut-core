using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Migrations;
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

        private async Task<(ICollection<Skill>, ICollection<Minigame>)> GetCurrentStats(String username)
        {
            List<Skill> skills = new List<Skill>();
            List<Minigame> minigames = new List<Minigame>();
            var apiData = await OfficialApiCall(username);
            string[] lines = apiData.Split('\n');

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
                };
                minigames.Add(minigame);
            }

            return (skills, minigames);
        }


        public async Task CreateStatRecord(User user)
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
            // return newStatRecord;
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
                await CreateStatRecord(newUser);
            }
            catch
            {
                Console.WriteLine($"Error adding initial stat record to new user with username {username}");
                return null;
            }
            
            var user = await _UserRepo.CreateUser(newUser);
            return user;
        }        
        public async Task<CurrentGainForUserServiceResponse> CurrentGainForUser(String username)
        {
            var user = await _UserRepo.GetUserByUsername(username);
            if (user == null)
            {
                // handle nonexistent user
                return null;
            }

            var response = new CurrentGainForUserServiceResponse();
            response.Username = username;
            
            var skillGains = new List<SkillGain>();
            var minigameGains = new List<MinigameGain>();
            // get current stats to show
            var (currentSkills, currentMinigames) = await GetCurrentStats(username);
            // get most recent record for day record
            var dayRecord = await _UserRepo.GetYesterdayRecord(user.Id);
            // most recent sunday record
            // most recent 1st of month
            // most recent 1st of jan
                
            // calculate gainz
            for (var i = 0; i < _totalSkills; i++)
            {
                var skillGain = new SkillGain();
                var currentSkill = currentSkills.ElementAt(i);
                var daySkill = dayRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                skillGain.SkillId = currentSkill.SkillId;
                skillGain.Xp = currentSkill.Xp;
                skillGain.Level = currentSkill.Level;
                skillGain.Rank = currentSkill.Rank;
                skillGain.DayGain = currentSkill.Xp - daySkill.Xp;
                skillGain.WeekGain = 0;
                skillGain.MonthGain = 0;
                skillGain.YearGain = 0;
                skillGains.Add(skillGain);
            }
            for (int i = _totalSkills; i < currentMinigames.Count - 1; i++)
            {
                var minigameGain = new MinigameGain();
                var currentMinigame = currentMinigames.ElementAt(i);
                var dayMinigame = dayRecord.Minigames.Where(s => s.MinigameId == currentMinigame.MinigameId).FirstOrDefault();
                minigameGain.MinigameId = currentMinigame.MinigameId;
                minigameGain.Score = currentMinigame.Score;
                minigameGain.Rank = currentMinigame.Rank;
                minigameGain.DayGain = currentMinigame.Score - dayMinigame.Score;
                minigameGain.WeekGain = 0;
                minigameGain.MonthGain = 0;
                minigameGain.YearGain = 0;
                minigameGains.Add(minigameGain);
            }

            response.SkillGains = skillGains;
            response.MinigameGains = minigameGains;
            
            return response;
        }
    }
}