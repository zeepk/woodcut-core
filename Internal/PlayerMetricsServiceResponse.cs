using System;
using System.Collections.Generic;
using dotnet5_webapp.Models;

namespace dotnet5_webapp.Internal
{
    public class PlayerMetricsServiceResponse
    {
        public String Username { get; set; }
        public int QuestsComplete { get; set; }
        public List<ActivityResponse> Activities { get; set; }
    }
}