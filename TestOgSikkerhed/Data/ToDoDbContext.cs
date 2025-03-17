using Microsoft.EntityFrameworkCore;

namespace TestOgSikkerhed.Data
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
        {
        }

        public DbSet<Cpr> CprRecords { get; set; } = null!;
        public DbSet<ToDo> ToDoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDo>()
                .HasOne(t => t.Cpr)
                .WithMany(c => c.ToDoItems)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
