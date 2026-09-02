using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HexMapEditor;

public partial class RosegoldrailroadContext : DbContext
{
    public RosegoldrailroadContext()
    {
    }

    public RosegoldrailroadContext(DbContextOptions<RosegoldrailroadContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<Grid> Grids { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<NanaStock> NanaStocks { get; set; }

    public virtual DbSet<RandomItem> RandomItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:rosegoldrailroad.database.windows.net,1433;Initial Catalog=rosegoldrailroad;Persist Security Info=False;User ID=moleadmin;Password='railroadH3';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.HasKey(e => e.FileName).HasName("PK__Assets__589E6EED3CC73A7D");

            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Attribute>(entity =>
        {
            entity.HasKey(e => e.AttributeId).HasName("PK__Attribut__C189298A8B7E99F5");

            entity.ToTable("Attribute");

            entity.Property(e => e.AttributeId)
                .ValueGeneratedNever()
                .HasColumnName("AttributeID");
            entity.Property(e => e.AttributeDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AttributeValue)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

            entity.HasOne(d => d.ItemType).WithMany(p => p.Attributes)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK__Attribute__ItemT__09A971A2");
        });

        modelBuilder.Entity<Grid>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Grid");

            entity.Property(e => e.Grid1)
                .HasMaxLength(8000)
                .IsUnicode(false)
                .HasColumnName("Grid");
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.ItemTypeId).HasName("PK__ItemType__F51540DBB37204B7");

            entity.ToTable("ItemType");

            entity.Property(e => e.ItemTypeId)
                .ValueGeneratedNever()
                .HasColumnName("ItemTypeID");
            entity.Property(e => e.ItemTypeValue)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NanaStock>(entity =>
        {
            entity.HasKey(e => e.StockId).HasName("PK__NanaStoc__2C83A9E2563F7EA5");

            entity.ToTable("NanaStock");

            entity.Property(e => e.StockId)
                .ValueGeneratedNever()
                .HasColumnName("StockID");
            entity.Property(e => e.ItemDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
        });

        modelBuilder.Entity<RandomItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__RandomIt__727E83EB12311071");

            entity.ToTable("RandomItem");

            entity.Property(e => e.ItemId)
                .ValueGeneratedNever()
                .HasColumnName("ItemID");
            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.ItemDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

            entity.HasOne(d => d.Attribute).WithMany(p => p.RandomItems)
                .HasForeignKey(d => d.AttributeId)
                .HasConstraintName("FK__RandomIte__Attri__0D7A0286");

            entity.HasOne(d => d.ItemType).WithMany(p => p.RandomItems)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK__RandomIte__ItemT__0C85DE4D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserName).HasName("PK__Users__C9F28457A65BA43D");

            entity.Property(e => e.UserName)
                .HasMaxLength(128)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
