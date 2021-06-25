using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;
using dotnet5_webapp.Repos;
using dotnet5_webapp.Utils;
using Newtonsoft.Json.Linq;

namespace dotnet5_webapp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _UserRepo;
        static string _address = Constants.RunescapeApiBaseUrl;
        static string _playerCountAddress = Constants.RunescapeApiPlayerCount;
        static int _totalSkills = Constants.TotalSkills + 1;

        public UserService(IUserRepo userRepo)
        {
            _UserRepo = userRepo;
        }

        private async Task<String> OfficialApiCall(String url)
        {
            // API call
            var client = new HttpClient();
            var response = new HttpResponseMessage();
            response = await client.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                return null;
            }
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }                
        
        private async Task<(ICollection<Skill>, ICollection<Minigame>)> GetCurrentStats(String username)
        {
            List<Skill> skills = new List<Skill>();
            List<Minigame> minigames = new List<Minigame>();
            var apiData = await OfficialApiCall(_address + username);
            if (apiData == null)
            {
                return (null, null);
            }
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
                
                // round instances of -1 to 0 instead
                skill.Xp = skill.Xp < 0 ? 0 : skill.Xp;
                skill.Rank = skill.Rank < 0 ? 0 : skill.Rank;
                
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
                
                // round instances of -1 to 0 instead
                minigame.Score = minigame.Score < 0 ? 0 : minigame.Score;
                minigame.Rank = minigame.Rank < 0 ? 0 : minigame.Rank;

                minigames.Add(minigame);
            }

            return (skills, minigames);
        }

        public async Task<int> CurrentPlayerCount()
        {
            int result;
            var outputString = await OfficialApiCall(_playerCountAddress);
            outputString = outputString.Split('(', ')')[1];
            bool isParsable = Int32.TryParse(outputString, out result);
            if (!isParsable)
            {
                result = 0;
            }
            return result;
        }
        
        public async Task<List<Activity>> GetAllActivities()
        {
            return await _UserRepo.GetAllActivities();
        }
        
        public async Task<ResponseWrapper<PlayerDetailsServiceResponse>> GetPlayerDetails(string username)
        {
            var data = new PlayerDetailsServiceResponse();
            var apiData = "";
            try
            {
            apiData = await OfficialApiCall(Constants.RunescapeApiPlayerDetailsUrlPre + username + Constants.RunescapeApiPlayerDetailsUrlPost);
            var apiItems = apiData.Remove(0, 33).Split("\"");
            
            data.Username = apiItems[7];
            data.ClanName = apiItems[11];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResponseWrapper<PlayerDetailsServiceResponse>
                {
                    Success = false,
                    Status = $"User {username} not found in the Official API details table",
                    Data = data
                };
            }
            
            return new ResponseWrapper<PlayerDetailsServiceResponse>
            {
                Success = true,
                Data = data
            };
        }         
        public async Task<ResponseWrapper<PlayerQuestsServiceResponse>> GetPlayerQuests(String username)
        {
            var data = new PlayerQuestsServiceResponse();

            try
            {
                var apiData = await OfficialApiCall(Constants.RunescapeApiQuestsUrl + username);
                JObject joResponse = JObject.Parse(apiData);
                data.Username = username;
                var totalQuests = 0; 
                var completedQuests = 0;
                var questPoints = 0;
                var totalQuestPoints = 0;
                
                JArray quests = (JArray)joResponse ["quests"];
                foreach (var quest in quests)
                {
                    var qp = quest.Value<int>("questPoints");
                    totalQuests++;
                    totalQuestPoints += qp;
                    if (quest.Value<string>("status") == Constants.QuestStatusCompleted)
                    {
                        completedQuests++;
                        questPoints += qp;
                    }

                }

                if (totalQuests <= 0 || totalQuestPoints <= 0)
                {
                    return new ResponseWrapper<PlayerQuestsServiceResponse>
                    {
                        Success = false,
                        Status = $"User {username} has RuneMetrics profile set to private",
                        Data = data
                    };
                }
                
                data.TotalQuests = totalQuests;
                data.CompletedQuests = completedQuests;
                data.QuestPoints = questPoints;
                data.TotalQuestPoints = totalQuestPoints;
                data.QuestCape = totalQuests == completedQuests && questPoints > 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResponseWrapper<PlayerQuestsServiceResponse>
                {
                    Success = false,
                    Status = $"Cannot find quest data for user: {username}",
                    Data = data
                };
            }
            
            return new ResponseWrapper<PlayerQuestsServiceResponse>
            {
                Success = true,
                Data = data
            };
        }        
        public async Task<ResponseWrapper<PlayerMetricsServiceResponse>> GetPlayerMetrics(String username)
        {
            var data = new PlayerMetricsServiceResponse();
            var user = await _UserRepo.GetShallowUserByUsername(username);

            try
            {
            var apiData = await OfficialApiCall(Constants.RunescapeApiPlayerMetricsUrlPre + username + Constants.RunescapeApiPlayerMetricsUrlPost);
            JObject joResponse = JObject.Parse(apiData);
            var questsComplete = (int)joResponse["questscomplete"];
            var name = (string)joResponse["name"];
            data.Username = name;
            data.QuestsComplete = questsComplete;

            data.Activities = new List<Activity>();
            JArray activities = (JArray)joResponse ["activities"];
            foreach (var activity in activities)
            {
                var dateRecorded = DateTime.Parse(activity.Value<String>("date"));
                var newActivity = new Activity()
                {
                    User = user,
                    UserId = user.Id,
                    DateRecorded = dateRecorded,
                    Title = activity.Value<String>("text"),
                    Details = activity.Value<String>("details"),
                };
                data.Activities.Add(newActivity);
            }

            var addedActivities = await _UserRepo.CreateActivities(data.Activities);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ResponseWrapper<PlayerMetricsServiceResponse>
                {
                    Success = false,
                    Status = $"User {username} has RuneMetrics profile set to private",
                    Data = data
                };
            }
            
            return new ResponseWrapper<PlayerMetricsServiceResponse>
            {
                Success = true,
                Data = data
            };
        }

        public async Task CreateStatRecord(User user)
        {
            List<Skill> skills = new List<Skill>();
            List<Minigame> minigames = new List<Minigame>();
            var apiData = await OfficialApiCall(_address + user.Username);
            if (apiData == null)
            {
                return;
            }
            string[] lines = apiData.Split('\n');

            // adding a StatRecord object
            StatRecord newStatRecord = new StatRecord()
            {
                DateCreated = DateTime.Now,
                UserId = user.Id,
                User = user
            };

            // looping through the skills and adding them
            for (var i = 0; i < _totalSkills; i++)
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
                skill.Xp = skill.Xp < 0 ? 0 : skill.Xp;
                skill.Rank = skill.Rank < 0 ? 0 : skill.Rank;
                skills.Add(skill);
            }

            // looping through the minigames and adding them
            for (var i = _totalSkills; i < lines.Length - 1; i++)
            {
                String[] stat = lines[i].Split(',');
                Minigame minigame = new Minigame()
                {
                    MinigameId = i,
                    Score = Int32.Parse(stat[1]),
                    Rank = Int32.Parse(stat[0]),
                    StatRecordId = newStatRecord.Id
                };
                minigame.Score = minigame.Score < 0 ? 0 : minigame.Score;
                minigame.Rank = minigame.Rank < 0 ? 0 : minigame.Rank;
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
            var users = await _UserRepo.GetAllTrackableUsers();
            
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
                StatRecords = new List<StatRecord>(),
                IsTracking = false
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

        public async Task<ResponseWrapper<Boolean>> TrackUser(String username)
        {
            var response = new ResponseWrapper<Boolean>
            {
                Success = true,
                Status = ""
            };
            
            var user = await _UserRepo.GetUserByUsername(username);
            if (user == null)
            {
                response.Success = false;
                response.Status = "User does not exist in the database.";
                return response;
            }
            
            user = await _UserRepo.StartTrackingUser(user);
            response.Data = user.IsTracking;
            return response;
        }

        public async Task<ResponseWrapper<CurrentGainForUserServiceResponse>> CurrentGainForUser(String username)
        {
            var response = new ResponseWrapper<CurrentGainForUserServiceResponse>
            {
                Success = true,
                Status = "",
                Data = new CurrentGainForUserServiceResponse()
            };
            
            var user = await _UserRepo.GetUserByUsername(username);
            if (user == null)
            {
                // create use if does not exist
                user = await CreateNewUser(username);
                if (user == null)
                {
                    //if still null, means nobody exists with that username and we just return null for now
                    response.Success = false;
                    response.Status = "User does not exist on the official hiscores.";
                    return response;
                }
                response.Status = "User created.";
            }
            
            response.Data.Username = username;
            response.Data.IsTracking = user.IsTracking;
            
            var skillGains = new List<SkillGain>();
            var minigameGains = new List<MinigameGain>();
            var badges = new List<Constants.BadgeType>();
            
            // get current stats to show and compare to records
            var (currentSkills, currentMinigames) = await GetCurrentStats(username);

            if (currentMinigames == null || currentSkills == null)
            {
                response.Status = "User not found.";
                response.Success = false;
                return response;
            }

            // get records for yesterday, sunday, month start, and year start
            var dayRecord = await _UserRepo.GetYesterdayRecord(user.Id);
            var weekRecord = await _UserRepo.GetWeekRecord(user.Id);
            var monthRecord = await _UserRepo.GetMonthRecord(user.Id);
            var yearRecord = await _UserRepo.GetYearRecord(user.Id);

            // calculate gainz
            for (var i = 0; i < _totalSkills; i++)
            {
                var skillGain = new SkillGain();
                
                // one skill at a time
                var currentSkill = currentSkills.ElementAt(i);
                var daySkill = dayRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                var weekSkill = weekRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                var monthSkill = monthRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                var yearSkill = yearRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();

                if (daySkill.Xp < 0 || weekSkill.Xp < 0 || monthSkill.Xp < 0 || yearSkill.Xp < 0)
                {
                    daySkill.Xp = daySkill.Xp < 0 ? 0 : daySkill.Xp;
                    weekSkill.Xp = weekSkill.Xp < 0 ? 0 : weekSkill.Xp;
                    monthSkill.Xp = monthSkill.Xp < 0 ? 0 : monthSkill.Xp;
                    yearSkill.Xp = yearSkill.Xp < 0 ? 0 : yearSkill.Xp;
                }

                skillGain.SkillId = currentSkill.SkillId;
                skillGain.Xp = currentSkill.Xp;
                skillGain.Level = currentSkill.Level;
                skillGain.Rank = currentSkill.Rank;
                skillGain.DayGain = currentSkill.Xp - daySkill.Xp;
                skillGain.WeekGain = currentSkill.Xp - weekSkill.Xp;
                skillGain.MonthGain = currentSkill.Xp - monthSkill.Xp;
                skillGain.YearGain = currentSkill.Xp - yearSkill.Xp;
                skillGains.Add(skillGain);
            }
            for (var i = 0; i < currentMinigames.Count - 1; i++)
            {
                var minigameGain = new MinigameGain();
                
                var currentMinigame = currentMinigames.ElementAt(i);
                var dayMinigame = dayRecord.Minigames.Where(s => s.MinigameId == currentMinigame.MinigameId).FirstOrDefault();
                var weekMinigame = weekRecord.Minigames.Where(s => s.MinigameId == currentMinigame.MinigameId).FirstOrDefault();
                var monthMinigame = monthRecord.Minigames.Where(s => s.MinigameId == currentMinigame.MinigameId).FirstOrDefault();
                var yearMinigame = yearRecord.Minigames.Where(s => s.MinigameId == currentMinigame.MinigameId).FirstOrDefault();
                
                if (dayMinigame.Score < 0 || weekMinigame.Score < 0 || monthMinigame.Score < 0 || yearMinigame.Score < 0)
                {
                    dayMinigame.Score = dayMinigame.Score < 0 ? 0 : dayMinigame.Score;
                    weekMinigame.Score = weekMinigame.Score < 0 ? 0 : weekMinigame.Score;
                    monthMinigame.Score = monthMinigame.Score < 0 ? 0 : monthMinigame.Score;
                    yearMinigame.Score = yearMinigame.Score < 0 ? 0 : yearMinigame.Score;
                }
                
                minigameGain.MinigameId = currentMinigame.MinigameId;
                minigameGain.Score = currentMinigame.Score;
                minigameGain.Rank = currentMinigame.Rank;
                minigameGain.DayGain = currentMinigame.Score - dayMinigame.Score;
                minigameGain.WeekGain = currentMinigame.Score - weekMinigame.Score;
                minigameGain.MonthGain = currentMinigame.Score - monthMinigame.Score;
                minigameGain.YearGain = currentMinigame.Score - yearMinigame.Score;
                minigameGains.Add(minigameGain);
            }
            
            // add appropriate badges

            var lowestLevel = skillGains.Min(sg => sg.Level);
            // this is without invention
            var lowestNonEliteLevel = skillGains.Where(sg => sg.SkillId != 27).Min(sg => sg.Level);

            if (skillGains.FirstOrDefault().Xp == Constants.MaxXp)
            {
                badges.Add(Constants.BadgeType.MaxXp);
            }            
            else if (skillGains.Min(sg => sg.Xp) >= 104273167 && skillGains[27].Xp > 80618654)
            {
                badges.Add(Constants.BadgeType.All120);
            }
            else if (skillGains.FirstOrDefault().Level == Constants.MaxTotal)
            {
                badges.Add(Constants.BadgeType.MaxTotal);
            }
            else if (lowestLevel >= 99)
            {
                badges.Add(Constants.BadgeType.Maxed);
            }            
            else
            {
                var baseLevel = (lowestNonEliteLevel /= 10) * 10;
                if (baseLevel > 0)
                {
                    badges.Add((Constants.BadgeType) baseLevel);
                }
            }

            response.Data.SkillGains = skillGains;
            response.Data.MinigameGains = minigameGains;
            response.Data.Badges = badges;
            response.Data.DisplayName = user.DisplayName;
            
            return response;
        }
    }
}