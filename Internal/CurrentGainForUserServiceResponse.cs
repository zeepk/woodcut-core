using System;
using System.Collections.Generic;
using dotnet5_webapp.Utils;

namespace dotnet5_webapp.Internal
{
    public class CurrentGainForUserServiceResponse
    {
        public String Username { get; set; }
        public String? DisplayName { get; set; }
        public String? StatusMessage { get; set; }
        public ICollection<SkillGain>? SkillGains { get; set; }
        public ICollection<MinigameGain>? MinigameGains { get; set; }
        public ICollection<Constants.BadgeType>? Badges { get; set; }
    }
}