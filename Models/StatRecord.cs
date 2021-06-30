using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet5_webapp.Models
{
    public class StatRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Player Player { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Minigame> Minigames { get; set; }

    }
}