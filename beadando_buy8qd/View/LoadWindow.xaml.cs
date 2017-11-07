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

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadWindow"/> class.
        /// </summary>
        /// <param name="bl">reference to the business logic instance</param>
        /// <param name="invokedIn">reference to the window the load window is invoked in</param>
        public LoadWindow(BL bl, Window invokedIn)
        {
            this.InitializeComponent();
            this.bl = bl;
            this.DataContext = bl;
            this.invokedIn = invokedIn;

            // subscribing to the event in case something goes wrong in the bl controlling this part
            bl.LoadNotification += (object s, TransferEventArgs trans) =>
            {
                MessageBox.Show(trans.Load, this.Name);
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

            e.Handled = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            e.Handled = true; // setting the handled property so that it won't fire another time
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            this.bl.DeleteSave(this.bl.SelectedPath);
            e.Handled = true; // setting the handled property so that it won't fire another time
        }
    }
}
