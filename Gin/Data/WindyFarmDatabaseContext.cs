using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WindyFarm.Gin.Data;

public partial class WindyFarmDatabaseContext : DbContext
{
    public WindyFarmDatabaseContext(DbContextOptions<WindyFarmDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<FarmShop> FarmShops { get; set; }

    public virtual DbSet<FarmlandDat> FarmlandDats { get; set; }

    public virtual DbSet<InventorySlotDat> InventorySlotDats { get; set; }

    public virtual DbSet<ItemDat> ItemDats { get; set; }

    public virtual DbSet<ItemSellPrice> ItemSellPrices { get; set; }

    public virtual DbSet<PlayerDat> PlayerDats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Account__A9D10535FF3D8303");

            entity.ToTable("Account");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HashedPassword).HasMaxLength(255);
        });

        modelBuilder.Entity<FarmShop>(entity =>
        {
            entity.HasKey(e => e.SlotIndex).HasName("PK__FarmShop__909A97CA3C2A23FD");

            entity.ToTable("FarmShop");

            entity.Property(e => e.SlotIndex).ValueGeneratedNever();
        });

        modelBuilder.Entity<FarmlandDat>(entity =>
        {
            entity.HasKey(e => new { e.OwnerId, e.PlotIndex }).HasName("PK__Farmland__19DA69EB49F8F4F4");

            entity.ToTable("FarmlandDat");

            entity.Property(e => e.CropQuality).HasDefaultValue(1);
            entity.Property(e => e.PlantedAt)
                .HasDefaultValueSql("(dateadd(year,(100),getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.PlotState)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Wild");

            entity.HasOne(d => d.Owner).WithMany(p => p.FarmlandDats)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FarmlandD__Owner__71D1E811");
        });

        modelBuilder.Entity<InventorySlotDat>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.Slot }).HasName("PK__Inventor__3189CE5C72D9B0DD");

            entity.ToTable("InventorySlotDat");

            entity.HasOne(d => d.ItemDat).WithMany(p => p.InventorySlotDats)
                .HasForeignKey(d => d.ItemDatId)
                .HasConstraintName("FK_Inventory_Item");

            entity.HasOne(d => d.Player).WithMany(p => p.InventorySlotDats)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Player");
        });

        modelBuilder.Entity<ItemDat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ItemDat__3214EC0759BFA461");

            entity.ToTable("ItemDat");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Quality).HasDefaultValue(1);
        });

        modelBuilder.Entity<ItemSellPrice>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__ItemSell__727E838B29450A6F");

            entity.Property(e => e.ItemId).ValueGeneratedNever();
        });

        modelBuilder.Entity<PlayerDat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerDa__3214EC07DFC23290");

            entity.ToTable("PlayerDat");

            entity.HasIndex(e => e.AccountId, "UQ__PlayerDa__349DA5A788737371").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountId).HasMaxLength(255);
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Level).HasDefaultValue(1);
            entity.Property(e => e.MaxInventory).HasDefaultValue(25);

            entity.HasOne(d => d.Account).WithOne(p => p.PlayerDat)
                .HasForeignKey<PlayerDat>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PlayerDat_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
