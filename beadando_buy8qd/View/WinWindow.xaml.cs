// <copyright file="WinWindow.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System.Windows;
    using System.Windows.Media;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for WinWindow.xaml
    /// </summary>
    public partial class WinWindow : Window
    {
        private BL bl;
        private MainWindow mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinWindow"/> class.
        /// </summary>
        /// <param name="bl">reference to the business logic</param>
        /// <param name="mainWindow">reference to the main window</param>
        public WinWindow(BL bl, MainWindow mainWindow)
        {
            this.InitializeComponent();
            this.bl = bl;
            this.DataContext = bl;
            this.mainWindow = mainWindow;
        }

        /// <summary>
        /// renders the background of the window
        /// </summary>
        /// <param name="drawingContext">reference to the drawing context</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle((ImageBrush)this.Resources["win"], null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainMenu main = new MainMenu();
            Application.Current.MainWindow = main;

            // bl.StartGameNow();
            this.Close();
            this.mainWindow.Close();

            main.Show();
            e.Handled = true;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Quit();
            e.Handled = true;
        }
    }
}
