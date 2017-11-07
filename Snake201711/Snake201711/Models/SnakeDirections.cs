namespace Snake201711.Models
{
    /// <summary>
    /// A kígyó fejének lehetséges irányai
    /// </summary>
    public enum SnakeDirections
    {
        /// <summary>
        /// alapértelmezett irány, ha nem adunk meg semmit
        /// </summary>
        None, 

        /// <summary>
        /// Balra megy a kígyó
        /// </summary>
        Left,

        /// <summary>
        /// Jobbra megy a kígyó
        /// </summary>
        Right,

        /// <summary>
        /// Felfelé megy a kígyó
        /// </summary>
        Up,

        /// <summary>
        /// Lefelé megy a kígyó
        /// </summary>
        Down
    }
}