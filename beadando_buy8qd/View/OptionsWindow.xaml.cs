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
using System.Windows.Shapes;
using Beadando.ViewModel;

namespace Beadando.View
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow(BL bl, MainWindow main)
        {
            InitializeComponent();
            this.bl = bl;
            this.mainW = main;
            
        }

        BL bl;
        MainWindow mainW;
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bl.Save();
            MessageBox.Show("Játékállásodat mentettük", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {

            bl.Peek();
            LoadWindow load = new LoadWindow(bl, this);
            if (load.ShowDialog() == true)
            {
                CloseWindows();
            }
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            
            //we set the mainwindow as the main menu
            MainMenu main = new MainMenu();
            App.Current.MainWindow = main;
            CloseWindows();

            //we show the main menu
            main.ShowDialog();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            bl.Quit();
        }

        void CloseWindows()
        {
            //we close both the options menu and the main window
            this.Close();
            mainW.Close();
        }
    }
}
