using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using RadzenBlazorServerADDemo.Models.ConData;

namespace RadzenBlazorServerADDemo.Data
{
    public partial class ConDataContext : DbContext
    {
        public ConDataContext()
        {
        }

        public ConDataContext(DbContextOptions<ConDataContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RadzenBlazorServerADDemo.Models.ConData.FetchRolesForAdUser>().HasNoKey();

            builder.Entity<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole>()
              .HasOne(i => i.SolutionRole)
              .WithMany(i => i.SolutionUsersInRoles)
              .HasForeignKey(i => i.RoleID)
              .HasPrincipalKey(i => i.RoleID);

            builder.Entity<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole>()
              .HasOne(i => i.SolutionUser)
              .WithMany(i => i.SolutionUsersInRoles)
              .HasForeignKey(i => i.UserID)
              .HasPrincipalKey(i => i.UserID);
            this.OnModelBuilding(builder);
        }

        public DbSet<RadzenBlazorServerADDemo.Models.ConData.SolutionRole> SolutionRoles { get; set; }

        public DbSet<RadzenBlazorServerADDemo.Models.ConData.SolutionUser> SolutionUsers { get; set; }

        public DbSet<RadzenBlazorServerADDemo.Models.ConData.SolutionUsersInRole> SolutionUsersInRoles { get; set; }

        public DbSet<RadzenBlazorServerADDemo.Models.ConData.FetchRolesForAdUser> FetchRolesForAdUsers { get; set; }
    }
}