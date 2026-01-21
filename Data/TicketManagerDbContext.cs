using Microsoft.EntityFrameworkCore;
using TicketManager.Models;

namespace TicketManager.Data;

public class TicketManagerDbContext : DbContext
{
    public TicketManagerDbContext(DbContextOptions<TicketManagerDbContext> opt) : base(opt) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Lastname)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne<Role>(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Description)
                .IsRequired();
            entity.Property(e => e.State)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()");

            entity.HasOne<User>(e => e.CreatedByUser)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>(e => e.AssignedToUser)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.SentAt)
                .IsRequired()
                .HasDefaultValueSql("now()");

            entity.HasOne<Ticket>(e => e.Ticket)
                .WithMany()
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<User>(e => e.SenderUser)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(e => e.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}