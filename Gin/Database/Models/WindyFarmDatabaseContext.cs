﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WindyFarm.Gin.Database.Models;

public partial class WindyFarmDatabaseContext : DbContext
{
    public WindyFarmDatabaseContext(DbContextOptions<WindyFarmDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<InventorySlot> InventorySlots { get; set; }

    public virtual DbSet<ItemMetaDat> ItemDats { get; set; }

    public virtual DbSet<PlayerDat> PlayerDats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Account__A9D105350E87D5A3");

            entity.ToTable("Account");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HashedPassword).HasMaxLength(255);
        });

        modelBuilder.Entity<InventorySlot>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.Slot }).HasName("PK__Inventor__3189CE5C4194E1FF");

            entity.ToTable("InventorySlot");

            entity.HasOne(d => d.ItemDataNavigation).WithMany(p => p.InventorySlots)
                .HasForeignKey(d => d.ItemData)
                .HasConstraintName("FK_Inventory_Item");

            entity.HasOne(d => d.Player).WithMany(p => p.InventorySlots)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Player");
        });

        modelBuilder.Entity<ItemMetaDat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ItemDat__3214EC07D5DDA88E");

            entity.ToTable("ItemDat");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<PlayerDat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerDa__3214EC07BDA161EB");

            entity.ToTable("PlayerDat");

            entity.HasIndex(e => e.AccountId, "UQ__PlayerDa__349DA5A73734E67E").IsUnique();

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
