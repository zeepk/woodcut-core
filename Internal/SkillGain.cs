namespace dotnet5_webapp.Internal
{
    public class SkillGain
    {
        public int SkillId { get; set; }
        public long Xp { get; set; }
        public int Level { get; set; }
        public int LevelGain { get; set; }
        public int Rank { get; set; }
        public long DayGain { get; set; }
        public long YesterdayGain { get; set; }
        public long WeekGain { get; set; }
        public long MonthGain { get; set; }
        public long YearGain { get; set; }
        public long? DxpGain { get; set; }
    }
}