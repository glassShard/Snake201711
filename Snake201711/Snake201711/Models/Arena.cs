using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
            GameTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Normal, ItIsTimeForShow, Application.Current.Dispatcher);
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
