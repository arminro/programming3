// <copyright file="RulesWindow.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
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

    /// <summary>
    /// Interaction logic for RulesWindow.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RulesWindow"/> class.
        /// The window showing the rules of the game
        /// </summary>
        /// <param name="bl">Reference to the business logic instance of the main window</param>
        public RulesWindow(BL bl)
        {
            this.InitializeComponent();
            this.DataContext = bl;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
