using Snake201711.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake201711
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Arena Arena;

        public MainWindow()
        {
            InitializeComponent();
            //this: hivatkozik arra az osztálypéldányra, 
            //amiben éppen vagyok, vagyis a megjelenített ablakot küldöm be az Arena példányba 
            Arena = new Arena(this);  
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Arena.Start();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            Arena.Stop();
        }

        /// <summary>
        /// Az ablakban leütött billentyűk figyelése, szűrése és továbbítása az Arena-nak
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    //Ha Left VAGY Right vagy Up vagy Down, akkor ez lefut
                    Arena.KeyDown(e.Key);
                    //Mivel ezek a gombok a játékhoz tartoznak,
                    //nem kell további feldolgozás, ezt jelezzük
                    e.Handled = true;
                    break;
                default:
                    break;
            }

        }

        private void CanvasArena_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double widthRatio;
            double heightRatio;

            if (e.PreviousSize.Width == 0 || e.PreviousSize.Height == 0)
            {
                widthRatio = 0;
                heightRatio = 0;
            }
            else
            {
                widthRatio = e.NewSize.Width / e.PreviousSize.Width;
                heightRatio = e.NewSize.Height / e.PreviousSize.Height;
            }
            Arena.ResizeCanvasElements(widthRatio, heightRatio);
        }
    }
}
