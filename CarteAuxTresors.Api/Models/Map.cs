namespace CarteAuxTresors.Api.Models
{
    /// <summary>
    /// Classe représentant la carte avec ses éléments
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>
        /// The elements.
        /// </value>
        public List<MapElement> Elements { get; } = new List<MapElement>();

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public Map(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"C - {Width} - {Height}";
        }

        /// <summary>
        /// Adds the specified element to the map.
        /// </summary>
        /// <param name="element">The element.</param>
        public void Add(MapElement element)
        {
            Elements.Add(element);
        }

        /// <summary>
        /// Simulates this instance.
        /// </summary>
        public void Simulate()
        {
            foreach (var adventurer in from MapElement element in Elements
                                       where element is Adventurer
                                       let adventurer = (Adventurer)element
                                       select adventurer)
            {
                adventurer.ExecuteMovements(this);
            }
        }
    }
}