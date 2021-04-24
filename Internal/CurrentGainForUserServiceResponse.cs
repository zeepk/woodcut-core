using System;
using System.Collections.Generic;

namespace dotnet5_webapp.Internal
{
    public class CurrentGainForUserServiceResponse
    {
        public String Username { get; set; }
        public ICollection<SkillGain> SkillGains { get; set; }
        public ICollection<MinigameGain> MinigameGains { get; set; }
    }
}