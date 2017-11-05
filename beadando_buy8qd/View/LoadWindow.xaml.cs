// <copyright file="LoadWindow.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System;
    using System.Windows;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        private BL bl;
        private Window invokedIn;

        public LoadWindow(BL bl, Window invokedIn)
        {
            this.InitializeComponent();
            this.bl = bl;
            this.DataContext = bl;
            this.invokedIn = invokedIn;

            // subscribing to the event in case something goes wrong in the bl controlling this part
            bl.GeneralNotification += (object s, TransferEventArgs trans) =>
            {
                MessageBox.Show(trans.Load);
            };
            bl.CloseOpenWindows += (object s, EventArgs e) =>
            {
                this.Close();
            };
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            // we only close the windows, if the load was successful
            if (this.bl.Load(this.bl.SelectedPath))
            {
                this.bl.InitializeGame();
                this.DialogResult = true;
                MainWindow main = new MainWindow(this.bl);
                Application.Current.MainWindow = main;
                this.Close();
                this.invokedIn.Close();

                // bl.CloseWindows();
                main.Show();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            this.bl.DeleteSave(this.bl.SelectedPath);
        }
    }
}
