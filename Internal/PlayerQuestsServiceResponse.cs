using System;

namespace dotnet5_webapp.Internal
{
    public class PlayerQuestsServiceResponse
    {
        public String Username { get; set; }
        public int TotalQuests { get; set; }
        public int CompletedQuests { get; set; }
        public int QuestPoints { get; set; }
        public int TotalQuestPoints { get; set; }
        public bool QuestCape { get; set; }
    }
}