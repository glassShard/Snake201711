using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake201711.Models
{
    /// <summary>
    /// A kígyót leíró osztály
    /// </summary>
    public class Snake
    {
        /// <summary>
        /// Mutatja, hogy a kígyó feje éppen merre áll
        /// </summary>
        public SnakeDirections Direction { get; set; }

        /// <summary>
        /// A kígyó pontjait tartalmazó lista
        /// </summary>
        public List<GamePoint> GamePoints { get; set; }
        public int Length { get; set; }
    }
}
