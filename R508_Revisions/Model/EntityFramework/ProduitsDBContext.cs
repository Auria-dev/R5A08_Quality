using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace R508_Revisions.Model.EntityFramework
{
    public partial class ProduitsDBContext : DbContext
    {
        public ProduitsDBContext()
        {

        }

        public ProduitsDBContext(DbContextOptions<ProduitsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Produit> Produits { get; set; } = null!;
        public virtual DbSet<TypeProduit> TypeProduits { get; set; } = null!;
        public virtual DbSet<Marque> Marques { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string? connectionString = configuration.GetConnectionString("DbCoreConnectionString");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasOne(d => d.idMarqueNavigation)
                    .WithMany(p => p.Produits)
                    .HasForeignKey(d => d.idMarque)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_produit_marque");

                entity.HasOne(d => d.idTypeProduitNavigation)
                    .WithMany(p => p.Produits)
                    .HasForeignKey(d => d.idTypeProduit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_produit_typeproduit");
            });

            modelBuilder.Entity<TypeProduit>(entity =>
            {
            });

            modelBuilder.Entity<Marque>(entity =>
            {
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
