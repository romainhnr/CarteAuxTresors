namespace CarteAuxTresors.Api.Models
{
    /// <summary>
    /// Classe abstraite représentant un élément de la carte (Montagne, Trésor, Aventurier)
    /// </summary>
    public abstract class MapElement
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public int Y { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public abstract override string ToString();
    }
}
