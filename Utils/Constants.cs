using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet5_webapp.Utils
{
    public class Constants
    {
        public const bool dxpEnabled = true;
        public const string dxpStartDateString = "2021/08/06";
        public const string dxpEndDateString = "2021/08/16";

        public const string RunescapeApiBaseUrl = "https://secure.runescape.com/m=hiscore/index_lite.ws?player=";
        public const string RunescapeImApiBaseUrl = "https://secure.runescape.com/m=hiscore_ironman/index_lite.ws?player=";
        public const string RunescapeHcimApiBaseUrl = "https://secure.runescape.com/m=hiscore_hardcore_ironman/index_lite.ws?player=";
        public const string RunescapeApiPlayerDetailsUrlPre = "https://secure.runescape.com/m=website-data/playerDetails.ws?names=%5B%22";
        public const string RunescapeApiPlayerDetailsUrlPost = "%22%5D&callback=jQuery000000000000000_0000000000&_=0";
        public const string RunescapeApiPlayerMetricsUrlPre = "https://apps.runescape.com/runemetrics/profile/profile?user=";
        public const string RunescapeApiPlayerMetricsUrlPost = "&activities=20";
        public const string RunescapeApiPlayerCount = "http://www.runescape.com/player_count.js?varname=iPlayerCount&callback=jQuery000000000000000_0000000000&_=0";
        public const string RunescapeApiQuestsUrl = "https://apps.runescape.com/runemetrics/quests?user=";
        public const string ExternalApiItemPriceUrl = "https://api.weirdgloop.org/exchange/history/rs/latest?name=";
        public const string RunescapeApiItemImageUrl = "https://secure.runescape.com/m=itemdb_rs/1625481579641_obj_big.gif?id=";
        public const string RunescapeApiItemDetailsUrl = "https://secure.runescape.com/m=itemdb_rs/api/catalogue/detail.json?item=";
        public const string RunescapeApiClanMemberListUrl = "http://services.runescape.com/m=clan-hiscores/members_lite.ws?clanName=";
        public const string QuestStatusCompleted = "COMPLETED";
        public const string QuestStatusStarted = "STARTED";
        public const string QuestStatusNotStarted = "NOT_STARTED";
        public const int TotalSkills = 28;
        public const long MaxXp = 5600000000;
        public const int MaxTotal = 2898;
        public readonly string[] SkillNames = {
            "Overall",
            "Attack",
            "Defence",
            "Strength",
            "Constitution",
            "Ranged",
            "Prayer",
            "Magic",
            "Cooking",
            "Woodcutting",
            "Fletching",
            "Fishing",
            "Firemaking",
            "Crafting",
            "Smithing",
            "Mining",
            "Herblore",
            "Agility",
            "Thieving",
            "Slayer",
            "Farming",
            "Runecrafting",
            "Hunter",
            "Construction",
            "Summoning",
            "Dungeoneering",
            "Divination",
            "Invention",
            "Archaeology"
        };
        public enum BadgeType
        {
            Maxed = 1,
            MaxTotal = 2,
            All120 = 3,
            MaxXp = 4,
            QuestCape = 5,
        }
        public readonly string[] UnimportantActivities = {

        };        
        public string[] ImportantMilestones = {
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
    }
}