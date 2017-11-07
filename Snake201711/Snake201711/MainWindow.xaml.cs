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
    }
}
