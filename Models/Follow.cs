namespace dotnet5_webapp.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public Player Player { get; set; }
        public ApplicationUser User { get; set; }
    }
}