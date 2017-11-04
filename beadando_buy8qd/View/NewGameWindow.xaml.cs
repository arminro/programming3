using Beadando.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Beadando.View
{
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        BL bl;
        
        public NewGameWindow(BL bl)
        {
            InitializeComponent();
            this.bl = bl;
            this.DataContext = this.bl;
            bl.CloseOpenWindows += (object sender, EventArgs eve) =>
            {
                this.Close();
            };
            bl.GeneralNotification += (object s, TransferEventArgs eve) =>
            {
                MessageBox.Show(eve.Load, "Már van ilyen játékos", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            };
            
            
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            bl.InitializeGame();
            bl.InitializeStartOfNewGame();
            this.DialogResult = true;
            MainWindow main = new MainWindow(bl);
            App.Current.MainWindow = main;
            bl.CloseWindows();
            main.Show();
           
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            bl.Players.Clear();
            this.DialogResult = false;
        }

        private void NewPlayer_Click(object sender, RoutedEventArgs e)
        {
            bl.AddPlayer();
        }

        private void DeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            bl.DeletePlayer(bl.NewGameSelectedPlayer);
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*We deny selecting the same type of player for the nth time by setting the*/
            ComboBox combo = (sender as ComboBox);
            //the event fires again when we set the selected value to null
            if (combo.SelectedValue != null)
            {
               
                if (!bl.CheckIfApplicable((string)combo.SelectedValue))
                {
                    bl.ChangePuppetKey(combo.DataContext, combo.SelectedValue, e.RemovedItems.Count > 0 ? e.RemovedItems[0] : null);
                }
                else
                {
                    MessageBox.Show("Nem lehet 2 ugyan olyan játékos!", "Már van ilyen játékos", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    combo.SelectedItem = null;
                } 
            }
        }
    }
}
