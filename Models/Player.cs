﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet5_webapp.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public DateTime DateCreated { get; set; }
        public List<StatRecord> StatRecords { get; set; }
        public bool IsTracking { get; set; }
    }
}
