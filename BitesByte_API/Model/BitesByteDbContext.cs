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

    public virtual DbSet<Menu> Menu { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=BitesByteDB;Trusted_Connection=True;TrustServerCertificate=Yes;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
        {
            //entity
            //    .HasNoKey()
            //    .ToTable("Menu");

            entity.Property(e => e.Carbs).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Category)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Fat).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MenuName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Protein).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalCalories).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Plan)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Image).HasColumnType("byte[]");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
