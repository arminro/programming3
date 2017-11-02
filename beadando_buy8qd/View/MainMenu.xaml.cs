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

namespace Beadando.View
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        BL bl;
        public MainMenu()
        {
            InitializeComponent();
            bl = new BL();
            bl.CloseOpenWindows += (object sender, EventArgs eve) =>
             {
                 this.Close();
             };
        }

        public BL Bl
        {
            get
            {
                return bl;
            }

           
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle((ImageBrush)Resources["paper"], null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            drawingContext.DrawRectangle((ImageBrush)Resources["ribbon"],null, new Rect(30, this.ActualHeight - 230, 160, 200));
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewGameWindow n = new NewGameWindow(bl);
            n.ShowDialog();
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            bl.Quit();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
