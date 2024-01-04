using CarteAuxTresors.Api.Models;
using CarteAuxTresors.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CarteAuxTresors.Api.Controllers
{
    /// <summary>
    /// Carte aux trésors Controller
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/carte-aux-tresors")]
    [Produces("application/json")]
    public class CarteAuxTresorsController : Controller
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CarteAuxTresorsController> _logger;

        /// <summary>
        /// The carte aux tresors service
        /// </summary>
        private readonly ICarteAuxTresorsService _carteAuxTresorsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarteAuxTresorsController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="carteAuxTresorsService">The carte aux tresors service.</param>
        public CarteAuxTresorsController(ILogger<CarteAuxTresorsController> logger, ICarteAuxTresorsService carteAuxTresorsService)
        {
            _logger = logger;
            _carteAuxTresorsService = carteAuxTresorsService;
        }

        /// <summary>
        /// Executes the chasse aux tresors.
        /// </summary>
        /// <param name="fileRequest">The file request.</param>
        /// <returns></returns>
        /// <exception cref="Microsoft.AspNetCore.Server.IIS.BadHttpRequestException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        [Route("execute-chasse-aux-tresors")]
        [HttpPost]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        public FileResult ExecuteChasseAuxTresors([FromForm] FileRequest fileRequest)
        {
            try
            {
                var outputLines = _carteAuxTresorsService.TraiterFichier(fileRequest);

                // Conversion des lignes de sorties en stream
                byte[] dataAsBytes = outputLines.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s + Environment.NewLine)).ToArray();
                MemoryStream stream = new(dataAsBytes);

                var fileName = $"Resultat_Chasse_Aux_Tresors_{Path.GetFileNameWithoutExtension(fileRequest.File.FileName)}_{DateTimeOffset.UtcNow.ToLocalTime():dd_MM_yyyy-HH:mm:ss}.txt";

                // Retourne un fichier à partir du stream
                return File(stream, "application/octet-stream", fileName);

            }
            catch (IOException e)
            {
                _logger.LogError("Le fichier ne peut pas être lu", e.Message);
                throw new BadHttpRequestException(e.Message);
            }
            catch (InvalidDataException e)
            {
                _logger.LogError("Le format de fichier est invalide", e.Message);
                throw new InvalidDataException(e.Message);
            }
        }


    }
}