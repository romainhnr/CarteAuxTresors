using CarteAuxTresors.Api.Models;
using CarteAuxTresors.Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text;
using Xunit;

namespace CarteAuxTresors.Tests
{
    /// <summary>
    /// CarteAuxTresorsServiceTests
    /// </summary>
    public class CarteAuxTresorsServiceTests
    {
        /// <summary>
        /// The sub logger
        /// </summary>
        private readonly ILogger<CarteAuxTresorsService> _subLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarteAuxTresorsServiceTests"/> class.
        /// </summary>
        public CarteAuxTresorsServiceTests()
        {
            _subLogger = Substitute.For<ILogger<CarteAuxTresorsService>>();
        }

        /// <summary>
        /// Creates the service.
        /// </summary>
        /// <returns></returns>
        private CarteAuxTresorsService CreateService()
        {
            return new CarteAuxTresorsService(_subLogger);
        }

        /// <summary>
        /// Traiter the fichier valide genere sortie.
        /// </summary>
        [Fact]
        public void TraiterFichier_FichierValide_GenereSortie()
        {
            // Arrange
            CarteAuxTresorsService service = CreateService();

            string fileContent = "# C - 3 - 4\r\n# M - 1 - 0\r\nM - 2 - 1\r\nT - 0 - 3 - 2\r\nT - 1 - 3 - 3\r\nA - Lara - 1 - 1 - S - AADADAGGA\r\nA - John - 2 - 2 - S - AADAAGGA";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            var formFile = new FormFile(stream, 0, stream.Length, "file", "filename.txt");

            var fileRequest = new FileRequest
            {
                File = formFile,
            };

            // Act
            var result = service.TraiterFichier(fileRequest);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(6);
            _ = result.ElementAtOrDefault(0).Should().Be("C - 3 - 4");
            _ = result.ElementAtOrDefault(1).Should().Be("M - 1 - 0");
            _ = result.ElementAtOrDefault(2).Should().Be("M - 2 - 1");
            _ = result.ElementAtOrDefault(3).Should().Be("T - 1 - 3 - 1");
            _ = result.ElementAtOrDefault(4).Should().Be("A - Lara - 0 - 3 - S - 3");
            _ = result.ElementAtOrDefault(5).Should().Be("A - John - 2 - 3 - E - 1");
        }

        /// <summary>
        /// Traiter fichier vide lance exception.
        /// </summary>
        [Fact]
        public void TraiterFichier_FichierVide_LanceException()
        {
            // Arrange
            CarteAuxTresorsService service = CreateService();

            var formFile = Substitute.For<IFormFile>();

            var fileRequest = new FileRequest
            {
                File = formFile
            };

            // Act
            Action act = () => service.TraiterFichier(fileRequest);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Traiter fichier invalide sans carte lance exception.
        /// </summary>
        [Fact]
        public void TraiterFichier_FichierInvalideSansCarte_LanceException()
        {
            // Arrange
            CarteAuxTresorsService service = CreateService();

            string fileContent = "# M - 1 - 0\r\nM - 2 - 1\r\nT - 0 - 3 - 2\r\nT - 1 - 3 - 3\r\nA - Lara - 1 - 1 - S - AADADAGGA\r\nA - John - 2 - 2 - S - AADAAGGA";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            var formFile = new FormFile(stream, 0, stream.Length, "file", "filename.txt");

            var fileRequest = new FileRequest
            {
                File = formFile,
            };

            // Act
            Action act = () => service.TraiterFichier(fileRequest);

            // Assert
            act.Should().Throw<InvalidDataException>().WithMessage("Le fichier ne commence pas par une carte.");
        }
    }

}