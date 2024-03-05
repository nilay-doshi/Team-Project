using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Team.Repo.Models;

namespace Team.Repo
{
    public class TeamDBContext:DbContext
    {
        public TeamDBContext(DbContextOptions<TeamDBContext> options):base(options) { }

        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<UserRegistration> Registration { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coach>().HasData(
                new Coach
                {
                    Coachid = 1,
                    Email = "nilaydoshi@gmail.com",
                    FirstName = "Nilay",
                    LastName = "Doshi",
                    ContactNumber = "9033062657",
                    Dob = new DateOnly(1999, 01, 02),
                    FlagRole = 5
                }
               );

            base.OnModelCreating(modelBuilder);
        }

    }

}
