namespace dotnet5_webapp.Models
{
    public class Minigame
    {
        public int Id { get; set; }
        public int MinigameId { get; set; }
        public int? StatRecordId { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
    }
}