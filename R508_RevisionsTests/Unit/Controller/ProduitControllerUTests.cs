using Microsoft.AspNetCore.Mvc;
using Moq;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository;

namespace R508_Revisions.Controller.Tests
{
    [TestClass()]
    public class ProduitControllerUTests
    {
        [TestMethod()]
        public void GetProduit_ExistingId_ReturnsOkResult()
        {
            // arrange
            Produit prod = new Produit
            {
                idProduit = 1,
                nomProduit = "Produit test",
                description = "Description du produit test",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = 1,
                idMarque = 1,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            Mock<IProduitRepository> mockRepository = new Mock<IProduitRepository>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(prod);
            ProduitController produitController = new ProduitController(mockRepository.Object);

            // act
            ActionResult<Produit> actionResult = produitController.GetProduitById(prod.idProduit).Result;

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            OkObjectResult okResult = (OkObjectResult)actionResult.Result;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod()]
        public void GetProduit_UnknownId_ReturnsNotFoundResult()
        {
            // arrange
            Mock<IProduitRepository> mockRepository = new Mock<IProduitRepository>();
            ProduitController produitController = new ProduitController(mockRepository.Object);

            // act
            var actionResult = produitController.GetProduitById(-1).Result;

            // assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
            NotFoundResult notFoundResult = (NotFoundResult)actionResult.Result;
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod()]
        public void PostProduit_ValidProduct_ReturnsCreatedAtAction()
        {
            // arrange
            Produit prod = new Produit
            {
                idProduit = 1,
                nomProduit = "Produit test",
                description = "Description du produit test",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = 1,
                idMarque = 1,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            Mock<IProduitRepository> mockRepository = new Mock<IProduitRepository>();
            mockRepository.Setup(r => r.AddAsync(It.IsAny<Produit>())).Returns(Task.CompletedTask);
            ProduitController produitController = new ProduitController(mockRepository.Object);

            // act
            ActionResult<Produit> actionResult = produitController.PostProduit(prod).Result;

            // assert
            Assert.IsInstanceOfType(actionResult.Result,typeof(CreatedAtActionResult));
            CreatedAtActionResult createdResult = (CreatedAtActionResult)actionResult.Result;
            Assert.AreEqual(201, createdResult.StatusCode);
            mockRepository.Verify(r => r.AddAsync(It.Is<Produit>(p => p == prod)), Times.Once);
        }

        [TestMethod()]
        public void PutProduit_IdMismatch_ReturnsBadRequest()
        {
            // arrange
            Produit prod = new Produit
            {
                idProduit = 1,
                nomProduit = "Produit test",
                description = "Description du produit test",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = 1,
                idMarque = 1,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            Mock<IProduitRepository> mockRepository = new Mock<IProduitRepository>();
            ProduitController produitController = new ProduitController(mockRepository.Object);

            // act
            IActionResult actionResult = produitController.PutProduit(2, prod).Result;

            // assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
            BadRequestResult badRequestResult = (BadRequestResult)actionResult;
            Assert.AreEqual(400, badRequestResult.StatusCode);
            mockRepository.Verify(r => r.SaveAsync(),Times.Never);
        }

        [TestMethod()]
        public void DeleteProduit_ExistingId_ReturnsNoContent()
        {
            // arrange
            Produit prod = new Produit
            {
                idProduit = 1,
                nomProduit = "Produit test",
                description = "Description du produit test",
                nomPhoto = "test.jpg",
                uriPhoto = "/images/test.jpg",
                idTypeProduit = 1,
                idMarque = 1,
                stockReel = 10,
                stockMin = 2,
                stockMax = 50
            };

            Mock<IProduitRepository> mockRepository = new Mock<IProduitRepository>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(prod);
            ProduitController produitController = new ProduitController(mockRepository.Object);

            // act
            var actionResult = produitController.DeleteUtilisateur(1).Result;

            // assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            NoContentResult noContentResult = (NoContentResult)actionResult;
            Assert.AreEqual(204, noContentResult.StatusCode);
        }
    }
}