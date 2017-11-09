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
        private List<GamePoint> Meals = null;

        /// <summary>
        /// Véletlenszámgenerátor, az aréna létrejöttekor egyből létre is hozzuk
        /// </summary>
        private Random randomNumberGenerator = new Random();

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
            //megelőzi az ételeket, mert csak oda tehetek
            //ételt, ahol nincs kígyó,
            //mivel az étel egy elemű, a kígyó nem
            //egyszerűbb az ételnek a kígyóhoz igazodni.
            SetSnakeForStart();
            SetMealsForStart();
            if (GameTimer!=null)
            { //ha már korábban elindítottuk a játékot, akkor ezt most megállítjuk
                GameTimer.Stop();
            }
            GameTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Normal, ItIsTimeForShow, Application.Current.Dispatcher);
        }

        private void SetSnakeForStart()
        {

            Snake = new Snake();
            Snake.GamePoints = new List<GamePoint>();


            //generáljuk a kígyó fejét
            var head = GetRandomGamePoint();
            Snake.GamePoints.Add(head);


            //egyszerű kígyóelhelyezés:
            //vizszintesen rajzolunk,
            //ha a x 10-nél nagyobb, akkor balra
            //ha az x 10-nél kisebb, akkor jobbra

            //A kígyó fejét megjelenítjük
            ShowSnakeHead(head);

            for (int i = 0; i < ArenaSettings.SnakeCountForStart; i++)
            {
                GamePoint gamePoint;
                if (head.X <= 10)
                { //jobbra nyúlik
                    gamePoint = new GamePoint(head.X + i + 1, head.Y);
                }
                else
                { //balra nyúlik
                    gamePoint = new GamePoint(head.X - i - 1, head.Y);
                }
                Snake.GamePoints.Add(gamePoint);
                ShowSnakeTail(gamePoint);
            }
        }

        private void SetMealsForStart()
        {
            Meals = new List<GamePoint>();

            //todo: házi feladat: ezt hogyan tudjuk tesztelni, hogy csak olyan helyekre teszünk, ami még nincs kiosztva
            while (Meals.Count<ArenaSettings.MealsCountForStart)
            { //addig megyünk, amíg sikerül kirakni valamennyi ételt
                GetNewMeal();
            }

        }

        private void GetNewMeal()
        {
            var meal = GetRandomGamePoint();

            //Az Any függvény igazat ad, ha bármelyik elemre 
            //a lambda függvény igazat ad
            //vagyis, a meal koordinátáival már szerepel étel a listán
            //ezt megfordítjuk a "!"-lel, 
            //vagyis csak akkor igaz a kifejezés, ha a meal koordinátái MÉG NEM 
            //szerepelnek a listán
            if (!Meals.Any(gamePoint =>
                    gamePoint.X == meal.X
                    && gamePoint.Y == meal.Y)
                && //és nem szerepel a koordináta a kígyó pontjai között sem
                !Snake.GamePoints.Any(gamePoint =>
                    gamePoint.X == meal.X
                    && gamePoint.Y == meal.Y)
            )
            { //Csak akkor, ha az étel még nincs a táblán
              //megjelenítés
                ShowMeal(meal);

                //hozzáadni a listához
                Meals.Add(meal);
            }
        }

        /// <summary>
        /// Kijelöl egy véletlen pontot a táblán
        /// </summary>
        /// <returns></returns>
        private GamePoint GetRandomGamePoint()
        {
            //todo: miért nem teszünk ételt az utolsó sorba és oszlopba??

            var x = randomNumberGenerator.Next(1, ArenaSettings.MaxX);
            var y = randomNumberGenerator.Next(1, ArenaSettings.MaxY);

            var gamePoint = new GamePoint(x: x, y: y);
            return gamePoint;
        }

        /// <summary>
        /// Megjelenítjük az ételt a táblán
        /// </summary>
        /// <param name="meal"></param>
        private void ShowMeal(GamePoint meal)
        {
            var child = GetGridArenaCell(meal);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.Star;
            child.Foreground = Brushes.Red;
            child.Spin = true;
            child.SpinDuration = 5;
        }

        /// <summary>
        /// Eltüntetjük az ételt a tábláról
        /// </summary>
        /// <param name="meal"></param>
        private void HideMeal(GamePoint meal)
        {
            var child = GetGridArenaCell(meal);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;
            child.Foreground = Brushes.Black;
            child.Spin = false;
            child.SpinDuration = 1;
        }

        /// <summary>
        /// A kígyó fejének megjelenítése
        /// </summary>
        /// <param name="head"></param>
        private void ShowSnakeHead(GamePoint head)
        {
            var child = GetGridArenaCell(head);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.Circle;
            child.Foreground = Brushes.Green;
        }

        /// <summary>
        /// A kígyó farkának megjelenítése
        /// </summary>
        private void ShowSnakeTail(GamePoint tail)
        {
            var child = GetGridArenaCell(tail);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.Circle;
            child.Foreground = Brushes.Blue;
        }

        /// <summary>
        /// A kígyó farka végének eltüntetése
        /// </summary>
        /// <param name="tailEnd"></param>
        private void HideSnakeTail(GamePoint tailEnd)
        {
            var child = GetGridArenaCell(tailEnd);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;
            child.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Kiválasztunk a táblán egy mezőt a megadott koordináták alapján
        /// </summary>
        /// <param name="gamePoint"></param>
        /// <returns></returns>
        private FontAwesome.WPF.ImageAwesome GetGridArenaCell(GamePoint gamePoint)
        {
            //midegyik sor 20 (MaxX) elemből áll, tehát úgy tudom megtalálni az adott 
            //koordinátát, hogy pl. a harmadik sorban lévő 5 elemhez elmegyek 2*20=40 elemet (2=3-1), 
            //majd elindulok a soron belül, és az első elemtől még lépek 4-et (4=5-1)
            var child = MainWindow.GridArena.Children[(gamePoint.Y - 1) * ArenaSettings.MaxX + (gamePoint.X - 1)];

            //Ez a Children gyűjtemén un. UIElement-ekből áll, ebbe van becsomagolva minden felületi megjelenítő
            //ahhoz, hogy kibányásszuk a benne lévő ImageAwesome vezérlőt, el kell kérnünk a változótól:
            //ezt az eléírt zárójeles típussal tudjuk megtenni
            var cell = (FontAwesome.WPF.ImageAwesome)child;

            return cell;
        }

        /// <summary>
        /// Játék megállítása
        /// </summary>
        public void Stop()
        {
            //todo: ezt jobban kidolgozni
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

            //játékmenet frissítése

            //a kígyó feje mozog a kijelölt irányba
            var oldHead = Snake.GamePoints[0];
            GamePoint newHead = null;
            switch (Snake.Direction)
            {
                case SnakeDirections.None:
                    break;
                case SnakeDirections.Left:
                    newHead = new GamePoint(oldHead.X - 1, oldHead.Y);
                    break;
                case SnakeDirections.Right:
                    newHead = new GamePoint(oldHead.X + 1, oldHead.Y);
                    break;
                case SnakeDirections.Up:
                    newHead = new GamePoint(oldHead.X, oldHead.Y - 1);
                    break;
                case SnakeDirections.Down:
                    newHead = new GamePoint(oldHead.X, oldHead.Y + 1);
                    break;
                default:
                    throw new Exception($"Erre nem vagyunk felkészülve: {Snake.Direction}");
            }

            if (newHead==null)
            { // nincs új fej, nincs mit tenni
                return;
            }

            //le kell elenőrizni, hogy 
            //saját magába harapott-e?
            if (Snake.GamePoints.Any(gp=>gp.X==newHead.X && gp.Y == newHead.Y))
            { // magába harapott
                GameOver();
            }

            //le kell elenőrizni, hogy 
            //nekimentünk-e a falnak?
            if (newHead.X == 0 || newHead.Y == 0 || newHead.X == ArenaSettings.MaxX || newHead.Y == ArenaSettings.MaxY)
            { // nekiment a falnak
                GameOver();
            }

            //le kell elenőrizni, hogy 
            //megettünk-e ételt?
            var isEated = Meals.Any(gp => gp.X == newHead.X && gp.Y == newHead.Y);
            if (isEated)
            { // ételt ettünk

                //méghozzá ezt
                var meal = Meals.Single(gp => gp.X == newHead.X && gp.Y == newHead.Y);
                HideMeal(meal);
                GetNewMeal();

                //todo: pontot számolni
            }

            //megjeleníteni a kígyó új helyzetét
            //régi fej már farok, hiszen a kígyó továbbcsúszott
            ShowSnakeTail(oldHead);

            //ha nem evett, akkor a farok végét eltüntetni
            if (!isEated)
            {
                var tailEnd = Snake.GamePoints[Snake.GamePoints.Count - 1];
                HideSnakeTail(tailEnd);
                Snake.GamePoints.Remove(tailEnd);
            }

            //új fejet hozzáadjuk a kígyóhoz
            //a lista legelejére
            Snake.GamePoints.Insert(0, newHead);
            ShowSnakeHead(newHead);

            //kiírni a képernyőre
            ShowGameCounters();
        }

        private void GameOver()
        {
            //todo: házi feladat
            throw new NotImplementedException();
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
