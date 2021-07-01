using System.Collections;
using System.Collections.Generic;
using dotnet5_webapp.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace dotnet5_webapp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Follow> FollowingPlayers { get; set; }
        public string Rs3Rsn { get; set; }
        public string OsrsRsn { get; set; }
    }
}