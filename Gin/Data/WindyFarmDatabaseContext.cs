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

    public virtual DbSet<BarnDat> BarnDats { get; set; }

    public virtual DbSet<CakeShop> CakeShops { get; set; }

    public virtual DbSet<CraftingSlotDat> CraftingSlotDats { get; set; }

    public virtual DbSet<FarmShop> FarmShops { get; set; }

    public virtual DbSet<FarmlandDat> FarmlandDats { get; set; }

    public virtual DbSet<FriendshipDat> FriendshipDats { get; set; }

    public virtual DbSet<InventorySlotDat> InventorySlotDats { get; set; }

    public virtual DbSet<ItemDat> ItemDats { get; set; }

    public virtual DbSet<ItemSellPrice> ItemSellPrices { get; set; }

    public virtual DbSet<MailDat> MailDats { get; set; }

    public virtual DbSet<MailMessage> MailMessages { get; set; }

    public virtual DbSet<PlayerDat> PlayerDats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK__Account__A9D10535908B8A81");

            entity.ToTable("Account");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.HashedPassword).HasMaxLength(255);
        });

        modelBuilder.Entity<BarnDat>(entity =>
        {
            entity.HasKey(e => new { e.OwnerId, e.SlotIndex }).HasName("PK__BarnDat__389A2CC4452A11FA");

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
                .HasConstraintName("FK__BarnDat__OwnerId__00200768");
        });

        modelBuilder.Entity<CakeShop>(entity =>
        {
            entity.HasKey(e => e.SlotIndex).HasName("PK__CakeShop__909A97CAF92A941B");

            entity.ToTable("CakeShop");

            entity.Property(e => e.SlotIndex).ValueGeneratedNever();
        });

        modelBuilder.Entity<CraftingSlotDat>(entity =>
        {
            entity.HasKey(e => new { e.OwnerId, e.SlotIndex }).HasName("PK__Crafting__389A2CC4E73EE0B5");

            entity.ToTable("CraftingSlotDat");

            entity.Property(e => e.CompleteAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MaterialQuality).HasDefaultValue(1);
        });

        modelBuilder.Entity<FarmShop>(entity =>
        {
            entity.HasKey(e => e.SlotIndex).HasName("PK__FarmShop__909A97CAF817276E");

            entity.ToTable("FarmShop");

            entity.Property(e => e.SlotIndex).ValueGeneratedNever();
        });

        modelBuilder.Entity<FarmlandDat>(entity =>
        {
            entity.HasKey(e => new { e.OwnerId, e.PlotIndex }).HasName("PK__Farmland__19DA69EB364DC3F4");

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
                .HasConstraintName("FK__FarmlandD__Owner__74AE54BC");
        });

        modelBuilder.Entity<FriendshipDat>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.OtherPlayerId }).HasName("PK__Friendsh__84317C9C6CA05CA4");

            entity.ToTable("FriendshipDat");

            entity.Property(e => e.AchieveRelationshipAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FriendshipStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Invite");

            entity.HasOne(d => d.OtherPlayer).WithMany(p => p.FriendshipDatOtherPlayers)
                .HasForeignKey(d => d.OtherPlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Friendshi__Other__18EBB532");

            entity.HasOne(d => d.Player).WithMany(p => p.FriendshipDatPlayers)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Friendshi__Playe__17F790F9");
        });

        modelBuilder.Entity<InventorySlotDat>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.Slot }).HasName("PK__Inventor__3189CE5C6652EA17");

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
            entity.HasKey(e => e.Id).HasName("PK__ItemDat__3214EC074611E7C9");

            entity.ToTable("ItemDat");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Quality).HasDefaultValue(1);
        });

        modelBuilder.Entity<ItemSellPrice>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__ItemSell__727E838BD1A29AB8");

            entity.Property(e => e.ItemId).ValueGeneratedNever();
        });

        modelBuilder.Entity<MailDat>(entity =>
        {
            entity.HasKey(e => e.MailId).HasName("PK__MailDat__09A8749A3C441E7A");

            entity.ToTable("MailDat");

            entity.Property(e => e.MailId).ValueGeneratedNever();
            entity.Property(e => e.PlayerOneTreat)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValue("New");
            entity.Property(e => e.PlayerTwoTreat)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasDefaultValue("New");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.PlayerOne).WithMany(p => p.MailDatPlayerOnes)
                .HasForeignKey(d => d.PlayerOneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MailDat__PlayerO__208CD6FA");

            entity.HasOne(d => d.PlayerTwo).WithMany(p => p.MailDatPlayerTwos)
                .HasForeignKey(d => d.PlayerTwoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MailDat__PlayerT__2180FB33");
        });

        modelBuilder.Entity<MailMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MailMess__3214EC07212670D3");

            entity.ToTable("MailMessage");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MessageContent)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Mail).WithMany(p => p.MailMessages)
                .HasForeignKey(d => d.MailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MailMessa__MailI__2739D489");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MailMessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MailMessa__Recei__2645B050");

            entity.HasOne(d => d.Sender).WithMany(p => p.MailMessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MailMessa__Sende__25518C17");
        });

        modelBuilder.Entity<PlayerDat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerDa__3214EC071A18277D");

            entity.ToTable("PlayerDat");

            entity.HasIndex(e => e.AccountId, "UQ__PlayerDa__349DA5A7049124E1").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountId).HasMaxLength(255);
            entity.Property(e => e.DisplayName).HasMaxLength(255);
            entity.Property(e => e.Energy).HasDefaultValue(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Gold).HasDefaultValue(5000);
            entity.Property(e => e.LastActiveAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
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
