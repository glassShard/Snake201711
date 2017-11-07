using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Snake201711.Models
{
    /// <summary>
    /// A játékunk működéséért felelő osztály
    /// 
    /// </summary>
    public class Arena
    {
        private DispatcherTimer GameTimer;
        private TimeSpan PlayTime;
        private MainWindow MainWindow;

        public Arena(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        /// <summary>
        /// Játék indítása
        /// </summary>
        public void Start()
        {
            //pontszámok "lenullázása"
            //PlayTime = new TimeSpan(0, 0, 0);
            PlayTime = TimeSpan.FromSeconds(0);
            GameTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Normal, ItIsTimeForShow, Application.Current.Dispatcher);
        }

        private void ItIsTimeForShow(object sender, EventArgs e)
        {
            //frissíteni a játékidőt
            PlayTime = PlayTime.Add(TimeSpan.FromMilliseconds(100));

            //kiírni a képernyőre
            MainWindow.LabelPlayTime.Content = $"Játékidő: {PlayTime.ToString("mm\\:ss")}";
        }

        /// <summary>
        /// Játék megállítása
        /// </summary>
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
