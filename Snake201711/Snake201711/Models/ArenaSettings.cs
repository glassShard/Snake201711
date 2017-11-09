namespace Snake201711.Models
{
    public class ArenaSettings
    {
        /// <summary>
        /// A játékban egyszerre látható ételek száma
        /// todo: static magyarázat
        /// megjegyzések:
        /// 1. kitöröltem a set-et a kódból: nem lehet módosítani az értékét: csak olvasható
        /// 2. egyből adtam neki értéket
        /// </summary>
        public static int MealsCountForStart { get; } = 8;

        //A kígyó hossza a játék kezdetén
        public static int SnakeCountForStart { get; } = 5;

        /// <summary>
        /// A játéktábla mérete vízszintes irányban
        /// 
        /// todo: házi feladat, ezt esetleg kitölteni valahogy, az Arena példányosításakor
        /// </summary>
        public static int MaxX { get; } = 20;

        /// <summary>
        /// A játéktábla mérete függőleges irányban
        /// </summary>
        public static int MaxY { get; } = 20;

    }
}