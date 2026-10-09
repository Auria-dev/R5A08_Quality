using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using R508_Revisions.Model.Mapping;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository.Interface;
using R508_Revisions.Model.Repository.Implementation;

namespace R508_Revisions.Controller.Tests
{
    [TestClass]
    public class ProductsControllerIntegrationTests
    {
        private ProduitsDBContext _context = null!;
        private IProductRepository _repository = null!;
        private ProductsController _controller = null!;
        private IMapper _mapper = null!;

        private readonly string _connectionString = "Server=localhost;Port=5432;Database=DB_R5A08_Quality_TEST;Uid=postgres;Password=postgres;";

        [TestInitialize]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<ProduitsDBContext>()
                .UseNpgsql(_connectionString)
                .Options;

            _context = new ProduitsDBContext(options);

            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            _repository = new ProductRepository(_context);

            var config = new MapperConfiguration(cfg => {
                cfg.ShouldMapMethod = _ => false;
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
            _controller = new ProductsController(_repository, _mapper);
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        private async Task<(Brand brand, ProductType productType)> CreateDependenciesAsync()
        {
            var brand = new Brand { nomMarque = "Test Brand" };
            var productType = new ProductType { nomTypeProduit = "Test Type" };

            _context.Brands.Add(brand);
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            return (brand, productType);
        }

        private async Task<Product> CreateProductAsync()
        {
            var (brand, productType) = await CreateDependenciesAsync();

            var product = new Product
            {
                nomProduit = "Test Product",
                description = "Test product description",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = productType.idTypeProduit,
                idMarque = brand.idMarque,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        [TestMethod]
        public async Task Create_ValidProduct_ReturnsCreatedAndCanBeRead()
        {
            // Arrange
            var (brand, productType) = await CreateDependenciesAsync();

            var createDto = new ProductCreateDto
            {
                Name = "Integration Product",
                Description = "Integration description",
                PhotoName = "test.jpg",
                PhotoUri = "/images/test.jpg",
                ProductTypeId = productType.idTypeProduit,
                BrandId = brand.idMarque,
                CurrentStock = 10,
                MinStock = 2,
                MaxStock = 50
            };

            // Act
            var actionResult = await _controller.Create(createDto);

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var createdResult = (CreatedAtActionResult)actionResult.Result!;
            Assert.AreEqual(201, createdResult.StatusCode);
            var createdDto = (ProductDto)createdResult.Value!;
            Assert.IsTrue(createdDto.Id > 0);
            int productId = createdDto.Id;

            // Act
            var getResult = await _controller.GetById(productId);

            // Assert
            Assert.IsInstanceOfType(getResult.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)getResult.Result!;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsNotNull(okResult.Value);

            var returnedProduct = (ProductDetailDto)okResult.Value;
            Assert.AreEqual(productId, returnedProduct.Id);
            Assert.AreEqual("Integration Product", returnedProduct.Name);
            Assert.AreEqual(brand.nomMarque, returnedProduct.Brand);
            Assert.AreEqual(productType.nomTypeProduit, returnedProduct.ProductType);
        }

        [TestMethod]
        public async Task Update_ExistingProduct_ReturnsNoContentAndUpdatesDatabase()
        {
            // Arrange
            var product = await CreateProductAsync();
            int productId = product.idProduit;

            var updateDto = new ProductUpdateDto
            {
                ProductId = productId,
                Name = "Modified Product",
                Description = product.description,
                PhotoName = product.nomPhoto,
                PhotoUri = product.uriPhoto,
                ProductTypeId = product.idTypeProduit,
                BrandId = product.idMarque,
                CurrentStock = 25,
                MinStock = product.stockMin,
                MaxStock = product.stockMax
            };

            // Act
            var actionResult = await _controller.Update(productId, updateDto);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            var noContentResult = (NoContentResult)actionResult;
            Assert.AreEqual(204, noContentResult.StatusCode);
            _context.ChangeTracker.Clear();

            // Act
            var getResult = await _controller.GetById(productId);

            // Assert
            Assert.IsInstanceOfType(getResult.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)getResult.Result!;
            Assert.AreEqual(200, okResult.StatusCode);

            var modifiedProduct = (ProductDetailDto)okResult.Value!;
            Assert.AreEqual(productId, modifiedProduct.Id);
            Assert.AreEqual("Modified Product", modifiedProduct.Name);
            Assert.AreEqual(25, modifiedProduct.Stock);
        }

        [TestMethod]
        public async Task Delete_ExistingProduct_ReturnsNoContentAndThenNotFound()
        {
            // Arrange
            var product = await CreateProductAsync();
            int productId = product.idProduit;

            // Act
            var actionResult = await _controller.Delete(productId);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            var noContentResult = (NoContentResult)actionResult;
            Assert.AreEqual(204, noContentResult.StatusCode);
            _context.ChangeTracker.Clear();

            // Act
            var getResult = await _controller.GetById(productId);

            // Assert
            Assert.IsInstanceOfType(getResult.Result, typeof(NotFoundResult));
            var notFoundResult = (NotFoundResult)getResult.Result!;
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task Create_InvalidForeignKey_ThrowsDbUpdateException()
        {
            // Arrange
            var createDto = new ProductCreateDto
            {
                Name = "Invalid FK Product",
                Description = "Integrity constraint test",
                PhotoName = "test.jpg",
                PhotoUri = "/images/test.jpg",
                ProductTypeId = 0,
                BrandId = 999999,
                CurrentStock = 10,
                MinStock = 2,
                MaxStock = 50
            };

            var productType = new ProductType { nomTypeProduit = "Valid Type" };
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();

            createDto.ProductTypeId = productType.idTypeProduit;

            // Act + Assert
            await Assert.ThrowsExceptionAsync<DbUpdateException>(async () =>
            {
                await _controller.Create(createDto);
            });
        }
    }
}
