using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet5_webapp.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Player Player { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string DateRecordedStamp { get; set; }
        public DateTime DateRecorded { get; set; }
        public List<ActivityLike> Likes { get; set; }
    }
}