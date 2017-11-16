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
        /// Megevett ételek száma, csak az osztályon belülről módosítható
        /// </summary>
        public int EatenMealsCount { get; private set; } = 0;

        /// <summary>
        /// Elért pontszám, csak az osztályon belülről módosítható
        /// </summary>
        public int Points { get; private set; } = 0;

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

        /// <summary>
        /// Visszaadja a kígyó fejét
        /// </summary>
        /// 

        //getter és setter röviden
        //public GamePoint Head_Get() { }
        //public void Head_Set(GamePoint value) { };
        public GamePoint Head
        {
            //getter: függvény, aminek NINCS paramétere és a visszatérési típusa a property típusa
            get
            {
                return GamePoints[0];
            }

            //setter: függvény, aminek egy paramétere van, ennek típusa a property típusa, és nincs visszatérési értéke
            set
            {
                GamePoints.Insert(0, value);
            }
        }

        /// <summary>
        /// A kígyó "nyaka": a fej utáni első farokrész
        /// </summary>
        public GamePoint Neck
        {
            get
            {
                return GamePoints[1];
            }
        }

        public GamePoint TailEnd
        {
            get
            {
                return GamePoints[GamePoints.Count - 1];
            }
        }

        /// <summary>
        /// A kígyó eszik
        /// </summary>
        /// <param name="meal">megevett étel</param>
        public void Eat(Meal meal)
        {
            EatenMealsCount = EatenMealsCount + 1;
            Points = Points + meal.Points;
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
        /// 

        /// A C# fordító ha nem implementálunk gettert és settert, 
        /// akkor elfogadja ezt a szintaxist:
        //public int MyProperty { get; set; }

        /// A fordító ebből egy ilyet csinál (property defoult implementáció
        //private int _myProperty;
        //public int MyProperty
        //{
        //    get
        //    {
        //        return _myProperty;
        //    }
        //    set
        //    {
        //        _myProperty = value;
        //    }
        //}

        ///És ez egyenértékű ezzel:
        //private int _myProperty;
        //public int MyProperty_Get()
        //{
        //    return _myProperty;
        //}
        //public void MyProperty_Set(int value)
        //{
        //    _myProperty = value;
        //}



    }
}
