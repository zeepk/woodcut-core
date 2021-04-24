namespace dotnet5_webapp.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public int? StatRecordId { get; set; }
        public long Xp { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }


    }
}