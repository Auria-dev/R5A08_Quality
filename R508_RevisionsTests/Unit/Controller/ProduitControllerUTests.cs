using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using R508_Revisions.Model.DTO;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository;
using AutoMapper;
using R508_Revisions.Model.Mapping;


namespace R508_Revisions.Controller.Tests
{
    [TestClass]
    public class ProduitControllerUTests
    {
        private IMapper mapper = null;

        [TestInitialize]
        public async Task Setup()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.ShouldMapMethod = _ => false;
                cfg.AddProfile<MappingProfile>();
            });

            mapper = config.CreateMapper();
        }

        [TestMethod]
        public async Task GetProduitById_ExistingId_ReturnsOkResult()
        {
            Assert.IsNotNull(typeof(IMapper).Assembly);
            Console.WriteLine(typeof(IMapper).Assembly.FullName);

            // arrange
            var prod = new Produit
            {
                idProduit = 1,
                nomProduit = "Produit test",
                description = "Description",
                nomPhoto = "photo.jpg",
                uriPhoto = "/photo.jpg",
                idTypeProduitNavigation = new TypeProduit { nomTypeProduit = "Type A" },
                idMarqueNavigation = new Marque { nomMarque = "Marque B" },
                stockReel = 10,
                stockMin = 2
            };

            var mockRepository = new Mock<IProduitRepository>();
            mockRepository.Setup(x => x.GetByIdWithDetailsAsync(1)).ReturnsAsync(prod);
            var produitController = new ProduitController(mockRepository.Object, mapper);

            // act
            var actionResult = await produitController.GetProduitById(1);

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)actionResult.Result!;
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsInstanceOfType(okResult.Value, typeof(ProduitDetailDto));
        }

        [TestMethod]
        public async Task GetProduitById_UnknownId_ReturnsNotFoundResult()
        {
            // arrange
            var mockRepository = new Mock<IProduitRepository>();
            mockRepository.Setup(x => x.GetByIdWithDetailsAsync(-1)).ReturnsAsync((Produit?)null);
            var produitController = new ProduitController(mockRepository.Object, mapper);

            // act
            var actionResult = await produitController.GetProduitById(-1);

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            var notFoundResult = (NotFoundResult)actionResult.Result!;
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public async Task PostProduit_ValidProduct_ReturnsCreatedAtAction()
        {
            // arrange
            var prod = new Produit
            {
                idProduit = 1,
                nomProduit = "Produit test",
                description = "Description",
                nomPhoto = "photo.jpg",
                uriPhoto = "/photo.jpg",
                idTypeProduitNavigation = new TypeProduit { nomTypeProduit = "Type A" },
                idMarqueNavigation = new Marque { nomMarque = "Marque B" },
                stockReel = 10,
                stockMin = 2
            };

            var mockRepository = new Mock<IProduitRepository>();
            mockRepository.Setup(r => r.AddAsync(It.IsAny<Produit>())).Returns(Task.CompletedTask);
            mockRepository.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(prod);

            var produitController = new ProduitController(mockRepository.Object, mapper);

            // act
            var actionResult = await produitController.PostProduit(prod);

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var createdResult = (CreatedAtActionResult)actionResult.Result!;
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.IsInstanceOfType(createdResult.Value, typeof(ProduitDto));
            mockRepository.Verify(r => r.AddAsync(It.Is<Produit>(p => p == prod)), Times.Once);
        }

        [TestMethod]
        public async Task PutProduit_IdMismatch_ReturnsBadRequest()
        {
            // arrange
            var dto = new ProduitUpdateDto
            {
                IdProduit = 1,
                NomProduit = "Produit test"
            };

            var mockRepository = new Mock<IProduitRepository>();
            var produitController = new ProduitController(mockRepository.Object, mapper);

            // act
            var actionResult = await produitController.PutProduit(2, dto);

            // assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
            var badRequestResult = (BadRequestResult)actionResult;
            Assert.AreEqual(400, badRequestResult.StatusCode);
            mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Produit>(), It.IsAny<Produit>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteProduit_ExistingId_ReturnsNoContent()
        {
            // arrange
            var prod = new Produit
            {
                idProduit = 1,
                nomProduit = "Produit test",
                description = "Description",
                nomPhoto = "photo.jpg",
                uriPhoto = "/photo.jpg",
                idTypeProduitNavigation = new TypeProduit { nomTypeProduit = "Type A" },
                idMarqueNavigation = new Marque { nomMarque = "Marque B" },
                stockReel = 10,
                stockMin = 2
            };

            var mockRepository = new Mock<IProduitRepository>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(prod);
            mockRepository.Setup(x => x.DeleteAsync(It.IsAny<Produit>())).Returns(Task.CompletedTask);

            var produitController = new ProduitController(mockRepository.Object, mapper);

            // act
            var actionResult = await produitController.DeleteProduit(1);

            // assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            var noContentResult = (NoContentResult)actionResult;
            Assert.AreEqual(204, noContentResult.StatusCode);
            mockRepository.Verify(x => x.DeleteAsync(It.IsAny<Produit>()), Times.Once);
        }
    }
}