using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository;
using R508_Revisions.Model.Repository.Implementation;

namespace R508_Revisions.Controller.Tests
{
    [TestClass]
    public class ProduitsControllerIntegrationTests
    {
        private ProduitsDBContext _context = null!;
        private IProduitRepository _repository = null!;
        private ProduitController _controller = null!;

        private readonly string _connectionString = "Server=localhost;Port=5432;Database=DB_R508_TP1REV_TEST;Uid=postgres;Password=postgres;";

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<ProduitsDBContext>()
                .UseNpgsql(_connectionString)
                .Options;

            _context = new ProduitsDBContext(options);

            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            _repository = new ProduitRepository(_context);
            _controller = new ProduitController(_repository);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        private async Task<(Marque marque, TypeProduit typeProduit)> CreateDependenciesAsync()
        {
            var marque = new Marque { nomMarque = "Marque test" };
            var typeProduit = new TypeProduit { nomTypeProduit = "Type test" };

            _context.Marques.Add(marque);
            _context.TypeProduits.Add(typeProduit);
            await _context.SaveChangesAsync();

            return (marque, typeProduit);
        }

        private async Task<Produit> CreateProduitAsync()
        {
            var (marque, typeProduit) = await CreateDependenciesAsync();

            var produit = new Produit
            {
                nomProduit = "Produit test",
                description = "Description du produit test",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = typeProduit.idTypeProduit,
                idMarque = marque.idMarque,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return produit;
        }

        [TestMethod]
        public async Task PostProduit_ValidProduct_ReturnsCreatedAndCanBeRead()
        {
            // Arrange
            var (marque, typeProduit) = await CreateDependenciesAsync();

            var produit = new Produit
            {
                nomProduit = "Produit intégration",
                description = "Description intégration",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = typeProduit.idTypeProduit,
                idMarque = marque.idMarque,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            // Act
            var actionResult = await _controller.PostProduit(produit);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var createdResult = (CreatedAtActionResult)actionResult.Result!;
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.IsTrue(produit.idProduit > 0);
            int idProduit = produit.idProduit;

            // Act
            var getResult = await _controller.GetProduitById(idProduit);

            // Assert
            Assert.IsInstanceOfType(getResult.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)getResult.Result!;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);

            var produitRetourne = (ProduitDetailDto)okResult.Value;
            Assert.AreEqual(idProduit, produitRetourne.Id);
            Assert.AreEqual("Produit intégration", produitRetourne.Nom);
            Assert.AreEqual(marque.nomMarque, produitRetourne.Marque);
            Assert.AreEqual(typeProduit.nomTypeProduit, produitRetourne.Type);
        }

        [TestMethod]
        public async Task PutProduit_ExistingProduct_ReturnsNoContentAndUpdatesDatabase()
        {
            // Arrange
            var produit = await CreateProduitAsync();
            int idProduit = produit.idProduit;

            var updateDto = new ProduitUpdateDto
            {
                IdProduit = idProduit,
                NomProduit = "Produit modifié",
                Description = produit.description,
                NomPhoto = produit.nomPhoto,
                UriPhoto = produit.uriPhoto,
                IdTypeProduit = produit.idTypeProduit,
                IdMarque = produit.idMarque,
                StockReel = 25,
                StockMin = produit.stockMin,
                StockMax = produit.stockMax
            };

            // Act
            var actionResult = await _controller.PutProduit(idProduit, updateDto);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            var noContentResult = (NoContentResult)actionResult;
            Assert.AreEqual(204, noContentResult.StatusCode);
            _context.ChangeTracker.Clear();

            // Act
            var getResult = await _controller.GetProduitById(idProduit);

            // Assert
            Assert.IsInstanceOfType(getResult.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)getResult.Result!;
            Assert.AreEqual(200, okResult.StatusCode);

            var produitModifie = (ProduitDetailDto)okResult.Value!;
            Assert.AreEqual(idProduit, produitModifie.Id);
            Assert.AreEqual("Produit modifié", produitModifie.Nom);
            Assert.AreEqual(25, produitModifie.Stock);
        }

        [TestMethod]
        public async Task DeleteProduit_ExistingProduct_ReturnsNoContentAndThenNotFound()
        {
            // Arrange
            var produit = await CreateProduitAsync();
            int idProduit = produit.idProduit;

            // Act
            var actionResult = await _controller.DeleteProduit(idProduit);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            var noContentResult = (NoContentResult)actionResult;
            Assert.AreEqual(204, noContentResult.StatusCode);
            _context.ChangeTracker.Clear();

            // Act
            var getResult = await _controller.GetProduitById(idProduit);

            // Assert
            Assert.IsInstanceOfType(getResult.Result, typeof(NotFoundResult));
            var notFoundResult = (NotFoundResult)getResult.Result!;
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task PostProduit_InvalidForeignKey_ThrowsDbUpdateException()
        {
            // Arrange
            var produit = new Produit
            {
                nomProduit = "Produit FK invalide",
                description = "Test contrainte intégrité",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = 0,
                idMarque = 999999,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            var typeProduit = new TypeProduit { nomTypeProduit = "Type valide" };
            _context.TypeProduits.Add(typeProduit);
            await _context.SaveChangesAsync();

            produit.idTypeProduit = typeProduit.idTypeProduit;

            // Act + Assert
            await Assert.ThrowsExceptionAsync<DbUpdateException>(async () =>
            {
                await _controller.PostProduit(produit);
            });
        }
    }
}