using Beadando.Model;
using Beadando.ViewModel;
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
    /// Interaction logic for EventViewer.xaml
    /// </summary>
    public partial class EventViewer : Window
    {

        /// <summary>
        /// Displays the pop-up events
        /// </summary>
        /// <param name="cardKey">The key of the card that triggered the event</param>
        /// 
        int offset; //the thickness of the frame around the card
        int textBoxWidth; // width of the textbox
        int textBoxHeight;
        BL bl;
        RenderedButton rbutton;
        string keyOfCurrentCard;
        Brush background;
        public EventViewer(string cardKey)
        {
            InitializeComponent();
            offset = 10;
            textBoxWidth = 350;
            textBoxHeight = 120;
            keyOfCurrentCard = cardKey;
            
            bl = new BL();

            //determine the background of the window based on the type of card the player stands
            switch (bl.EventCategroySelector(keyOfCurrentCard))
            {
                case "green":
                    background = new SolidColorBrush(Colors.LawnGreen);
                    break;
                case "blue":
                    background = new SolidColorBrush(Colors.RoyalBlue);
                    break;
                case "black":
                    background = new SolidColorBrush(Colors.Black);
                    break;
                case "yellow":
                    background = new SolidColorBrush(Colors.Yellow);
                    break;
            }

          
           

        }

        protected override void OnRender(DrawingContext dc)
        {
            //draws the background
            dc.DrawRectangle(background, null, new Rect(0, 0, ActualWidth, ActualHeight));
        
            //draws the eventcard
            Rect drawingRect = new Rect(offset, offset, ActualWidth-(offset*2), ActualHeight-200);
            dc.DrawRoundedRectangle(Brushes.Blue, null, drawingRect, 5, 5);
        
            //draw textbox
            Rect textboxRect = new Rect(ActualWidth/2 - textBoxWidth/2, (ActualHeight - 200) + textBoxHeight/6, textBoxWidth, textBoxHeight);
            dc.DrawRoundedRectangle(Brushes.White, null, textboxRect, 5, 5);
        
        
            //FormattedText eventText = new FormattedText(
            //   bl.Player.Name,
            //    CultureInfo.CurrentUICulture,
            //    FlowDirection.LeftToRight,
            //    new Typeface("Impact"),
            //    18, Brush);
            //
            //dc.DrawText(eventText, new Point((uiRect.X + 100), (uiRect.Y + 210)));
            rbutton = new RenderedButton(dc, new Rect(ActualWidth / 2 - 50, ActualHeight - 50, 100, 30), "Rendben",
                Brushes.White, Brushes.Black);
            rbutton.Click += (object sender, EventArgs e) =>
            {
                //TODO call for some bl logic
            };
        
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rbutton.CheckIfPressed(e.GetPosition(this));
            this.DialogResult = true;
        }
    }


}
