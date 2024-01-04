namespace CarteAuxTresors.Api.Models
{
    /// <summary>
    /// Classe représentant l'aventurier
    /// </summary>
    /// <seealso cref="MapElement" />
    public class Adventurer : MapElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or sets the collected treasures.
        /// </summary>
        /// <value>
        /// The collected treasures.
        /// </value>
        public int CollectedTreasures { get; set; }

        /// <summary>
        /// Gets or sets the movements.
        /// </summary>
        /// <value>
        /// The movements.
        /// </value>
        public string Movements { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Adventurer"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="orientation">The orientation.</param>
        /// <param name="movements">The movements.</param>
        public Adventurer(string name, int x, int y, string orientation, string movements)
        {
            Name = name;
            X = x;
            Y = y;
            Orientation = orientation;
            Movements = movements;
            CollectedTreasures = 0;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"A - {Name} - {X} - {Y} - {Orientation} - {CollectedTreasures}";
        }

        /// <summary>
        /// Executes the movements.
        /// </summary>
        /// <param name="map">The map.</param>
        public void ExecuteMovements(Map map)
        {
            foreach (char movement in Movements)
            {
                switch (movement)
                {
                    case 'A':
                        MoveForward(map);
                        break;
                    case 'G':
                        TurnLeft();
                        break;
                    case 'D':
                        TurnRight();
                        break;
                    default:
                        // Ignorer les mouvements inconnus
                        break;
                }
            }
        }

        /// <summary>
        /// Moves forward.
        /// </summary>
        /// <param name="map">The map.</param>
        private void MoveForward(Map map)
        {
            int newX = X;
            int newY = Y;

            // Calculer les nouvelles coordonnées en fonction de l'orientation
            switch (Orientation)
            {
                case "N":
                    newY = Math.Max(0, Y - 1);
                    break;
                case "S":
                    newY = Math.Min(map.Height - 1, Y + 1);
                    break;
                case "E":
                    newX = Math.Min(map.Width - 1, X + 1);
                    break;
                case "O":
                    newX = Math.Max(0, X - 1);
                    break;
                default:
                    // Ignorer les orientations inconnues
                    break;
            }

            // Vérifier s'il y a une montagne ou un autre aventurier à la nouvelle position
            if (!HasMountainOrAdventurer(map, newX, newY))
            {
                // Mettre à jour les coordonnées de l'aventurier
                X = newX;
                Y = newY;

                // Collecter les trésors s'il y en a
                CollectTreasures(map);
            }
        }

        /// <summary>
        /// Turns the left.
        /// </summary>
        private void TurnLeft()
        {
            // Tourner à gauche en mettant à jour l'orientation
            switch (Orientation)
            {
                case "N":
                    Orientation = "O";
                    break;
                case "S":
                    Orientation = "E";
                    break;
                case "E":
                    Orientation = "N";
                    break;
                case "O":
                    Orientation = "S";
                    break;
                default:
                    // Ignorer les orientations inconnues
                    break;
            }
        }

        /// <summary>
        /// Turns the right.
        /// </summary>
        private void TurnRight()
        {
            // Tourner à droite en mettant à jour l'orientation
            switch (Orientation)
            {
                case "N":
                    Orientation = "E";
                    break;
                case "S":
                    Orientation = "O";
                    break;
                case "E":
                    Orientation = "S";
                    break;
                case "O":
                    Orientation = "N";
                    break;
                default:
                    // Ignorer les orientations inconnues
                    break;
            }
        }

        /// <summary>
        /// Determines whether the specified map has mountain or other adventurer.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if the specified map has mountain or other adventurer; otherwise, <c>false</c>.
        /// </returns>
        private bool HasMountainOrAdventurer(Map map, int x, int y)
        {
            // Vérifier si une montagne ou un autre aventurier est présent à la position spécifiée
            foreach (MapElement element in map.Elements)
            {
                if ((element is Mountain || element is Adventurer) && element != this && element.X == x && element.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Collects the treasures.
        /// </summary>
        /// <param name="map">The map.</param>
        private void CollectTreasures(Map map)
        {
            // Vérifier si une case contient des trésors à collecter
            foreach (MapElement element in map.Elements)
            {
                if (element is Treasure treasure && element.X == X && element.Y == Y && treasure.Count > 0)
                {
                    treasure.Count--;
                    CollectedTreasures++;
                }
            }
        }
    }
}
