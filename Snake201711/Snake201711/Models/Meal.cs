using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Snake201711.Models
{
    /// <summary>
    /// Leszármaztatjuk a GamePoint-ból, mert
    /// szeretnénk, ha a Meal-nek ugyanazok a tulajdonságai
    /// meglennének, mint a GamePoint-nak. (X és Y koordináta)
    /// </summary>
    public class Meal : GamePoint
    {
        public Meal(int x, int y) : base(x, y)
        { }

        /// <summary>
        /// Lehet statikus:
        /// property 
        /// field
        /// method
        /// </summary>
        //public static SolidColorBrush OsztalyszintuColor { get; internal set; }
        //public static int Szam = 10;
        //public static void Fuggveny() { }

        //public SolidColorBrush PeldanyszintuColor { get; internal set; }

        public int Points { get; } = 3;

    }
}
