// using System;
// using System.Collections.Generic;
// using HexMapEditor.Models;
// using Microsoft.EntityFrameworkCore;

// namespace HexMapEditor.Data;

// public partial class RosegoldrailroadContext : DbContext
// {
//     public RosegoldrailroadContext()
//     {
//     }

//     public RosegoldrailroadContext(DbContextOptions<RosegoldrailroadContext> options)
//         : base(options)
//     {
//     }

//     public virtual DbSet<Asset> Assets { get; set; }

//     public virtual DbSet<Models.Attribute> Attributes { get; set; }

//     public virtual DbSet<Grid> Grids { get; set; }

//     public virtual DbSet<ItemType> ItemTypes { get; set; }

//     public virtual DbSet<NanaStock> NanaStocks { get; set; }

//     public virtual DbSet<RandomItem> RandomItems { get; set; }

//     public virtual DbSet<User> Users { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         => optionsBuilder.UseSqlServer("Name=RGRContext");

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder.Entity<Asset>(entity =>
//         {
//             entity.HasKey(e => e.Filename).HasName("PK__Assets__589E6EEDDED1BF8F");

//             entity.Property(e => e.Filename)
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//             entity.Property(e => e.Name)
//                 .IsRequired()
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//         });

//         modelBuilder.Entity<Attribute>(entity =>
//         {
//             entity.HasKey(e => e.AttributeId).HasName("PK__Attribut__C189298AE51E6449");

//             entity.ToTable("Attribute");

//             entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
//             entity.Property(e => e.AttributeDescription)
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//             entity.Property(e => e.AttributeValue)
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//             entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

//             entity.HasOne(d => d.ItemType).WithMany(p => p.Attributes)
//                 .HasForeignKey(d => d.ItemTypeId)
//                 .HasConstraintName("FK__Attribute__ItemT__0F2D40CE");
//         });

//         modelBuilder.Entity<Grid>(entity =>
//         {
//             entity
//                 .HasNoKey()
//                 .ToTable("Grid");

//             entity.Property(e => e.Grid1)
//                 .HasMaxLength(8000)
//                 .IsUnicode(false)
//                 .HasColumnName("Grid");
//         });

//         modelBuilder.Entity<ItemType>(entity =>
//         {
//             entity.HasKey(e => e.ItemTypeId).HasName("PK__ItemType__F51540DBAFE013B9");

//             entity.ToTable("ItemType");

//             entity.Property(e => e.ItemTypeId)
//                 .ValueGeneratedNever()
//                 .HasColumnName("ItemTypeID");
//             entity.Property(e => e.ItemTypeValue)
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//         });

//         modelBuilder.Entity<NanaStock>(entity =>
//         {
//             entity.HasKey(e => e.StockId).HasName("PK__NanaStoc__2C83A9E27BC88374");

//             entity.ToTable("NanaStock");

//             entity.Property(e => e.StockId).HasColumnName("StockID");
//             entity.Property(e => e.ItemDescription)
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//             entity.Property(e => e.ItemId).HasColumnName("ItemID");
//         });

//         modelBuilder.Entity<RandomItem>(entity =>
//         {
//             entity.HasKey(e => e.ItemId).HasName("PK__RandomIt__727E83EB1B020123");

//             entity.ToTable("RandomItem");

//             entity.Property(e => e.ItemId).HasColumnName("ItemID");
//             entity.Property(e => e.ItemDescription)
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//             entity.Property(e => e.ItemName)
//                 .HasMaxLength(255)
//                 .IsUnicode(false);
//             entity.Property(e => e.ItemTypeId).HasColumnName("ItemTypeID");

//             entity.HasOne(d => d.ItemType).WithMany(p => p.RandomItems)
//                 .HasForeignKey(d => d.ItemTypeId)
//                 .HasConstraintName("FK__RandomIte__ItemT__1209AD79");
//         });

//         modelBuilder.Entity<User>(entity =>
//         {
//             entity.HasKey(e => e.UserName).HasName("PK__Users__C9F28457C0E9E1B0");

//             entity.Property(e => e.UserName)
//                 .HasMaxLength(128)
//                 .IsUnicode(false);
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }
