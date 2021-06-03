using System;
using System.Collections.Generic;
using System.Text;

namespace dotnet5_webapp.Utils
{
    public class Constants
    {

        public const string RunescapeApiBaseUrl = "https://secure.runescape.com/m=hiscore/index_lite.ws?player=";
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
    }
}