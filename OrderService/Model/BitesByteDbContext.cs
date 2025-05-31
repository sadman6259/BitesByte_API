using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Model;

public partial class BitesByteDbContext : DbContext
{
    public BitesByteDbContext()
    {
    }

    public BitesByteDbContext(DbContextOptions<BitesByteDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GuestUser> GuestUsers { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=host.docker.internal,1433;Database=BitesByteDB;User Id=sha;Password=YourStrong!Passw0rd; Trusted_Connection=false; TrustServerCertificate=Yes; MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GuestUser>(entity =>
        {
            entity.ToTable("Guest_User").HasKey(u => u.Id);

            entity.Property(e => e.ApartmentUnit).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(500);
            entity.Property(e => e.CompanyName).HasMaxLength(500);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FirstName).HasMaxLength(500);
            entity.Property(e => e.LastName).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PostCode).HasMaxLength(500);
            entity.Property(e => e.State).HasMaxLength(500);
            entity.Property(e => e.Street).HasMaxLength(500);
        });

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
            entity.Property(e => e.PricePerGram).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Protein).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.Subcategories)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TotalCalories).HasColumnType("decimal(18, 4)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order").HasKey(u => u.Id); 
              

            entity.Property(e => e.CardNumber)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeliveryDateTime).HasColumnType("datetime");
            entity.Property(e => e.DeliveryMethod)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OrderReferenceNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.PickupLocation).HasMaxLength(3000);
            entity.Property(e => e.Remarks).HasMaxLength(4000);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.CreatedTime).HasColumnType("datetime");
            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.OrderReferenceNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalGrams).HasColumnType("decimal(18, 4)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Password).HasMaxLength(500);
        });

        modelBuilder.Entity<GuestUser>()
        .HasOne<Order>()
        .WithOne()
        .HasForeignKey<Order>(o => o.UserId);

        modelBuilder.HasSequence("OrderSequence");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
