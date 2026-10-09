using Microsoft.EntityFrameworkCore;

namespace R508_Revisions.Model.EntityFramework
{
    public partial class ProduitsDBContext : DbContext
    {
        public ProduitsDBContext() { }
        public ProduitsDBContext(DbContextOptions<ProduitsDBContext> options) : base(options) { }

        public virtual DbSet<Product> Produits { get; set; } = null!;
        public virtual DbSet<ProductType> TypeProduits { get; set; } = null!;
        public virtual DbSet<Brand> Marques { get; set; } = null!;

        public virtual DbSet<Product> Products { get => Produits; set => Produits = value; }
        public virtual DbSet<ProductType> ProductTypes { get => TypeProduits; set => TypeProduits = value; }
        public virtual DbSet<Brand> Brands { get => Marques; set => Marques = value; }

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

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.idMarqueNavigation)
                    .WithMany(p => p.Products)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_produit_marque");

                entity.HasOne(d => d.idTypeProduitNavigation)
                    .WithMany(p => p.Products)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_produit_typeproduit");
            });

            modelBuilder.Entity<ProductType>(entity => {});
            modelBuilder.Entity<Brand>(entity => {});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
