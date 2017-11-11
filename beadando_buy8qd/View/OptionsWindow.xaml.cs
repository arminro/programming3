// <copyright file="OptionsWindow.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System.Windows;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private BL bl;
        private MainWindow mainW;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsWindow"/> class.
        /// </summary>
        /// <param name="bl">reference to a business logic instance</param>
        /// <param name="main">reference to the main window</param>
        public OptionsWindow(BL bl, MainWindow main)
        {
            this.InitializeComponent();
            this.bl = bl;
            this.mainW = main;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Save();
            MessageBox.Show("Játékállásodat mentettük", string.Empty, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            e.Handled = true;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Peek();
            LoadWindow load = new LoadWindow(this.bl, this);
            if (load.ShowDialog() == true)
            {
                this.CloseWindows();
            }

            e.Handled = true;
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            // we set the mainwindow as the main menu
            MainMenu main = new MainMenu();
            Application.Current.MainWindow = main;
            this.CloseWindows();

            // we show the main menu
            main.ShowDialog();
            e.Handled = true;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Quit();
            e.Handled = true;
        }

        private void CloseWindows()
        {
            // we close both the options menu and the main window
            this.Close();
            this.mainW.Close();
        }

        private void Rules_Click(object sender, RoutedEventArgs e)
        {
            this.bl.SetTutorialText();
            RulesWindow r = new RulesWindow(this.bl);
            r.ShowDialog();
        }
    }
}
