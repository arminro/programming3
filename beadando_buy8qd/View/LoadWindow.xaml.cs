﻿using Beadando.ViewModel;
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

namespace Beadando.View
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        BL bl;
        Window invokedIn;
        public LoadWindow(BL bl, Window invokedIn)
        {
            InitializeComponent();
            this.bl = bl;
            this.DataContext = bl;
            this.invokedIn = invokedIn;
            //subscribing to the event in case something goes wrong in the bl controlling this part
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
            //we only close the windows, if the load was successful
            if (bl.Load(bl.SelectedPath))
            {
                bl.InitializeGame();
                this.DialogResult = true;
                MainWindow main = new MainWindow(bl);
                App.Current.MainWindow = main;
                this.Close();
                invokedIn.Close();
                //bl.CloseWindows();
                main.Show();

            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            bl.DeleteSave(bl.SelectedPath);
        }
    }
}
