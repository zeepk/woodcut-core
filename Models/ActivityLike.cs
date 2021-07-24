namespace dotnet5_webapp.Models
{
    public class ActivityLike
    {
        public int Id { get; set; }
        public Activity Activity { get; set; }
        public ApplicationUser User { get; set; }
    }
}