using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BitesByte_API.Model;

public partial class BitesByteDbContext : DbContext
{
    public BitesByteDbContext()
    {
    }

    public BitesByteDbContext(DbContextOptions<BitesByteDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Menu> Menus { get; set; }

    

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=host.docker.internal,1433;Database=BitesByteDB;User Id=sha;Password=YourStrong!Passw0rd;Trusted_Connection=false;Persist Security Info=False;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.Carbs).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Category)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Fat).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.MenuName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Plan)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Protein).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalCalories).HasColumnType("decimal(18, 4)");
        });

      
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Password).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
