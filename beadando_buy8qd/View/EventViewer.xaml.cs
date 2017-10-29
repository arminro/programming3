using Beadando.Model;
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
        RenderedButton rbuttonConfirm;
        RenderedButton rbuttonDeny;
        string keyOfCurrentCard;
        Brush background;
        int fontSize;
        string textToBeDisplayed;

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
                    background = new SolidColorBrush(Colors.ForestGreen);
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
            Tuple<string, int> tempTup = bl.GetTextToDisplay(keyOfCurrentCard);
            textToBeDisplayed = tempTup.Item1;
            fontSize = tempTup.Item2;
           

        }

        protected override void OnRender(DrawingContext dc)
        {
            //draws the background
            dc.DrawRectangle(background, null, new Rect(0, 0, ActualWidth, ActualHeight));
        
            //draws the eventcard
            Rect drawingRect = new Rect(offset, offset, ActualWidth-(offset*2), ActualHeight-200);

            //used the same keys, becuase it is a different window
            //if the start triggered the event, we show the oe image
            dc.DrawRoundedRectangle((ImageBrush)Resources[keyOfCurrentCard == "start" ? 
               "oe" : keyOfCurrentCard], null, drawingRect, 5, 5);
        
            //draw textbox
            Rect textboxRect = new Rect(ActualWidth/2 - textBoxWidth/2, (ActualHeight - 200) + textBoxHeight/6, textBoxWidth, textBoxHeight);
            dc.DrawRoundedRectangle(Brushes.White, null, textboxRect, 5, 5);
        
        
            FormattedText eventText = new FormattedText(
              textToBeDisplayed,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                fontSize, Brushes.Black);
            
            dc.DrawText(eventText, new Point(textboxRect.X+5, textboxRect.Y+5));
            Rect confirmButtonRect = new Rect(ActualWidth / 2 - 50, ActualHeight - 50, 100, 30);

            //if the player arrived on an enroll card, we have to display a slightly different layout
            if (keyOfCurrentCard == "enroll")
            {
                confirmButtonRect = new Rect(ActualWidth / 2 - 100, ActualHeight - 50, 100, 30);
                rbuttonConfirm = new RenderedButton(dc, confirmButtonRect, "Rendben",
                Brushes.White, Brushes.Black);

                rbuttonDeny = new RenderedButton(dc, new Rect(ActualWidth / 2+10, ActualHeight-50, 100, 30), 
                    "Mégsem", Brushes.White, Brushes.Black);
                rbuttonDeny.Click += (object sender, EventArgs e) =>
                {
                    this.DialogResult = false;
                };

                rbuttonConfirm.Click += RbuttonConfirmEnroll_Click;
            }

            else
            {
                rbuttonConfirm = new RenderedButton(dc, confirmButtonRect, "Rendben",
                Brushes.White, Brushes.Black);
                rbuttonConfirm.Click += RbuttonConfirm_Click;
            }
            
            

        
        }

        private void RbuttonConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            
        }

        private void RbuttonConfirmEnroll_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            bl.InitializeSubjectTransactions();
            
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rbuttonConfirm.CheckIfPressed(e.GetPosition(this));
            if(rbuttonDeny != null)
            {
                rbuttonDeny.CheckIfPressed(e.GetPosition(this));
            }
            
        }
    }


}
