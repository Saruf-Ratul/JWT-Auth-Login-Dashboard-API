// Data/AppDbContext.cs

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the Identity tables and fields.

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("UserId");
            // Add additional customization for ApplicationUser if needed
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("RoleId");
            // Add additional customization for IdentityRole if needed
        });

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.RoleId).HasColumnName("RoleId");
            // Add additional customization for IdentityUserRole if needed
        });

        // Add additional configurations for other Identity entities if needed

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("AppointmentId");
            // Add additional customization for Appointment if needed
        });

        // Add additional configurations for other entities if needed
    }
}