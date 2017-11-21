using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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
        /// A(z aktuális) játékban szereplő kígyó
        /// </summary>
        private Snake Snake;

        /// <summary>
        /// Az ételek listája, amit a kígyó aktuálisan megehet
        /// </summary>
        private List<Meal> Meals = new List<Meal>();

        /// <summary>
        /// Véletlenszámgenerátor, az aréna létrejöttekor egyből létre is hozzuk
        /// </summary>
        private Random randomNumberGenerator = new Random();

        /// <summary>
        /// Ezzel szabályozzuk, hogy egyszerre csak egy megjelenítés fusson
        /// </summary>
        private bool IsInShow=false;
        private bool IsGameInProgress;

        // vászonra rajzolt kép egységei 
        private double EllipseWidth;
        private double EllipseHeight; 

         /// <summary>
        /// Arena konstruktor, létrehozáskor megkapja a megjelenítő ablakot
        /// </summary>
        /// <param name="mainWindow">Az ablak, ami megjeleníti a játék menetét</param>
        public Arena(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            SetNewGameCounters();
            ShowGameCounters();
 
            //var meal = new Meal(10,15);
            //meal.PeldanyszintuColor = Brushes.Bisque;

            //var meal2 = new Meal(10, 15);
            //meal2.PeldanyszintuColor = Brushes.Black;

            //Meal.OsztalyszintuColor = Brushes.Beige;
            //Meal.OsztalyszintuColor = Brushes.Azure;

        }

        /// <summary>
        /// Játék indítása
        /// </summary>
        public void Start()
        {
            InitializeArena();

            SetNewGameCounters();
            //megelőzi az ételeket, mert csak oda tehetek
            //ételt, ahol nincs kígyó,
            //mivel az étel egy elemű, a kígyó nem
            //egyszerűbb az ételnek a kígyóhoz igazodni.
            SetSnakeForStart();
            SetMealsForStart();
            GameTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Normal, ItIsTimeForShow, Application.Current.Dispatcher);
            IsGameInProgress = true;
            SetButtonAvailability();
        }

        private void InitializeArena()
        {
            if (GameTimer != null)
            { //ha már korábban elindítottuk a játékot, akkor ezt most megállítjuk
                GameTimer.Stop();
            }

            //ezt a ciklust a lista módosításához nem használhatom!
            //foreach (var meal in Meals)
            //{
            //    RemoveMeal(meal);
            //}

            //while ciklussal könnyű végtelen ciklust használni
            //while (Meals.Count > 0)
            //{
            //    RemoveMeal(Meals[0]);
            //}

            //ételek törlése
            var cnt = Meals.Count;
            for (int i = 0; i < cnt; i++)
            {
                RemoveMeal(Meals[0]);
            }

            //kígyó törlése
            cnt = Snake.GamePoints.Count;
            for (int i = 0; i < cnt; i++)
            {
                RemoveSnakeTail(Snake.GamePoints[0]);
            }


        }

        private void SetButtonAvailability()
        {
            MainWindow.ButtonStart.IsEnabled = !IsGameInProgress;
            MainWindow.ButtonStop.IsEnabled = IsGameInProgress;
        }

        private void SetSnakeForStart()
        {

            Snake = new Snake();
            Snake.GamePoints = new List<GamePoint>();


            //generáljuk a kígyó fejét
            var head = GetRandomMeal();
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
            Meals = new List<Meal>();

            ////tesztelünk, a négy sarokban elhelyezek négy ételt, hogy jól látszik-e?
            //Meals.Add(new GamePoint(1, 1));
            //Meals.Add(new GamePoint(1, 20));
            //Meals.Add(new GamePoint(20, 1));
            //Meals.Add(new GamePoint(20, 20));

            //foreach (var meal in Meals)
            //{
            //    ShowMeal(meal);
            //}

            //todo: házi feladat: ezt hogyan tudjuk tesztelni, hogy csak olyan helyekre teszünk, ami még nincs kiosztva
            while (Meals.Count < ArenaSettings.MealsCountForStart)
            { //addig megyünk, amíg sikerül kirakni valamennyi ételt
                GetNewMeal();
            }

        }

        private void GetNewMeal()
        {
            var meal = GetRandomMeal();

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
                CreateMeal(meal);
            }
        }

        /// <summary>
        /// Kijelöl egy véletlen pontot a táblán
        /// </summary>
        /// <returns></returns>
        private Meal GetRandomMeal()
        {
            //a random intervallum felső határa nincs a kiosztott számok között
            //ezért hozzáadok a lehetséges maximumhoz egyet, hogy a maximumot is 
            //kiossza
            var x = randomNumberGenerator.Next(1, ArenaSettings.MaxX+1);
            var y = randomNumberGenerator.Next(1, ArenaSettings.MaxY+1);

            var meal = new Meal(x: x, y: y);
            return meal;
        }

        /// <summary>
        /// Megjelenítjük az ételt a táblán
        /// </summary>
        /// <param name="meal"></param>
        private void CreateMeal(Meal meal)
        {
            var child = GetGridArenaCell(meal);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.Star;
            child.Foreground = Brushes.Red;
            child.Spin = true;
            child.SpinDuration = 5;

            //hozzáadni a listához
            Meals.Add(meal);

            //Kirakjuk a Canvas-ra is

            //létrehozzuk
            var paint = new Ellipse();

            SetEllipseSize(paint);

            //megformázzuk
            paint.Fill = Brushes.OrangeRed;
            //todo: a méretezést a képernyőhöz igazítani
            
            //összehangoljuk a vászonnal
            Canvas.SetTop(paint, (meal.Y - 1) * EllipseHeight);
            Canvas.SetLeft(paint, (meal.X - 1) * EllipseWidth);

            //kirakjuk a vászonra
            MainWindow.CanvasArena.Children.Add(paint);

            //var index = MainWindow.CanvasArena.Children.IndexOf(paint);
            //meal.CanvasIndex = index;

        }

        private void SetEllipseSize(Ellipse paint)
        {
            EllipseWidth = MainWindow.CanvasArena.ActualWidth / ArenaSettings.MaxX;
            EllipseHeight = MainWindow.CanvasArena.ActualHeight / ArenaSettings.MaxY;
            paint.Height = EllipseHeight;
            paint.Width = EllipseWidth;
        }

        internal void ResizeCanvasElements(double widthRatio, double heightRatio)
        {
            foreach (Ellipse element in MainWindow.CanvasArena.Children)
            {
                double top = Canvas.GetTop(element);
                double left = Canvas.GetLeft(element);

                SetEllipseSize(element);

                Canvas.SetTop(element, top * heightRatio);
                Canvas.SetLeft(element, left * widthRatio);
            }
        }

        /// <summary>
        /// Eltüntetjük az ételt a tábláról
        /// </summary>
        /// <param name="meal"></param>
        private void RemoveMeal(Meal meal)
        {

            var child = GetGridArenaCell(meal);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;
            child.Foreground = Brushes.Black;
            child.Spin = false;
            child.SpinDuration = 1;

            //meghatározzuk, hányadik ételről beszélünk
            var index = Meals.IndexOf(meal);
            //leszedjük a vászonról
            MainWindow.CanvasArena.Children.RemoveAt(index);

            //végül eltüntetjük a nyilvántartásból
            Meals.Remove(meal);


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
        private void RemoveSnakeTail(GamePoint tailEnd)
        {
            var child = GetGridArenaCell(tailEnd);
            child.Icon = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;
            child.Foreground = Brushes.Black;

            Snake.GamePoints.Remove(tailEnd);
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
            IsGameInProgress = false;
            SetButtonAvailability();
        }

        /// <summary>
        /// Ha leütjük valamelyik nyílgombot, akkor itt mnegérkezik
        /// </summary>
        /// <param name="key">jelzi, hogy melyik nyílgombot ütöttük le</param>
        public void KeyDown(Key key)
        {
            
            if (ArenaSettings.IsSittingInTheHeadOfSnake)
            { // a kígyó fejében ülünk
                switch (key)
                {
                    case Key.Left:
                        switch (Snake.Direction)
                        {
                            case SnakeDirections.None:
                                var head = Snake.Head;
                                var neck = Snake.Neck;

                                if (head.X<neck.X)
                                { //a kígyó balra áll
                                    Snake.Direction = SnakeDirections.Down;
                                }
                                else
                                {
                                    Snake.Direction = SnakeDirections.Up;
                                }
                                break;
                            case SnakeDirections.Left:
                                Snake.Direction = SnakeDirections.Down;
                                break;
                            case SnakeDirections.Right:
                                Snake.Direction = SnakeDirections.Up;
                                break;
                            case SnakeDirections.Up:
                                Snake.Direction = SnakeDirections.Left;
                                break;
                            case SnakeDirections.Down:
                                Snake.Direction = SnakeDirections.Right;
                                break;
                            default:
                                throw new Exception($"Erre az irányra nem vagyunk felkészülve: {Snake.Direction}");
                        }
                        break;
                    case Key.Right:
                        switch (Snake.Direction)
                        {
                            case SnakeDirections.None:
                                var head = Snake.Head;
                                var neck = Snake.Neck;
                                if (head.X<neck.X)
                                {
                                    Snake.Direction = SnakeDirections.Up;
                                }
                                else
                                {
                                    Snake.Direction = SnakeDirections.Down;
                                }
                                break;
                            case SnakeDirections.Left:
                                Snake.Direction = SnakeDirections.Up;
                                break;
                            case SnakeDirections.Right:
                                Snake.Direction = SnakeDirections.Down;
                                break;
                            case SnakeDirections.Up:
                                Snake.Direction = SnakeDirections.Right;
                                break;
                            case SnakeDirections.Down:
                                Snake.Direction = SnakeDirections.Left;
                                break;
                            default:
                                break;
                        }
                        break;
                    case Key.Up:
                    case Key.Down:
                        //ezekben az esetekben nem változik a kígyó mozgása,
                        //a gombok "nem élnek"
                        break;
                    default:
                        throw new Exception($"Erre a gombra nem vagyunk felkészülve: {key}");
                }
            }
            else
            { // kívülről nézzük a játékot
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
        }

        /// <summary>
        /// Ha a játéknak eljön a következő órajele, akkor ezt a függvényt hívjuk meg.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItIsTimeForShow(object sender, EventArgs e)
        {
            //mivel 100 millisec-enként hívjuk ezt a függvényt, egyszerre nem 
            //érkezhet ide két végrehajtás, ezért ez jó megoldás
            if (IsInShow)
            {
                return;
            }
            IsInShow = true;

            //frissíteni a játékidőt
            PlayTime = PlayTime.Add(TimeSpan.FromMilliseconds(100));

            //játékmenet frissítése

            //a kígyó feje mozog a kijelölt irányba
            var oldHead = Snake.Head;
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
                IsInShow = false;
                return;
            }

            //le kell elenőrizni, hogy 
            //saját magába harapott-e?
            if (Snake.GamePoints.Any(gp=>gp.X==newHead.X && gp.Y == newHead.Y))
            { // magába harapott
                GameOver();
                IsInShow = false;
                return; //játék vége, nincs tovább
            }

            //le kell elenőrizni, hogy 
            //nekimentünk-e a falnak?
            //azért tettük be az első és utolsó sort + oszlopot, hogy
            //azok jelentsék a falat. Tehát a 0. és a Max+1. sor és oszlop
            //a fal
            if (newHead.X == 0 || newHead.Y == 0 || newHead.X == ArenaSettings.MaxX+1 || newHead.Y == ArenaSettings.MaxY+1)
            { // nekiment a falnak
                GameOver();
                IsInShow = false;
                return; //játék vége, nincs tovább
            }

            //új fejet hozzáadjuk a kígyóhoz
            //a lista legelejére. Ha itt adjuk hozzá az új fejet, akkor
            //benne lesz a Snake.GamePoints listában és az étel generálás már 
            //figyelembe veszi.
            Snake.Head = newHead;
            ShowSnakeHead(newHead);

            //le kell elenőrizni, hogy 
            //megettünk-e ételt?
            var isEated = Meals.Any(gp => gp.X == newHead.X && gp.Y == newHead.Y);
            if (isEated)
            { // ételt ettünk

                //méghozzá ezt
                var meal = Meals.Single(gp => gp.X == newHead.X && gp.Y == newHead.Y);

                Snake.Eat(meal);

                RemoveMeal(meal);

                while (Meals.Count < ArenaSettings.MealsCountForStart)
                { //addig megyünk, amíg minden étel megvan, ez jelen esetben mindig az utolsó
                    GetNewMeal();
                }
            }

            //megjeleníteni a kígyó új helyzetét
            //régi fej már farok, hiszen a kígyó továbbcsúszott
            ShowSnakeTail(oldHead);

            //ha nem evett, akkor a farok végét eltüntetni
            if (!isEated)
            {
                var tailEnd = Snake.TailEnd;
                RemoveSnakeTail(tailEnd);
            }

            //kiírni a képernyőre
            ShowGameCounters();
            IsInShow = false;
        }

        private void GameOver()
        {
            MainWindow.LabelGameOver.Content = "Játék vége";
            Stop();
        }

        /// <summary>
        /// Beállítja a játék alapállapotának megfelelő számolókat
        /// </summary>
        private void SetNewGameCounters()
        {
            //pontszámok "lenullázása"
            //PlayTime = new TimeSpan(0, 0, 0);
            PlayTime = TimeSpan.FromSeconds(0);
            Snake = new Snake();
            MainWindow.LabelGameOver.Content = "";
            ShowGameCounters();
        }

        /// <summary>
        /// Megjeleníti/frissíti a játék számlálóit
        /// </summary>
        private void ShowGameCounters()
        {
            MainWindow.LabelPlayTime.Content = $"Játékidő: {PlayTime.ToString("mm\\:ss")}";
            MainWindow.LabelPoints.Content = $"Pontszám: {Snake.Points}";
            MainWindow.LabelEatenMealsCount.Content = $"Megevett ételek: {Snake.EatenMealsCount}";
            MainWindow.LabelSnakeLength.Content = $"Kígyó hossza: {Snake.Length}";
            MainWindow.LabelKeyDown.Content = $"Irány: {Snake.Direction}";
        }

    }
}
