using Diary.Models.Configurations;
using Diary.Models.Domains;
using Diary.Properties;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Diary
{
    public class ApplicationDbContext : DbContext
    {
        private static string _conString =
        $@"Server=({Settings.Default.ServerName})\{Settings.Default.AddressName};
         Database={Settings.Default.DbName};
        User Id = {Settings.Default.user}; 
        Password={Settings.Default.password};";

        public ApplicationDbContext()
            : base(_conString)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new StudentConfiguration());
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());

        }

    }


}