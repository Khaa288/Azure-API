using Microsoft.EntityFrameworkCore;

using AzureAPI.Entities;

namespace AzureAPI.Database;

public class AzureDbContext : DbContext {
    public AzureDbContext(DbContextOptions<AzureDbContext> options) : base(options) {}

    public DbSet<Note> Notes { get; set; }
    public DbSet<NoteDetail> NoteDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>()
            .HasKey(n => n.Id)
            .HasName("PK_NoteId");

        modelBuilder.Entity<NoteDetail>()
            .HasKey(n => n.Id)
            .HasName("PK_NoteDetailId");
    }
}

