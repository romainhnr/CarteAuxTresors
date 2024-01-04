using CarteAuxTresors.Api.Models;

namespace CarteAuxTresors.Api.Services
{
    /// <summary>
    /// Carte Aux Tresors Service Interface
    /// </summary>
    public interface ICarteAuxTresorsService
    {
        /// <summary>
        /// Traiter le fichier.
        /// </summary>
        /// <param name="fileRequest">The file request.</param>
        List<string> TraiterFichier(FileRequest fileRequest);
    }

}
