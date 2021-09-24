using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dotnet5_webapp.Internal;
using dotnet5_webapp.Models;
using dotnet5_webapp.Repos;
using dotnet5_webapp.Utils;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;

namespace dotnet5_webapp.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration Configuration;
        private readonly IUserRepo _UserRepo;
        static string hiscoreUrl = Constants.RunescapeApiBaseUrl;
        static string imHiscoreUrl = Constants.RunescapeImApiBaseUrl;
        static string hcimHiscoreUrl = Constants.RunescapeHcimApiBaseUrl;
        static string playerCountUrl = Constants.RunescapeApiPlayerCount;
        static string clanMemberListUrl = Constants.RunescapeApiClanMemberListUrl;
        static int totalSkills = Constants.TotalSkills + 1;

        public UserService(IUserRepo userRepo, IConfiguration configuration)
        {
            _UserRepo = userRepo;
            Configuration = configuration;
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
        
        public async Task<ResponseWrapper<(string, string)>> CurrentVos()
        {
            var twitterApiKey = Configuration["TwitterApiKey"];
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", twitterApiKey);
            var reqResp = new HttpResponseMessage();
            reqResp = await client.GetAsync("https://api.twitter.com/2/users/3429940841/tweets?max_results=5");
            
            var response = new ResponseWrapper<(string, string)>()
            {
                    Success = false,
            };

            try
            {
                response.Status = "VoS request failed";
                reqResp.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                return null;
            }
            var result = await reqResp.Content.ReadAsStringAsync();
            var innerString = result.Split("The Voice of Seren is now active in the ")
                .Last()
                .Split(" districts")
                .First()
                .Split(" and ");
            response.Data = (innerString.First(), innerString.Last());
            response.Status = "Success";
            response.Success = true;
            
            return response;
        }         
        
        private async Task<(ICollection<Skill>, ICollection<Minigame>)> GetCurrentStats(Player user)
        {
            var apiData = String.Empty;
            if ((DateTime.Now - user.LastChecked).TotalSeconds > 60)
            {
                Console.WriteLine("Making official API call");
                apiData = await OfficialApiCall(hiscoreUrl + user.Username);
                Console.WriteLine("Done making official API call");
                user = await _UserRepo.UpdatePlayerLastChecked(user, apiData);
            }
            else
            {
                apiData = user.RecentStats;
            }
            
            List<Skill> skills = new List<Skill>();
            List<Minigame> minigames = new List<Minigame>();
            if (apiData == null)
            {
                return (null, null);
            }
            string[] lines = apiData.Split('\n');
            // looping through the skills and adding them
            for (int i = 0; i < totalSkills; i++)
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
            for (int i = totalSkills; i < lines.Length - 1; i++)
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
            var outputString = await OfficialApiCall(playerCountUrl);
            outputString = outputString.Split('(', ')')[1];
            bool isParsable = Int32.TryParse(outputString, out result);
            if (!isParsable)
            {
                result = 0;
            }
            return result;
        }
        
        public async Task<List<ActivityResponse>> GetAllActivities(int size)
        {
            var activityList = await _UserRepo.GetLimitedActivities(size);
            var activityTasks =  activityList.Select(async a => await FormatActivity(a, null));
            var activityResponses = await Task.WhenAll(activityTasks);
            return activityResponses.ToList();
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
        public async Task<ResponseWrapper<Player>> UpdateIronStatus(String username)
        {
            var response = new ResponseWrapper<Player>
            {
                Success = true,
                Data = null
            };
            
            var player = await _UserRepo.GetPlayerByUsernameLite(username);
            if (player == null)
            {
                response.Status = $"User {username} not found in the Official API details table";
                return response;
            }
            
            var accountStatus = await GetAccountStatus(username);
            player = await _UserRepo.UpdatePlayerIronStatus(player, accountStatus);
            
            response.Data = player;
            return response;
        }          
        public async Task<ResponseWrapper<AccountType>> GetIronStatus(String username)
        {
            var response = new ResponseWrapper<AccountType>
            {
                Success = true,
                Data = AccountType.Main
            };
            
            var player = await _UserRepo.GetPlayerByUsernameLite(username);
            if (player == null)
            {
                response.Status = $"User {username} not found.";
                return response;
            }

            response.Data = player.IronmanStatus;
            return response;
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

            var activityList = new List<Activity>();
            JArray activities = (JArray)joResponse ["activities"];
            foreach (var activity in activities)
            {
                var dateString = activity.Value<String>("date");
                var dateRecorded = DateTime.Parse(dateString);
                var newActivity = new Activity()
                {
                    Player = user,
                    UserId = user.Id,
                    DateRecorded = dateRecorded,
                    DateRecordedStamp = dateString,
                    Title = activity.Value<String>("text"),
                    Details = activity.Value<String>("details"),
                };
                activityList.Add(newActivity);
            }

            var addedActivities = await _UserRepo.CreateActivities(activityList);

            var activityTasks =  activityList.Select(async a => await FormatActivity(a, null));
            var activityResponses = await Task.WhenAll(activityTasks);
            data.Activities = activityResponses.ToList();
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

        public async Task<ICollection<Activity>> UpdateActivitiesForPlayer(Player player)
        {
            var activities = new List<Activity>();
            try
            {
                var activityApiData = await OfficialApiCall(Constants.RunescapeApiPlayerMetricsUrlPre + player.Username + Constants.RunescapeApiPlayerMetricsUrlPost);
                JObject joResponse = JObject.Parse(activityApiData);
            
                JArray jsonActivities = (JArray)joResponse ["activities"];
                foreach (var activity in jsonActivities)
                {
                    var dateString = activity.Value<String>("date");
                    var dateRecorded = DateTime.Parse(dateString);
                    var newActivity = new Activity()
                    {
                        Player = player,
                        UserId = player.Id,
                        DateRecorded = dateRecorded,
                        DateRecordedStamp = dateString,
                        Title = activity.Value<String>("text"),
                        Details = activity.Value<String>("details"),
                    };
                    activities.Add(newActivity);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return activities;
        }
        public async Task CreateStatRecord(Player player)
        {
            List<Skill> skills = new List<Skill>();
            List<Minigame> minigames = new List<Minigame>();
            var apiData = await OfficialApiCall(hiscoreUrl + player.Username);
            if (apiData == null)
            {
                return;
            }
            string[] lines = apiData.Split('\n');

            // adding a StatRecord object
            StatRecord newStatRecord = new StatRecord()
            {
                DateCreated = DateTime.Now,
                UserId = player.Id,
                Player = player
            };

            // looping through the skills and adding them
            for (var i = 0; i < totalSkills; i++)
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
            for (var i = totalSkills; i < lines.Length - 1; i++)
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
            player.StatRecords.Add(newStatRecord);
        }
        
        

        public async Task<UserSearchResponse> SearchForPlayer(String username)
        {
            var response = new UserSearchResponse();
            var user = await _UserRepo.GetPlayerByUsername(username);
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
        public async Task<ApplicationUser> SearchForUser(String username)
        {
            var user = await _UserRepo.GetUserByUsername(username);
            return user;
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
        public async Task<List<Activity>> AddNewActivitiesForAllUsers()
        {
            var users = await _UserRepo.GetAllTrackableUsers();
            
            var activityTasks = users.ToList().Select(u => UpdateActivitiesForPlayer(u));
            var activities = await Task.WhenAll(activityTasks);
            
            var updatedActivities = await _UserRepo.CreateActivities(activities.SelectMany( i => i ).ToList());           
            // don't really need to pass a user, but await doesn't work well with methods which return void
            return updatedActivities;
        }

        public async Task<Player> CreateNewUser(String username)
        {
            // creating new user
            Player newPlayer = new Player()
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
                await CreateStatRecord(newPlayer);
            }
            catch
            {
                Console.WriteLine($"Error adding initial stat record to new user with username {username}");
                return null;
            }

            var accountType = await GetAccountStatus(username);

            newPlayer.IronmanStatus = accountType;
            
            var user = await _UserRepo.CreateUser(newPlayer);
            return user;
        }

        public async Task<ResponseWrapper<Boolean>> TrackUser(String username)
        {
            var response = new ResponseWrapper<Boolean>
            {
                Success = true,
                Status = ""
            };
            
            var user = await _UserRepo.GetPlayerByUsernameLite(username);
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
            
            var user = await _UserRepo.GetPlayerByUsernameLite(username);
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
            var (currentSkills, currentMinigames) = await GetCurrentStats(user);

            if (currentMinigames == null || currentSkills == null)
            {
                response.Status = "User not found.";
                response.Success = false;
                return response;
            }
            
            // establish dxp start and end
            var dxpStartDate = DateTime.Parse(Constants.dxpStartDateString);
            var dxpEndDate = DateTime.Parse(Constants.dxpEndDateString);
            var isDxpOver = DateTime.Today > dxpEndDate;

            // get records for yesterday, sunday, month start, and year start
            var dayRecord = await _UserRepo.GetYesterdayRecord(user.Id);
            var yesterdayRecord = await _UserRepo.GetTwoDaysAgoRecord(user.Id);
            var weekRecord = await _UserRepo.GetWeekRecord(user.Id);
            var monthRecord = await _UserRepo.GetMonthRecord(user.Id);
            var yearRecord = await _UserRepo.GetYearRecord(user.Id);
            var (dxpStartRecord, dxpEndRecord) = await _UserRepo.GetDxpRecords(user.Id, dxpStartDate, dxpEndDate);

            var validTracking = user.IsTracking && dayRecord != null;

            // calculate gainz
            for (var i = 0; i < totalSkills; i++)
            {
                var skillGain = new SkillGain();
                
                // one skill at a time
                var currentSkill = currentSkills.ElementAt(i);
                
                skillGain.SkillId = currentSkill.SkillId;
                skillGain.Xp = currentSkill.Xp;
                skillGain.Level = currentSkill.Level;
                skillGain.Rank = currentSkill.Rank;

                if (validTracking)
                {
                    var daySkill = dayRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                    var yesterdaySkill = yesterdayRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                    if (weekRecord == null)
                    {
                        weekRecord = dayRecord;
                    }
                    var weekSkill = weekRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                    if (monthRecord == null)
                    {
                        monthRecord = dayRecord;
                    }                    
                    var monthSkill = monthRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                    var yearSkill = yearRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                    var dxpStartSkill = dxpStartRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();
                    var dxpEndSkill = dxpStartRecord.Skills.Where(s => s.SkillId == currentSkill.SkillId).FirstOrDefault();

                    if (daySkill.Xp < 0 || yesterdaySkill.Xp < 0 || weekSkill.Xp < 0 || monthSkill.Xp < 0 || yearSkill.Xp < 0 || dxpStartSkill.Xp < 0 || dxpEndSkill.Xp < 0)
                    {
                        daySkill.Xp = daySkill.Xp < 0 ? 0 : daySkill.Xp;
                        yesterdaySkill.Xp = yesterdaySkill.Xp < 0 ? 0 : yesterdaySkill.Xp;
                        weekSkill.Xp = weekSkill.Xp < 0 ? 0 : weekSkill.Xp;
                        monthSkill.Xp = monthSkill.Xp < 0 ? 0 : monthSkill.Xp;
                        yearSkill.Xp = yearSkill.Xp < 0 ? 0 : yearSkill.Xp;
                        dxpStartSkill.Xp = dxpStartSkill.Xp < 0 ? 0 : dxpStartSkill.Xp;
                        dxpEndSkill.Xp = dxpEndSkill.Xp < 0 ? 0 : dxpEndSkill.Xp;
                    }

                    skillGain.DayGain = currentSkill.Xp - daySkill.Xp;
                    // we use yesterday's stats minus two days ago's stats to get total xp gain of yesterday's skilling
                    skillGain.YesterdayGain = daySkill.Xp - yesterdaySkill.Xp;
                    skillGain.LevelGain = currentSkill.Level - daySkill.Level;
                    skillGain.WeekGain = currentSkill.Xp - weekSkill.Xp;
                    skillGain.MonthGain = currentSkill.Xp - monthSkill.Xp;
                    skillGain.YearGain = currentSkill.Xp - yearSkill.Xp;
                    skillGain.DxpGain = (isDxpOver ? dxpEndSkill.Xp : currentSkill.Xp) - dxpStartSkill.Xp;
                }
                skillGains.Add(skillGain);
            }
            for (var i = 0; i < currentMinigames.Count - 1; i++)
            {
                var minigameGain = new MinigameGain();
                
                var currentMinigame = currentMinigames.ElementAt(i);
                
                minigameGain.MinigameId = currentMinigame.MinigameId;
                minigameGain.Score = currentMinigame.Score;
                minigameGain.Rank = currentMinigame.Rank;

                if (validTracking)
                {
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
                    
                    minigameGain.DayGain = currentMinigame.Score - dayMinigame.Score;
                    minigameGain.WeekGain = currentMinigame.Score - weekMinigame.Score;
                    minigameGain.MonthGain = currentMinigame.Score - monthMinigame.Score;
                    minigameGain.YearGain = currentMinigame.Score - yearMinigame.Score;
                }
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
        
        public async Task<ResponseWrapper<ICollection<String>>> GetFollowedPlayerNames(ApplicationUser user)
        {
            var response = new ResponseWrapper<ICollection<String>>
            {
                Success = true,
                Status = ""
            };
            
            var usernames = await _UserRepo.GetFollowedPlayerNames(user);
            response.Data = usernames;
            return response;
        }          
        public async Task<ResponseWrapper<ICollection<ActivityResponse>>> GetFollowedPlayerActivities(ApplicationUser user, int size)
        {
            var response = new ResponseWrapper<ICollection<ActivityResponse>>
            {
                Success = true,
                Status = ""
            };
            
            var activities = await _UserRepo.GetFollowedPlayerActivities(user, size);
            var activityTasks =  activities.Select(async a => await FormatActivity(a, user));
            var activityResponses = await Task.WhenAll(activityTasks);
            response.Data = activityResponses.ToList();
            return response;
        }         
        public async Task<ResponseWrapper<String>> FollowPlayer(String username, ApplicationUser user)
        {
            var response = new ResponseWrapper<String>
            {
                Success = true,
                Status = "", Data = username
            };
            
            var player = await _UserRepo.GetPlayerByUsername(username);
            if (player == null)
            {
                response.Success = false;
                response.Status = "Player does not exist in the database.";
                return response;
            }

            var follow = new Follow()
            {
                Player = player,
                User = user
            };

            var updatedPlayer = _UserRepo.FollowPlayer(follow, user);
            if (updatedPlayer == null)
            {
                response.Success = false;
                response.Status = "Follow action failed.";
                return response;
            }

            return response;
        }         
        public async Task<ResponseWrapper<Activity>> LikeActivity(ApplicationUser user, int activityId)
        {
            var response = new ResponseWrapper<Activity>
            {
                Success = true,
                Status = "",
            };
            
            var activity = await _UserRepo.GetActivityById(activityId);
            if (activity == null)
            {
                response.Success = false;
                response.Status = "Activity does not exist with id.";
                return response;
            }

            var activityLike = new ActivityLike()
            {
                Activity = activity,
                User = user
            };

            var updatedActivity = await _UserRepo.LikeActivity(activity, activityLike);
            if (updatedActivity == null)
            {
                response.Success = false;
                response.Status = "Like activity action failed.";
                return response;
            }

            response.Data = updatedActivity;

            return response;
        }         
        public async Task<ResponseWrapper<Activity>> UnlikeActivity(ApplicationUser user, int activityId)
        {
            var response = new ResponseWrapper<Activity>
            {
                Success = true,
                Status = "",
            };
            
            var activity = await _UserRepo.GetActivityById(activityId);
            if (activity == null)
            {
                response.Success = false;
                response.Status = "Activity does not exist with id.";
                return response;
            }

            var updatedActivity = await _UserRepo.UnlikeActivity(activity, user);
            if (updatedActivity == null)
            {
                response.Success = false;
                response.Status = "Unlike activity action failed.";
                return response;
            }

            response.Data = updatedActivity;

            return response;
        } 
        public async Task<ResponseWrapper<String>> UnfollowPlayer(String username, ApplicationUser user)
        {
            var response = new ResponseWrapper<String>
            {
                Success = true,
                Status = "",
                Data = username
            };
            
            var player = await _UserRepo.GetPlayerByUsername(username);
            if (player == null)
            {
                response.Success = false;
                response.Status = "Player does not exist in the database.";
                return response;
            }
            
            var updatedPlayer = await _UserRepo.UnfollowPlayer(player, user);
            if (updatedPlayer == null)
            {
                response.Success = false;
                response.Status = "Follow action failed.";
                return response;
            }
            
            return response;
        }        
        public async Task<ResponseWrapper<string>> UpdateRs3Rsn(String username, ApplicationUser user)
        {
            var response = new ResponseWrapper<string>
            {
                Success = true,
                Status = ""
            };
            
            var player = await _UserRepo.GetPlayerByUsername(username);
            if (player == null)
            {
                response.Success = false;
                response.Status = "Player does not exist in the database.";
                return response;
            }
            
            var updatedPlayer = await _UserRepo.UpdateRs3Rsn(username, user);
            if (updatedPlayer == false)
            {
                response.Success = false;
                response.Status = "Rsn could not be updated.";
                return response;
            }

            response.Data = username;

            return response;
        }        
        public async Task<ResponseWrapper<ICollection<String>>> SeedPlayersFromClanMemberList(String clanName)
        {
            var response = new ResponseWrapper<ICollection<String>>
            {
                Success = true,
                Status = ""
            };
            
            var clanMemberData = await OfficialApiCall(clanMemberListUrl + clanName);
            
            if (clanMemberData == null)
            {
                response.Success = false;
                response.Status = "Clan does not exist in the database.";
                return response;
            }

            var clanMemberList = clanMemberData.Split("\n")
                .Select(d => d
                    .Split(",")
                    .FirstOrDefault()
                    .ToLower()
                );
            var memberNames = clanMemberList.Select(u => Regex.Replace(u, @"[^0-9a-zA-Z]+", " ")).ToList();
            var currentPlayerNames = await _UserRepo.GetPlayerNames();
            var playersToAdd = memberNames.Except(currentPlayerNames).ToList();
            var createdPlayers = new List<String>();
            
            foreach (var p in playersToAdd)
            {
                var user = await CreateNewUser(p);
                if (user != null)
                {
                    createdPlayers.Add(user.Username);
                }
            }
            
            // var tasks = playersToAdd.Select(u => CreateNewUser(u));
            // await Task.WhenAll(tasks);

            response.Data = createdPlayers;
                return response;
        }

        public async Task<AccountType> GetAccountStatus(String username)
        {
            try
            {
                var hcimData = await OfficialApiCall(hcimHiscoreUrl + username);
                var imData = await OfficialApiCall(imHiscoreUrl + username);
                
                if (imData == null)
                {
                    // if iron data null, means they are main since they would otherwise have hiscore data
                    return AccountType.Main;
                }
                if (hcimData == null)
                {
                    // if iron data not null, but hcim null, must be regular iron
                    return AccountType.Ironman;
                }

                var imTotalXp = imData.Split('\n').First().Split(',').LastOrDefault();
                var hcimTotalXp = hcimData.Split('\n').First().Split(',').LastOrDefault();
                var hcimAndImHiscoresEqual = imTotalXp == hcimTotalXp;

                // if total xp is the same on im and hcim, must be hcim, otherwise dead hcim
                return hcimAndImHiscoresEqual ? AccountType.HardcoreIronman : AccountType.DeadHardcoreIronman;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return AccountType.Main;
            }
        }
        
        public async Task<ActivityResponse> FormatActivity(Activity activity, ApplicationUser? user)
        {
            var response = new ActivityResponse();
            response.UserId = activity.UserId;
            response.Id = activity.Id;
            response.Player = activity.Player;
            response.Title = activity.Title;
            response.Details = activity.Details;
            response.DateRecorded = activity.DateRecorded;

            if (activity.Likes != null)
            {
                response.Likes = activity.Likes.Count;
                if (user != null)
                {
                    response.CurrentUserLiked = activity.Likes.Any(l => l.User == user);
                }
            }

            if (activity.Title.Contains("Levelled up "))
            {
                var level = activity.Details.Split("level ")[1].Replace(".", "");
                var skill = activity.Title.Split("up ")[1].Replace(".", "");
                response.Title = $"{skill} level {level}";
            }
            
            if (activity.Title.Contains("XP in "))
            {
                var skill = activity.Title.Split("XP in ")[1].Replace(".", "");
                var level = activity.Title.Split("XP in ")[0].Replace(".", "");
                if (level.Substring(level.Length - 6) == "000000")
                {
                    level = level.Substring(0, level.Length - 6) + "m";
                }
                response.Title = $"{level} xp in {skill}";
            }

            // if (activity.Title.Contains("I killed "))
            // {
            //     var boss = activity.Title
            //         .Split("I killed ")[1]
            //         .Split(" ")[1]
            //         .Replace(".", "");
            //     var bossResponse = await OfficialApiCall("https://secure.runescape.com/m=itemdb_rs/bestiary/beastSearch.json?term=" + boss);
            //     var joResponse = JArray.Parse(bossResponse);
            //     var bossId = joResponse.FirstOrDefault()?.Value<int>("value");
            //     var bossInfoApiUrl = "https://secure.runescape.com/m=itemdb_rs/bestiary/beastData.json?beastid=" + bossId;
            //         
            //     var bossInfoResponse = await OfficialApiCall(bossInfoApiUrl);
            // }

            if (activity.Title.Contains("I found "))
            {
                var item = new string("");
                if (activity.Title.Contains("I found a "))
                {
                    item = activity.Title.Split("I found a ")[1];
                } else if (activity.Title.Contains("I found an "))
                {
                    item = activity.Title.Split("I found an ")[1];
                } else if (activity.Title.Contains("I found some "))
                {
                    item = activity.Title.Split("I found some ")[1];
                }
                else
                {
                    return response;
                }
                var itemPriceResponse =
                    await OfficialApiCall(Constants.ExternalApiItemPriceUrl + item.Replace(".", ""));
                try
                {
                    IDictionary<string, JToken> joResponse = JObject.Parse(itemPriceResponse);
                    var itemProperty = joResponse.First();
                    var itemData = (JObject)itemProperty.Value;
                    var price = (int)itemData["price"];
                    response.Price = price;
                    try
                    {
                        var itemId = (int)itemData["id"];
                        var itemDetailsResponseString =
                            await OfficialApiCall(Constants.RunescapeApiItemDetailsUrl + itemId);
                        IDictionary<string, JToken> itemDetailsJoResponse = JObject.Parse(itemDetailsResponseString);
                        var itemDetailsProperty = itemDetailsJoResponse.First();
                        var itemDetailsData = (JObject)itemDetailsProperty.Value;
                        var iconUri = (string)itemDetailsData["icon_large"];
                        response.IconUri = iconUri;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return response;
        }
    }
}