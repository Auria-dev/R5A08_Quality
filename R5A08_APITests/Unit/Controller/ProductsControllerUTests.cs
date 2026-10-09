using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using R508_Revisions.Controller;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Mapping;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions.Controller.Tests
{
    [TestClass]
    public class ProductsControllerUTests
    {
        private IMapper _mapper = null!;

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.ShouldMapMethod = _ => false;
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task GetById_ExistingId_ReturnsOkResult()
        {
            // arrange
            var prod = new Product
            {
                idProduit = 1,
                nomProduit = "Test Product",
                description = "Description",
                nomPhoto = "photo.jpg",
                uriPhoto = "/photo.jpg",
                idTypeProduitNavigation = new ProductType { nomTypeProduit = "Type A" },
                idMarqueNavigation = new Brand { nomMarque = "Brand B" },
                stockReel = 10,
                stockMin = 2
            };

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetByIdWithDetailsAsync(1)).ReturnsAsync(prod);

            var productsController = new ProductsController(mockRepository.Object, _mapper);

            // act
            var actionResult = await productsController.GetById(1);

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)actionResult.Result!;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(ProductDetailDto));
        }

        [TestMethod]
        public async Task GetById_UnknownId_ReturnsNotFoundResult()
        {
            // arrange
            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetByIdWithDetailsAsync(-1)).ReturnsAsync((Product?)null);

            var productsController = new ProductsController(mockRepository.Object, _mapper);

            // act
            var actionResult = await productsController.GetById(-1);

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            var notFoundResult = (NotFoundResult)actionResult.Result!;
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task Create_ValidProductDto_ReturnsCreatedAtAction()
        {
            // arrange
            var createDto = new ProductCreateDto
            {
                Name = "Test Product",
                Description = "Description",
                PhotoName = "photo.jpg",
                PhotoUri = "/photo.jpg",
                ProductTypeId = 1,
                BrandId = 1,
                CurrentStock = 10,
                MinStock = 2,
                MaxStock = 50
            };

            var createdEntity = new Product
            {
                idProduit = 1,
                nomProduit = createDto.Name,
                description = createDto.Description,
                nomPhoto = createDto.PhotoName,
                uriPhoto = createDto.PhotoUri,
                idTypeProduit = createDto.ProductTypeId,
                idMarque = createDto.BrandId,
                stockReel = createDto.CurrentStock,
                stockMin = createDto.MinStock,
                stockMax = createDto.MaxStock,
                idTypeProduitNavigation = new ProductType { nomTypeProduit = "Type A" },
                idMarqueNavigation = new Brand { nomMarque = "Brand B" }
            };

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).Callback<Product>(p => p.idProduit = 1).Returns(Task.CompletedTask);
            mockRepository.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(createdEntity);

            var productsController = new ProductsController(mockRepository.Object, _mapper);

            // act
            var actionResult = await productsController.Create(createDto);

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var createdResult = (CreatedAtActionResult)actionResult.Result!;
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.IsInstanceOfType(createdResult.Value, typeof(ProductDto));
            mockRepository.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [TestMethod]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            // arrange
            var dto = new ProductUpdateDto
            {
                ProductId = 1,
                Name = "Test Product"
            };

            var mockRepository = new Mock<IProductRepository>();
            var productsController = new ProductsController(mockRepository.Object, _mapper);

            // act
            var actionResult = await productsController.Update(2, dto);

            // assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
            var badRequestResult = (BadRequestResult)actionResult;
            Assert.AreEqual(400, badRequestResult.StatusCode);
            mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [TestMethod]
        public async Task Delete_ExistingId_ReturnsNoContent()
        {
            // arrange
            var prod = new Product
            {
                idProduit = 1,
                nomProduit = "Test Product",
                description = "Description",
                nomPhoto = "photo.jpg",
                uriPhoto = "/photo.jpg",
                idTypeProduitNavigation = new ProductType { nomTypeProduit = "Type A" },
                idMarqueNavigation = new Brand { nomMarque = "Brand B" },
                stockReel = 10,
                stockMin = 2
            };

            var mockRepository = new Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(prod);
            mockRepository.Setup(x => x.DeleteAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            var productsController = new ProductsController(mockRepository.Object, _mapper);

            // act
            var actionResult = await productsController.Delete(1);

            // assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            var noContentResult = (NoContentResult)actionResult;
            Assert.AreEqual(204, noContentResult.StatusCode);
            mockRepository.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}

