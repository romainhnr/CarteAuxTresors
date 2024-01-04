using CarteAuxTresors.Api.Models;

namespace CarteAuxTresors.Api.Services
{
    /// <summary>
    /// Carte Aux Tresors Service
    /// </summary>
    /// <seealso cref="ICarteAuxTresorsService" />
    public class CarteAuxTresorsService : ICarteAuxTresorsService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CarteAuxTresorsService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarteAuxTresorsService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CarteAuxTresorsService(ILogger<CarteAuxTresorsService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Traiter le fichier.
        /// </summary>
        /// <param name="fileRequest">The file request.</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">Le fichier ne contient pas d'informations de carte.</exception>
        public List<string> TraiterFichier(FileRequest fileRequest)
        {
            List<string> inputlines = new();
            List<string> outputLines = new();

            using (var reader = new StreamReader(fileRequest.File.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    inputlines.Add(reader.ReadLine()?.Replace("#", "") ?? "");
            }

            Map? map = null;

            foreach (string line in inputlines)
            {
                string[] parts = line.Split("-");
                string type = parts[0].Trim();

                // Récupérer le type (C, M, T, A)
                switch (type)
                {
                    case "C":
                        map = new Map(int.Parse(parts[1].Trim()), int.Parse(parts[2].Trim()));
                        _logger.LogInformation("Ajout carte {w}x{h}", map.Width, map.Height);
                        break;
                    case "M":
                        CheckIfMapExist(map);
                        var newMoutain = new Mountain(int.Parse(parts[1].Trim()), int.Parse(parts[2].Trim()));
                        map?.Add(newMoutain);
                        _logger.LogInformation("Ajout montagne à {x} {y}", newMoutain.X, newMoutain.Y);
                        break;
                    case "T":
                        CheckIfMapExist(map);
                        var newTreasure = new Treasure(int.Parse(parts[1].Trim()), int.Parse(parts[2].Trim()), int.Parse(parts[3].Trim()));
                        map?.Add(newTreasure);
                        _logger.LogInformation("Ajout trésor à {x} {y}", newTreasure.X, newTreasure.Y);
                        break;
                    case "A":
                        CheckIfMapExist(map);
                        var newAdventurer = new Adventurer(parts[1].Trim(), int.Parse(parts[2].Trim()), int.Parse(parts[3].Trim()), parts[4].Trim(), parts[5].Trim());
                        map?.Add(newAdventurer);
                        _logger.LogInformation("Ajout aventurier à {x} {y}", newAdventurer.X, newAdventurer.Y);
                        break;
                    default:
                        _logger.LogError("Type inconnu : ", type);
                        break;
                }
            }

            if (map == null)
            {
                _logger.LogError("Le fichier ne contient pas d'informations de carte.");
                throw new InvalidDataException("Le fichier ne contient pas d'informations de carte.");
            }

            // Simule les mouvements des aventuriers
            map.Simulate();

            // Ajoute la carte en ligne de sortie
            outputLines.Add(map.ToString());

            // Génère les lignes de sortie des éléments
            foreach (MapElement element in map.Elements)
            {
                if (element is Treasure elementTreasure && elementTreasure.Count < 1)
                {
                    _logger.LogInformation("Trésor entièrement récolté à {x} {y}", elementTreasure.X, elementTreasure.Y);
                    continue;
                }
                outputLines.Add(element.ToString());
            }

            // Retourne les lignes de sorties
            return outputLines;
        }

        /// <summary>
        /// Checks if the map exist.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <exception cref="InvalidDataException">Le fichier ne commence pas par une carte.</exception>
        private void CheckIfMapExist(Map? map)
        {
            if (map == null)
            {
                _logger.LogError("Le fichier ne commence pas par une carte.");
                throw new InvalidDataException("Le fichier ne commence pas par une carte.");
            }
        }
    }
}
