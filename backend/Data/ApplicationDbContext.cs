using Dqsm.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Dqsm.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserMsg> UserMsgs { get; set; }
        public DbSet<TemplateMsg> TemplateMsgs { get; set; }
        public DbSet<Logs> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity properties if needed (explicit configuration)
            modelBuilder.Entity<UserMsg>().ToTable("UserMsg");
            modelBuilder.Entity<TemplateMsg>().ToTable("TemplateMsg");
            modelBuilder.Entity<Logs>().ToTable("Logs");
        }
    }
}
