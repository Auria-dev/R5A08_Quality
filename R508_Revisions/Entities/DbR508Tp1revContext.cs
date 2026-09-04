using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace R508_Revisions.Entities;

public partial class DbR508Tp1revContext : DbContext
{
    public DbR508Tp1revContext()
    {
    }

    public DbR508Tp1revContext(DbContextOptions<DbR508Tp1revContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TEMarque> TEMarques { get; set; }

    public virtual DbSet<TEProduit> TEProduits { get; set; }

    public virtual DbSet<TETypeproduit> TETypeproduits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost,5432;Database=DB_R508_TP1REV;UserId=postgres;Password=postgres;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TEMarque>(entity =>
        {
            entity.HasKey(e => e.MrqId);

            entity.ToTable("t_e_marque");

            entity.Property(e => e.MrqId).HasColumnName("mrq_id");
            entity.Property(e => e.MrqNom).HasColumnName("mrq_nom");
        });

        modelBuilder.Entity<TEProduit>(entity =>
        {
            entity.HasKey(e => e.PrdProduit);

            entity.ToTable("t_e_produit");

            entity.HasIndex(e => e.PrdIdmarque, "IX_t_e_produit_prd_idmarque");

            entity.HasIndex(e => e.PrdIdtype, "IX_t_e_produit_prd_idtype");

            entity.Property(e => e.PrdProduit).HasColumnName("prd_produit");
            entity.Property(e => e.PrdDescription)
                .HasMaxLength(100)
                .HasColumnName("prd_description");
            entity.Property(e => e.PrdIdmarque).HasColumnName("prd_idmarque");
            entity.Property(e => e.PrdIdtype).HasColumnName("prd_idtype");
            entity.Property(e => e.PrdNom)
                .HasMaxLength(100)
                .HasColumnName("prd_nom");
            entity.Property(e => e.PrdNomphoto)
                .HasMaxLength(100)
                .HasColumnName("prd_nomphoto");
            entity.Property(e => e.PrdStockmax).HasColumnName("prd_stockmax");
            entity.Property(e => e.PrdStockmin).HasColumnName("prd_stockmin");
            entity.Property(e => e.PrdStockreel).HasColumnName("prd_stockreel");
            entity.Property(e => e.PrdUriphoto)
                .HasMaxLength(256)
                .HasColumnName("prd_uriphoto");

            entity.HasOne(d => d.PrdIdmarqueNavigation).WithMany(p => p.TEProduits)
                .HasForeignKey(d => d.PrdIdmarque)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produit_typeproduit");

            entity.HasOne(d => d.PrdIdtypeNavigation).WithMany(p => p.TEProduits)
                .HasForeignKey(d => d.PrdIdtype)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produit_marque");
        });

        modelBuilder.Entity<TETypeproduit>(entity =>
        {
            entity.HasKey(e => e.TppId);

            entity.ToTable("t_e_typeproduit");

            entity.Property(e => e.TppId).HasColumnName("tpp_id");
            entity.Property(e => e.TppNom).HasColumnName("tpp_nom");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
