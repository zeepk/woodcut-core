using dotnet5_webapp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace dotnet5_webapp.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<dotnet5_webapp.Models.StatRecord> StatRecord { get; set; }
        public DbSet<dotnet5_webapp.Models.Player> User { get; set; }
        public DbSet<dotnet5_webapp.Models.Skill> Skill { get; set; }
        public DbSet<dotnet5_webapp.Models.Minigame> Minigame { get; set; }
        public DbSet<dotnet5_webapp.Models.Activity> Activity { get; set; }
        public DbSet<dotnet5_webapp.Models.ActivityLike> ActivityLike { get; set; }
        public DbSet<dotnet5_webapp.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<dotnet5_webapp.Models.Follow> Follow { get; set; }

    }
}
