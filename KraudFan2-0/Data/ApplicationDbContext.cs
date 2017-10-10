using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KraudFan2_0.Models;
using KraudFan2_0.Models.AppData;
namespace KraudFan2_0.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<FinancialTarget> Targets { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Achievment> Achievments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SignProject> SignedProjects { get; set; }
        public DbSet<UserAchievments> UserAchievments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ProjectTags>()
           .HasKey(t => new { t.ProjectId, t.TagId });

            builder.Entity<ProjectTags>()
                .HasOne(sc => sc.Project)
                .WithMany(s => s.ProjectTags)
                .HasForeignKey(sc => sc.ProjectId);

            builder.Entity<ProjectTags>()
                .HasOne(sc => sc.Tag)
                .WithMany(c => c.ProjectTags)
                .HasForeignKey(sc => sc.TagId);
            builder.Entity<ApplicationUser>().Property(u => u.locale).HasDefaultValue("RU");
            builder.Entity<ApplicationUser>().Property(u => u.status).HasDefaultValue("Not verified");
            builder.Entity<ApplicationUser>().Property(u => u.Banned).HasDefaultValue(false);
        }
    }
}
