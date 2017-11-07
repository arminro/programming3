// <copyright file="NewGameWindow.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        private BL bl;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewGameWindow"/> class.
        /// </summary>
        /// <param name="bl">reference to the business logic instance</param>
        public NewGameWindow(BL bl)
        {
            this.InitializeComponent();
            this.bl = bl;
            this.DataContext = this.bl;
            bl.CloseOpenWindows += (object sender, EventArgs eve) =>
            {
                this.Close();
            };
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (this.bl.ValidateStartNewGame())
            {
                this.bl.InitializeGame();
                this.bl.InitializeStartOfNewGame();
                this.DialogResult = true;
                MainWindow main = new MainWindow(this.bl);
                Application.Current.MainWindow = main;
                this.bl.CloseWindows();
                main.Show();
            }
            else
            {
                MessageBox.Show("Muszáj valamilyen karra beíratkozni!", "Nincs elég adat a játék megkezdéséhez");
            }

            e.Handled = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Players.Clear();
            this.DialogResult = false;
            e.Handled = true;
        }

        private void NewPlayer_Click(object sender, RoutedEventArgs e)
        {
            this.bl.AddPlayer();
            e.Handled = true;
        }

        private void DeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            this.bl.DeletePlayer(this.bl.NewGameSelectedPlayer);
            e.Handled = true;
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*We deny selecting the same type of player for the nth time by setting the*/
            ComboBox combo = sender as ComboBox;

            // the event fires again when we set the selected value to null
            if (combo.SelectedValue != null)
            {
                if (!this.bl.CheckIfApplicable((string)combo.SelectedValue))
                {
                    this.bl.ChangePuppetKey(combo.DataContext, combo.SelectedValue);
                }
                else
                {
                    MessageBox.Show("Nem lehet 2 ugyan olyan játékos!", this.Name, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    combo.SelectedItem = null;
                }
            }
        }
    }
}
