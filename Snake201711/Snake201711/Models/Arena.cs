using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Snake201711.Models
{
    /// <summary>
    /// A játékunk működéséért felelő osztály
    /// 
    /// </summary>
    public class Arena
    {

        /// <summary>
        /// A játék "órajelét" adó időzítő
        /// </summary>
        private DispatcherTimer GameTimer;

        /// <summary>
        /// A képernyő, amin a játék fut (megjelenik)
        /// </summary>
        private MainWindow MainWindow;

        /// <summary>
        /// A(z aktuális) játék eltelt ideje
        /// </summary>
        private TimeSpan PlayTime;

        /// <summary>
        /// A(z aktuális) játék pontszáma
        /// </summary>
        private int Points;

        /// <summary>
        /// A(z aktuális) játékban megevett ételek száma
        /// </summary>
        private int EatenMealsCount;

        /// <summary>
        /// A(z aktuális) játékban szereplő kígyó
        /// </summary>
        private Snake Snake;

        /// <summary>
        /// Az ételek listája, amit a kígyó aktuálisan megehet
        /// </summary>
        private List<GamePoint> Meals;

        /// <summary>
        /// Véletlenszámgenerátor
        /// </summary>
        private Random randomNumberGenerator;

        /// <summary>
        /// Arena konstruktor, létrehozáskor megkapja a megjelenítő ablakot
        /// </summary>
        /// <param name="mainWindow">Az ablak, ami megjeleníti a játék menetét</param>
        public Arena(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            SetNewGameCounters();
            ShowGameCounters();
        }

        /// <summary>
        /// Játék indítása
        /// </summary>
        public void Start()
        {
            SetNewGameCounters();
            SetMealsForStart();
            GameTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Normal, ItIsTimeForShow, Application.Current.Dispatcher);
        }

        private void SetMealsForStart()
        {
            Meals = new List<GamePoint>();

            randomNumberGenerator = new Random();


            //ez így már működik, de_
            //1. a kibányászás kelleni fog majd, így jó lenne kiszervezni egy függvénybe
            //2. most nem kezeljük, hogyha olyan helyre tesszük a csillagot, ahol már van
            //3. A megjelenítést is érdemes lenn kiszervezni.
            for (int i = 0; i < ArenaSettings.MealsCountForStart; i++)
            {
                var x = randomNumberGenerator.Next(1, ArenaSettings.MaxX);
                var y = randomNumberGenerator.Next(1, ArenaSettings.MaxY);

                var meal = new GamePoint(x: x, y: y);

                //megjelenítés
                //midegyik sor 20 (MaxX) elemből áll, tehát úgy tudom megtalálni az adott 
                //koordinátát, hogy pl. a harmadik sorban lévő 5 elemhez elmegyek 2*20=40 elemet (2=3-1), 
                //majd elindulok a soron belül, és az első elemtől még lépek 4-et (4=5-1)
                var child = MainWindow.GridArena.Children[(meal.Y - 1) * ArenaSettings.MaxX + (meal.X - 1)];

                //Ez a Children gyűjtemén un. UIElement-ekből áll, ebbe van becsomagolva minden felületi megjelenítő
                //ahhoz, hogy kibányásszuk a benne lévő ImageAwesome vezérlőt, el kell kérnünk a változótól:
                //ezt az eléírt zárójeles típussal tudjuk megtenni
                //a második zárójel azért kell, hogy először ezt a "kibányászást" hajtsa végre, aztán
                //menjen tovább, mert így már van Icon property-nk
                ((FontAwesome.WPF.ImageAwesome)child).Icon = FontAwesome.WPF.FontAwesomeIcon.Star;
                ((FontAwesome.WPF.ImageAwesome)child).Foreground = Brushes.Red;
                ((FontAwesome.WPF.ImageAwesome)child).Spin = true;
                ((FontAwesome.WPF.ImageAwesome)child).SpinDuration = 5;


                //hozzáadni a listához
                Meals.Add(meal);
            }

        }

        /// <summary>
        /// Játék megállítása
        /// </summary>
        public void Stop()
        {
            GameTimer.Stop();
        }

        /// <summary>
        /// Ha leütjük valamelyik nyílgombot, akkor itt mnegérkezik
        /// </summary>
        /// <param name="key">jelzi, hogy melyik nyílgombot ütöttük le</param>
        public void KeyDown(Key key)
        {
            //todo: ha úgy tetszik, házi feladat: hogy kell megoldani azt, ha a fejben ülünk, és csak jobbra/balra nyíllal vezérlünk?

            //A leütött billenytű jelzi, hogy merre kell a kígyónak továbbmennie
            switch (key)
            {
                case Key.Left:
                    Snake.Direction = SnakeDirections.Left;
                    break;
                case Key.Right:
                    Snake.Direction = SnakeDirections.Right;
                    break;
                case Key.Up:
                    Snake.Direction = SnakeDirections.Up;
                    break;
                case Key.Down:
                    Snake.Direction = SnakeDirections.Down;
                    break;
                default:
                    throw new Exception($"Erre a gombra nem vagyunk felkészülve: {key}");
            }
        }

        /// <summary>
        /// Ha a játéknak eljön a következő órajele, akkor ezt a függvényt hívjuk meg.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItIsTimeForShow(object sender, EventArgs e)
        {
            //frissíteni a játékidőt
            PlayTime = PlayTime.Add(TimeSpan.FromMilliseconds(100));

            //kiírni a képernyőre
            ShowGameCounters();
        }

        /// <summary>
        /// Beállítja a játék alapállapotának megfelelő számolókat
        /// </summary>
        private void SetNewGameCounters()
        {
            //pontszámok "lenullázása"
            //PlayTime = new TimeSpan(0, 0, 0);
            PlayTime = TimeSpan.FromSeconds(0);
            Points = 0;
            EatenMealsCount = 0;
            Snake = new Snake();
        }


        /// <summary>
        /// Megjeleníti/frissíti a játék számlálóit
        /// </summary>
        private void ShowGameCounters()
        {
            MainWindow.LabelPlayTime.Content = $"Játékidő: {PlayTime.ToString("mm\\:ss")}";
            MainWindow.LabelPoints.Content = $"Pontszám: {Points}";
            MainWindow.LabelEatenMealsCount.Content = $"Megevett ételek: {EatenMealsCount}";
            MainWindow.LabelSnakeLength.Content = $"Kígyó hossza: {Snake.Length}";
            MainWindow.LabelKeyDown.Content = $"Irány: {Snake.Direction}";
        }

    }
}
