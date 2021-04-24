namespace dotnet5_webapp.Internal
{
    public class CurrentGainForUserServiceResponse
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public long Xp { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int DayGain { get; set; }
        public int WeekGain { get; set; }
        public int MonthGain { get; set; }
        public int YearGain { get; set; }
        public int? DxpGain { get; set; }
    }
}