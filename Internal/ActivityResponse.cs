using System.Diagnostics;
using Activity = dotnet5_webapp.Models.Activity;

namespace dotnet5_webapp.Internal
{
    public class ActivityResponse : Activity
    {
        public int? Price { get; set; }
        public string? IconUri { get; set; }
        public int Likes { get; set; }
        public bool CurrentUserLiked { get; set; }
    }
}