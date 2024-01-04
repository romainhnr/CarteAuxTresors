namespace CarteAuxTresors.Api.Models
{
    /// <summary>
    /// Classe représentant l'objet trésor
    /// </summary>
    /// <seealso cref="CarteAuxTresors.Models.MapElement" />
    public class Treasure : MapElement
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Treasure"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="count">The count.</param>
        public Treasure(int x, int y, int count)
        {
            X = x;
            Y = y;
            Count = count;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"T - {X} - {Y} - {Count}";
        }
    }
}
