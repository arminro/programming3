// <copyright file="MainMenu.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private BL bl;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        {
            this.InitializeComponent();
            this.bl = new BL();
            this.bl.CloseOpenWindows += (object sender, EventArgs eve) =>
             {
                 this.Close();
             };
        }

        /// <summary>
        /// Gets the reference to the business logic instance
        /// </summary>
        public BL BL
        {
            get
            {
                return this.bl;
            }
        }

        /// <summary>
        /// Renders the background of the main menu
        /// </summary>
        /// <param name="drawingContext">an instance of the drawing context</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle((ImageBrush)this.Resources["paper"], null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.DrawRectangle((ImageBrush)this.Resources["ribbon"], null, new Rect(30, this.ActualHeight - 230, 160, 200));
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewGameWindow n = new NewGameWindow(this.bl);
            n.ShowDialog();
            e.Handled = true;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Quit();
            e.Handled = true;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Peek();
            LoadWindow load = new LoadWindow(this.bl, this);
            load.ShowDialog();
            e.Handled = true;
        }
    }
}
