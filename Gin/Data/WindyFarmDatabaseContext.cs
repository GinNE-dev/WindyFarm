﻿using System;
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

    public virtual DbSet<BarnDat> BarnDats { get; set; }

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
            entity.HasKey(e => e.Email).HasName("PK__Account__A9D10535FFDED4C5");

            entity.ToTable("Account");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HashedPassword).HasMaxLength(255);
        });

        modelBuilder.Entity<BarnDat>(entity =>
        {
            entity.HasKey(e => new { e.OwnerId, e.SlotIndex }).HasName("PK__BarnDat__389A2CC4F2B1FE50");

            entity.ToTable("BarnDat");

            entity.Property(e => e.GiveProductAt)
                .HasDefaultValueSql("(dateadd(year,(100),getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.GrowAt)
                .HasDefaultValueSql("(dateadd(year,(100),getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.LastFeedAt)
                .HasDefaultValueSql("(dateadd(year,(100),getdate()))")
                .HasColumnType("datetime");
            entity.Property(e => e.LastTimeMarkerUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Owner).WithMany(p => p.BarnDats)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BarnDat__OwnerId__503BEA1C");
        });

        modelBuilder.Entity<FarmShop>(entity =>
        {
            entity.HasKey(e => e.SlotIndex).HasName("PK__FarmShop__909A97CA0F8B5FBB");

            entity.ToTable("FarmShop");

            entity.Property(e => e.SlotIndex).ValueGeneratedNever();
        });

        modelBuilder.Entity<FarmlandDat>(entity =>
        {
            entity.HasKey(e => new { e.OwnerId, e.PlotIndex }).HasName("PK__Farmland__19DA69EB3E42E5FB");

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
                .HasConstraintName("FK__FarmlandD__Owner__73BA3083");
        });

        modelBuilder.Entity<InventorySlotDat>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.Slot }).HasName("PK__Inventor__3189CE5C4C45DAB2");

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
            entity.HasKey(e => e.Id).HasName("PK__ItemDat__3214EC0774CBB5BD");

            entity.ToTable("ItemDat");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Quality).HasDefaultValue(1);
        });

        modelBuilder.Entity<ItemSellPrice>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__ItemSell__727E838BEDBD170C");

            entity.Property(e => e.ItemId).ValueGeneratedNever();
        });

        modelBuilder.Entity<PlayerDat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerDa__3214EC07D90BFF77");

            entity.ToTable("PlayerDat");

            entity.HasIndex(e => e.AccountId, "UQ__PlayerDa__349DA5A73E8F983C").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountId).HasMaxLength(255);
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Energy).HasDefaultValue(50);
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
