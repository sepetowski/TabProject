﻿using Microsoft.EntityFrameworkCore;
using TabProjectServer.Models.Domain;

namespace TabProjectServer.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions dbContextOptions):base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");


            var roles = new List<Role>
            {
                new()
                {
                      Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                      UserRole=UserRole.Admin,
                      RoleKey="7k7usefUN4kfBIVJ7XnR4NYTC61ioWOB"

                },

                new()
                {
                      Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                      UserRole=UserRole.User
                },
            };

            modelBuilder.Entity<Role>().HasData(roles);
        }

    }
}