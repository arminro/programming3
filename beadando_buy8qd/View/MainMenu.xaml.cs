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

        public MainMenu()
        {
            this.InitializeComponent();
            this.bl = new BL();
            this.bl.CloseOpenWindows += (object sender, EventArgs eve) =>
             {
                 this.Close();
             };
        }

        public BL BL
        {
            get
            {
                return this.bl;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle((ImageBrush)this.Resources["paper"], null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.DrawRectangle((ImageBrush)this.Resources["ribbon"], null, new Rect(30, this.ActualHeight - 230, 160, 200));
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewGameWindow n = new NewGameWindow(this.bl);
            n.ShowDialog();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Quit();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            this.bl.Peek();
            LoadWindow load = new LoadWindow(this.bl, this);
            load.ShowDialog();
        }
    }
}
