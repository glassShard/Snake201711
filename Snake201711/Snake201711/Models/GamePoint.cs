 namespace Snake201711.Models
{
    /// <summary>
    /// A játéktáblán egy (étel vagy kígyó) pontot jelképező egység
    /// </summary>
    public class GamePoint
    {

        public GamePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int CanvasIndex { get; set; }
    }
}