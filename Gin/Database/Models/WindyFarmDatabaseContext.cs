using System;
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

    public virtual DbSet<PlayerDat> PlayerDats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Account__A9D10535F3D85831");

            entity.ToTable("Account");

            entity.HasIndex(e => e.PlayerId, "UQ__Account__4A4E74C91CF47387").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HashedPassword).HasMaxLength(255);

            entity.HasOne(d => d.Player).WithOne(p => p.Account)
                .HasForeignKey<Account>(d => d.PlayerId)
                .HasConstraintName("FK_Account_Player");
        });

        modelBuilder.Entity<PlayerDat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerDa__3214EC079D570C30");

            entity.ToTable("PlayerDat");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DisplayName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
