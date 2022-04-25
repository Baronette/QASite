using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace QASite.Data
{
    public class QASiteContext : DbContext
    {
        private string _connectionString;
        public QASiteContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<QuestionsTags>()
    .HasKey(qt => new { qt.QuestionId, qt.TagId });

            modelBuilder.Entity<QuestionLike>()
               .HasKey(l => new { l.QuestionId, l.UserId });

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionLike> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionsTags> QuestionsTags { get; set; }
    }
}


