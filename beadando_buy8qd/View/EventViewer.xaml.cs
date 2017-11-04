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
        Dictionary<string, SolidColorBrush> playerColors;



        public EventViewer(string cardKey, BL bl, Dictionary<string, SolidColorBrush> playerColors)
        {
            InitializeComponent();
            offset = 10;
            textBoxWidth = 350;
            textBoxHeight = 120;
            keyOfCurrentCard = cardKey;
            this.bl = bl;
            this.playerColors = playerColors;

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
            Tuple<string, int, int?> tempTup = bl.GetTextToDisplay(keyOfCurrentCard);
            textToBeDisplayed = tempTup.Item1;
            fontSize = tempTup.Item2;
            bl.IndexOfEventCardCollection = tempTup.Item3;

        }

        protected override void OnRender(DrawingContext dc)
        {
            //draws the background
            dc.DrawRectangle(background, null, new Rect(0, 0, ActualWidth, ActualHeight));
        
            //draws the eventcard
            Rect drawingRect = new Rect(offset, offset, ActualWidth-(offset*2), ActualHeight-200);


            //used the same keys, becuase it is a different window
            //the event has different pics
            //if the start triggered the event, we show the oe image
            if(keyOfCurrentCard == "event")
            {
                //we use the mapper to map the given int to a source in the resource dictionary
                dc.DrawRoundedRectangle((ImageBrush)Resources[bl.EventMapper[(int)bl.IndexOfEventCardCollection]], 
                    null, drawingRect, 5, 5);
            }
            else
            {
                dc.DrawRoundedRectangle((ImageBrush)Resources[keyOfCurrentCard == "start" ?
                      "oe" : keyOfCurrentCard], null, drawingRect, 5, 5);
            }

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
            //this is done by relocating the confirm button, adding a new deny button and subscribing to a diff event
            //so if we have the enroll, we go with this
            if (keyOfCurrentCard == "enroll")
            {
                //confirm button needs a new place
                confirmButtonRect = new Rect(ActualWidth / 2 - 100, ActualHeight - 50, 100, 30);
                rbuttonConfirm = new RenderedButton(dc, confirmButtonRect, "Rendben",
                Brushes.White, Brushes.Black, true, 16);

                //denybutton
                rbuttonDeny = new RenderedButton(dc, new Rect(ActualWidth / 2+10, ActualHeight-50, 100, 30), 
                    "Mégsem", Brushes.White, Brushes.Black, true, 16);
                rbuttonDeny.Click += (object sender, EventArgs e) =>
                {
                    this.DialogResult = false;
                };

                rbuttonConfirm.Click += RbuttonConfirmEnroll_Click;
            }

            //this is the defualt scenario
            else
            {
                rbuttonConfirm = new RenderedButton(dc, confirmButtonRect, "Rendben",
                Brushes.White, Brushes.Black, true, 16);
                rbuttonConfirm.Click += RbuttonConfirm_Click;
            }
            
            

        
        }

        private void RbuttonConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            /*there are some event that require parameters, they have different sections
             others can be stored in two diff collections: 1) dictioanary: those that are mapped to a single string in Constants, 
             for these, the IndexOfEventCardCollection will be null
             2) array of Tasks: those that are mapped to a string[], for these, the IndexOfEventCardCollection will hold the index of the chosen*/
            if (keyOfCurrentCard == "go")
            {
                //the player we go to an extra distance
                //we have to get the number from the text which is the last character
                //then the player steps forward
                
                bl.GoToPosition(bl.GetPositionFromGostring(textToBeDisplayed));
            }
            else if (keyOfCurrentCard == "event")
            {
               
                //if the player can get a new course for free

                if (bl.IsFreeCourseValid(bl.IndexOfEventCardCollection))
                {
                    bl.InitializeSubjectTransactions(true, false);
                    SubjectWindow subw = new SubjectWindow(bl, true);
                    subw.Background = playerColors[bl.Player.PuppetKey];
                    if (subw.ShowDialog() == true)
                    {
                        InvalidateVisual();
                        bl.IndexOfEventCardCollection = null; //we have to set the var back to null
                    }

                }
                else if (bl.IndexOfEventCardCollection != null && bl.IndexOfEventCardCollection == 3)
                {
                    //the tökös legény event is the 4th one
                    bl.ArriveAtRandomPosition();
                    bl.Refresh();
                    bl.IndexOfEventCardCollection = null;
                }

            }
            //we index the dict with the single elements
            else if(bl.IndexOfEventCardCollection == null)
            {
                bl.EventActions_Single[keyOfCurrentCard].Invoke();

            }
            //we index the array 
            else
            {
                bl.NeptunActions[(int)bl.IndexOfEventCardCollection].Invoke(); 
            }




        }

        private void RbuttonConfirmEnroll_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            bl.InitializeSubjectTransactions(false, false);
            SubjectWindow subw = new SubjectWindow(bl, false);
            subw.Background = playerColors[bl.Player.PuppetKey];
            if (subw.ShowDialog() == true)
            {
                bl.Refresh(); //make the main window refresh
                bl.IndexOfEventCardCollection = null; //we have to set the var back to null
            }


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
