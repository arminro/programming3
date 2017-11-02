using Beadando.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for WinWindow.xaml
    /// </summary>
    public partial class WinWindow : Window
    {
        BL bl;
        MainWindow mainWindow;
        public WinWindow(BL bl, MainWindow mainWindow)
        {
            InitializeComponent();
            this.bl = bl;
            this.DataContext = bl;
            this.mainWindow = mainWindow;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            drawingContext.DrawRectangle((ImageBrush)Resources["win"], null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            //FormattedText playerName = new FormattedText(
            //        $"GRATULÁLUNK, {bl.Winner} NYERTÉL!!!",
            //         CultureInfo.CurrentUICulture,
            //         FlowDirection.LeftToRight,
            //         new Typeface("Impact"),
            //         18, Brushes.Gold);
            //
            //drawingContext.DrawText(playerName, new Point(0, this.ActualHeight/2));
            //RenderedButton backToMain = new RenderedButton(drawingContext, new Rect(60, 150, 100, 70), "Vissza a Főmenűbe", Brushes.White, Brushes.Black);
            //RenderedButton quit = new RenderedButton(drawingContext, new Rect(this.ActualWidth- 60, 150, 100, 70), "Kilépés", Brushes.White, Brushes.Black);


        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            
            MainMenu main = new MainMenu();
            App.Current.MainWindow = main;
            //bl.StartGameNow();
            this.Close();
            mainWindow.Close();
            
            main.Show();
           

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            bl.Quit();
        }
    }
}
