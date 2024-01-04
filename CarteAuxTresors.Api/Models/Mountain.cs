namespace CarteAuxTresors.Api.Models
{
    /// <summary>
    /// Classe représentant l'objet montagne
    /// </summary>
    /// <seealso cref="CarteAuxTresors.Models.MapElement" />
    public class Mountain : MapElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mountain"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Mountain(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"M - {X} - {Y}";
        }
    }
}
