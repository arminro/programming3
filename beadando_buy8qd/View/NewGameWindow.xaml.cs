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

        public NewGameWindow(BL bl)
        {
            this.InitializeComponent();
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
            this.bl.InitializeGame();
            this.bl.InitializeStartOfNewGame();
            this.DialogResult = true;
            MainWindow main = new MainWindow(this.bl);
            Application.Current.MainWindow = main;
            this.bl.CloseWindows();
            main.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Players.Clear();
            this.DialogResult = false;
        }

        private void NewPlayer_Click(object sender, RoutedEventArgs e)
        {
            this.bl.AddPlayer();
        }

        private void DeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            this.bl.DeletePlayer(this.bl.NewGameSelectedPlayer);
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
                    MessageBox.Show("Nem lehet 2 ugyan olyan játékos!", "Már van ilyen játékos", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    combo.SelectedItem = null;
                }
            }
        }
    }
}
