using CarteAuxTresors.Api.Controllers;
using CarteAuxTresors.Api.Models;
using CarteAuxTresors.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace CarteAuxTresors.Tests
{
    /// <summary>
    /// CarteAuxTresorsControllerTests
    /// </summary>
    public class CarteAuxTresorsControllerTests
    {
        /// <summary>
        /// The sub logger
        /// </summary>
        private readonly ILogger<CarteAuxTresorsController> _subLogger;
        /// <summary>
        /// The sub carte aux tresors service
        /// </summary>
        private readonly ICarteAuxTresorsService _subCarteAuxTresorsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarteAuxTresorsControllerTests" /> class.
        /// </summary>
        public CarteAuxTresorsControllerTests()
        {
            _subLogger = Substitute.For<ILogger<CarteAuxTresorsController>>();
            _subCarteAuxTresorsService = Substitute.For<ICarteAuxTresorsService>();
        }

        /// <summary>
        /// Creates the controller.
        /// </summary>
        /// <returns></returns>
        private CarteAuxTresorsController CreateController()
        {
            return new CarteAuxTresorsController(_subLogger, _subCarteAuxTresorsService);
        }


        /// <summary>
        /// Executes the chasse aux tresors should return file stream result when valid file request.
        /// </summary>
        [Fact]
        public void ExecuteChasseAuxTresors_ShouldReturnFileStreamResult_WhenValidFileRequest()
        {
            // Arrange
            CarteAuxTresorsController controller = CreateController();

            string fileContent = "# C - 3 - 4\r\n# M - 1 - 0\r\nM - 2 - 1\r\nT - 0 - 3 - 2\r\nT - 1 - 3 - 3\r\nA - Lara - 1 - 1 - S - AADADAGGA\r\nA - John - 2 - 2 - S - AADAAGGA";

            var fileRequest = new FileRequest
            {
                File = CreateMockedFormFile(fileContent)
            };

            var outputLines = new List<string> { "C - 3 - 4", "M - 1 - 0" };
            _subCarteAuxTresorsService.TraiterFichier(fileRequest).Returns(outputLines);

            // Act
            var result = controller.ExecuteChasseAuxTresors(fileRequest);

            // Assert
            result.Should().BeOfType<FileStreamResult>();
            result.ContentType.Should().Be("application/octet-stream");
            _subCarteAuxTresorsService.Received(1).TraiterFichier(fileRequest); // Vérifie que la méthode TraiterFichier a été appelée exactement une fois
        }

        /// <summary>
        /// Executes the chasse aux tresors should throw bad HTTP request exception when io exception occurs.
        /// </summary>
        [Fact]
        public void ExecuteChasseAuxTresors_ShouldThrowBadHttpRequestException_WhenIOExceptionOccurs()
        {
            // Arrange
            CarteAuxTresorsController controller = CreateController();

            string fileContent = "aaa";

            _subCarteAuxTresorsService.TraiterFichier(Arg.Any<FileRequest>()).Throws(new IOException("Erreur de lecture du fichier"));

            var fileRequest = new FileRequest
            {
                File = CreateMockedFormFile(fileContent),
            };

            // Act & Assert
            controller.Invoking(c => c.ExecuteChasseAuxTresors(fileRequest))
                .Should().Throw<BadHttpRequestException>()
                .WithMessage("Erreur de lecture du fichier");
        }


        /// <summary>
        /// Executes the chasse aux tresors should throw invalid data exception when invalid data exception occurs.
        /// </summary>
        [Fact]
        public void ExecuteChasseAuxTresors_ShouldThrowInvalidDataException_WhenInvalidDataExceptionOccurs()
        {
            // Arrange
            CarteAuxTresorsController controller = CreateController();

            string fileContent = "aaa";

            _subCarteAuxTresorsService.TraiterFichier(Arg.Any<FileRequest>()).Throws(new InvalidDataException("Format de fichier invalide"));

            var fileRequest = new FileRequest
            {
                File = CreateMockedFormFile(fileContent)
            };

            // Act & Assert
            controller.Invoking(c => c.ExecuteChasseAuxTresors(fileRequest))
                .Should().Throw<InvalidDataException>()
                .WithMessage("Format de fichier invalide");
        }

        /// <summary>
        /// Creates the mocked form file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private static IFormFile CreateMockedFormFile(string content)
        {
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "file", "filename.txt");
        }

    }

}