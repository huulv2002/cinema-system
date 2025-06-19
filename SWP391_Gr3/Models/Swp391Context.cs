using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SWP391_Gr3.Models;

public partial class Swp391Context : DbContext
{
    public Swp391Context()
    {
    }

    public Swp391Context(DbContextOptions<Swp391Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderCombo> OrderCombos { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductCombo> ProductCombos { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PromotionType> PromotionTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatType> SeatTypes { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public virtual DbSet<Theater> Theaters { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-MK7FI4T;Initial Catalog=swp391; Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Combo__3214EC07324F7A08");

            entity.ToTable("Combo");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Theater).WithMany(p => p.Combos)
                .HasForeignKey(d => d.TheaterId)
                .HasConstraintName("FK__Combo__TheaterId__2B3F6F97");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3214EC072C1D9724");

            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Image__3214EC0744000432");

            entity.ToTable("Image");

            entity.HasOne(d => d.Movie).WithMany(p => p.Images)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Image__MovieId__4CA06362");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Movie__3214EC07E6175F41");

            entity.ToTable("Movie");

            entity.Property(e => e.Director).HasMaxLength(100);
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Performer).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasMany(d => d.Genres).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MovieGenr__Genre__49C3F6B7"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MovieGenr__Movie__48CFD27E"),
                    j =>
                    {
                        j.HasKey("MovieId", "GenreId").HasName("PK__MovieGen__BBEAC44DC71A7AEF");
                        j.ToTable("MovieGenre");
                    });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3214EC076F81D665");

            entity.ToTable("Order");

            entity.HasIndex(e => e.PaymentId, "UQ__Order__9B556A39EB51B939").IsUnique();

            entity.HasOne(d => d.Payment).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.PaymentId)
                .HasConstraintName("FK__Order__PaymentId__571DF1D5");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Order__Promotion__5812160E");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserId__5629CD9C");
        });

        modelBuilder.Entity<OrderCombo>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ComboId }).HasName("PK__OrderCom__3E447E4D1C662EFB");

            entity.ToTable("OrderCombo");

            entity.HasOne(d => d.Combo).WithMany(p => p.OrderCombos)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderComb__Combo__6383C8BA");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderCombos)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderComb__Order__628FA481");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__OrderPro__08D097A3038EED10");

            entity.ToTable("OrderProduct");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderProd__Order__5EBF139D");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderProd__Produ__5FB337D6");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC0708597CEA");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC07F74F1585");

            entity.ToTable("Product");

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Size).HasMaxLength(50);

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("FK__Product__Product__267ABA7A");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC077DB1A046");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ProductCombo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC070D6EF580");

            entity.ToTable("ProductCombo");

            entity.HasOne(d => d.Combo).WithMany(p => p.ProductCombos)
                .HasForeignKey(d => d.ComboId)
                .HasConstraintName("FK__ProductCo__Combo__2F10007B");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCombos)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductCo__Produ__2E1BDC42");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC0726B41E8F");

            entity.ToTable("Promotion");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Value).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.PromotionType).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.PromotionTypeId)
                .HasConstraintName("FK__Promotion__Promo__4222D4EF");
        });

        modelBuilder.Entity<PromotionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Promotio__3214EC07AAE01C92");

            entity.ToTable("PromotionType");

            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC07D6E249A4");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Room__3214EC07A00BB246");

            entity.ToTable("Room");

            entity.Property(e => e.Code).HasMaxLength(50);

            entity.HasOne(d => d.Theater).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.TheaterId)
                .HasConstraintName("FK__Room__TheaterId__31EC6D26");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seat__3214EC07DAE4E515");

            entity.ToTable("Seat");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Position).HasMaxLength(10);

            entity.HasOne(d => d.Room).WithMany(p => p.Seats)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Seat__RoomId__37A5467C");

            entity.HasOne(d => d.Type).WithMany(p => p.Seats)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Seat__TypeId__36B12243");
        });

        modelBuilder.Entity<SeatType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SeatType__3214EC07D9653DCC");

            entity.ToTable("SeatType");

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Showtime__3214EC07F7C302C1");

            entity.ToTable("Showtime");

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Showtime__MovieI__4F7CD00D");

            entity.HasOne(d => d.Room).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Showtime__RoomId__5070F446");
        });

        modelBuilder.Entity<Theater>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Theater__3214EC07FD9DD51C");

            entity.ToTable("Theater");

            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket__3214EC07A2698DA5");

            entity.ToTable("Ticket");

            entity.Property(e => e.Code).HasMaxLength(50);

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK__Ticket__SeatId__5BE2A6F2");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ShowtimeId)
                .HasConstraintName("FK__Ticket__Showtime__5AEE82B9");
            entity.HasOne(d => d.Order)
                .WithMany(p => p.Tickets)         
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Ticket__OrderId__XXXXXXX");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC074F8C6C1F");

            entity.ToTable("Transaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Seat).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Seat");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.ShowtimeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Showtime");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07263DB969");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CodeExpiration).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.HashPass).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.VerificationCode)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__RoleId__3C69FB99");

            entity.HasOne(d => d.Theater).WithMany(p => p.Users)
                .HasForeignKey(d => d.TheaterId)
                .HasConstraintName("FK__User__TheaterId__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
