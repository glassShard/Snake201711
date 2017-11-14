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
        /// 
        /// ha lehet, ne használjunk null értéket: mindig inicializáljuk, ha mást nem egy üres listával
        /// </summary>
        public List<GamePoint> GamePoints { get; set; } = new List<GamePoint>();

        /// <summary>
        /// A kígyó hossza
        /// csak lekérdezhető, így nem módosítható, így nincs un. settere
        /// és saját kóddal a gettert implementáltuk: összekötöttük a kígyó pontjaivak listájával.
        /// </summary>
        public int Length
        {
            get
            {
                //if (GamePoints==null)
                //{
                //    return 0;
                //}

                //GamePoints?.Count: ha a GamePoints értéke null, akkor a végeredmény null
                //                   , ha nem, akkor veszi a Count propertyjét
                //    a ?? b: ha a értéke null, akkor b, ha nem null akkor a
                //return GamePoints?.Count ?? 0;

                return GamePoints.Count;
            }
        }

        ///Property tulajdonságai (a field-hez viszonyított további lehetőségei)
        ///külön lehet szabályozni, hogy írható és hogy olvasható-e?
        ///get: jelenti: olvasható
        ///set: jelenti: írható
        ///
        /// az írhatóság és olvashatóság kivülről történő láthatósága is külön szabályozható:
        /// private get: csak osztályon használható
        /// private set: csak osztályon belül használható
        /// 
        /// ráadásul a működése implementálható: saját kóddal adhatjuk meg, hogy mit tegyen


    }
}
