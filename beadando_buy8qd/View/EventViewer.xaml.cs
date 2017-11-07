// <copyright file="EventViewer.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for EventViewer.xaml
    /// </summary>
    public partial class EventViewer : Window
    {
        /// <summary>
        /// Displays the pop-up events
        /// </summary>
        private int offset; // the thickness of the frame around the card
        private int textBoxWidth; // width of the textbox
        private int textBoxHeight;
        private BL bl;
        private RenderedButton rbuttonConfirm;
        private RenderedButton rbuttonDeny;
        private string keyOfCurrentCard;
        private Brush background;
        private int fontSize;
        private string textToBeDisplayed;
        private Dictionary<string, SolidColorBrush> playerColors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventViewer"/> class.
        /// </summary>
        /// <param name="cardKey">the key of a gameboard card</param>
        /// <param name="bl">reference to a business logic instance</param>
        /// <param name="playerColors">reference to a list containing the colors chosen to represent diff type of players</param>
        public EventViewer(string cardKey, BL bl, Dictionary<string, SolidColorBrush> playerColors)
        {
            this.InitializeComponent();
            this.offset = 10;
            this.textBoxWidth = 350;
            this.textBoxHeight = 120;
            this.keyOfCurrentCard = cardKey;
            this.bl = bl;
            this.playerColors = playerColors;

            // determine the background of the window based on the type of card the player stands
            switch (bl.EventCategroySelector(this.keyOfCurrentCard))
            {
                case "green":
                    this.background = new SolidColorBrush(Colors.ForestGreen);
                    break;
                case "blue":
                    this.background = new SolidColorBrush(Colors.RoyalBlue);
                    break;
                case "black":
                    this.background = new SolidColorBrush(Colors.Black);
                    break;
                case "yellow":
                    this.background = new SolidColorBrush(Colors.Yellow);
                    break;
            }

            Tuple<string, int, int?> tempTup = bl.GetTextToDisplay(this.keyOfCurrentCard);
            this.textToBeDisplayed = tempTup.Item1;
            this.fontSize = tempTup.Item2;
            bl.IndexOfEventCardCollection = tempTup.Item3;
        }

        /// <summary>
        /// Renders the image invoked when a player steps on a card
        /// </summary>
        /// <param name="drawingContext">The drawing context provided by the framwork element</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            // draws the background
            drawingContext.DrawRectangle(this.background, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            // draws the eventcard
            Rect drawingRect = new Rect(this.offset, this.offset, this.ActualWidth - (this.offset * 2), this.ActualHeight - 200);

            // used the same keys, becuase it is a different window
            // the event has different pics
            // if the start triggered the event, we show the oe image
            if (this.keyOfCurrentCard == "event")
            {
                // we use the mapper to map the given int to a source in the resource dictionary
                drawingContext.DrawRoundedRectangle(
                    (ImageBrush)this.Resources[this.bl.EventMapper[(int)this.bl.IndexOfEventCardCollection]], null, drawingRect, 5, 5);
            }
            else
            {
                drawingContext.DrawRoundedRectangle(
                    (ImageBrush)this.Resources[this.keyOfCurrentCard == "start" ?
                      "oe" : this.keyOfCurrentCard],
                    null,
                    drawingRect,
                    5,
                    5);
            }

            // draw textbox
            Rect textboxRect = new Rect((this.ActualWidth / 2) - (this.textBoxWidth / 2), (this.ActualHeight - 200) + (this.textBoxHeight / 6), this.textBoxWidth, this.textBoxHeight);
            drawingContext.DrawRoundedRectangle(Brushes.White, null, textboxRect, 5, 5);

            FormattedText eventText = new FormattedText(
              this.textToBeDisplayed,
              CultureInfo.CurrentUICulture,
              FlowDirection.LeftToRight,
              new Typeface("Impact"),
              this.fontSize,
              Brushes.Black);

            drawingContext.DrawText(eventText, new Point(textboxRect.X + 5, textboxRect.Y + 5));
            Rect confirmButtonRect = new Rect((this.ActualWidth / 2) - 50, this.ActualHeight - 50, 100, 30);

            // if the player arrived on an enroll card, we have to display a slightly different layout
            // this is done by relocating the confirm button, adding a new deny button and subscribing to a diff event
            // so if we have the enroll, we go with this
            if (this.keyOfCurrentCard == "enroll")
            {
                // confirm button needs a new place
                confirmButtonRect = new Rect((this.ActualWidth / 2) - 100, this.ActualHeight - 50, 100, 30);
                this.rbuttonConfirm = new RenderedButton(
                drawingContext,
                confirmButtonRect,
                "Rendben",
                Brushes.White,
                Brushes.Black,
                true,
                16);

                // denybutton
                this.rbuttonDeny = new RenderedButton(
                    drawingContext,
                    new Rect((this.ActualWidth / 2) + 10, this.ActualHeight - 50, 100, 30),
                    "Mégsem",
                    Brushes.White,
                    Brushes.Black,
                    true,
                    16);
                this.rbuttonDeny.Click += (object sender, EventArgs e) =>
                {
                    this.DialogResult = false;
                };

                this.rbuttonConfirm.Click += this.RbuttonConfirmEnroll_Click;
            }

            // this is the defualt scenario
            else
            {
                this.rbuttonConfirm = new RenderedButton(
                    drawingContext,
                    confirmButtonRect,
                    "Rendben",
                    Brushes.White,
                    Brushes.Black,
                    true,
                    16);
                this.rbuttonConfirm.Click += this.RbuttonConfirm_Click;
            }
        }

        private void RbuttonConfirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            /*there are some event that require parameters, they have different sections
             others can be stored in two diff collections: 1) dictioanary: those that are mapped to a single string in Constants,
             for these, the IndexOfEventCardCollection will be null
             2) array of Tasks: those that are mapped to a string[], for these, the IndexOfEventCardCollection will hold the index of the chosen*/
            if (this.keyOfCurrentCard == "go")
            {
                // the player we go to an extra distance
                // we have to get the number from the text which is the last character
                // then the player steps forward
                this.bl.GoToPosition(this.bl.GetPositionFromGostring(this.textToBeDisplayed));
            }
            else if (this.keyOfCurrentCard == "event")
            {
                // if the player can get a new course for free
                if (this.bl.IsFreeCourseValid(this.bl.IndexOfEventCardCollection))
                {
                    this.bl.InitializeSubjectTransactions(true, false);
                    SubjectWindow subw = new SubjectWindow(this.bl, true)
                    {
                        Background = this.playerColors[this.bl.Player.PuppetKey]
                    };
                    if (subw.ShowDialog() == true)
                    {
                        this.InvalidateVisual();
                        this.bl.IndexOfEventCardCollection = null; // we have to set the var back to null
                    }
                }
                else if (this.bl.IndexOfEventCardCollection != null && this.bl.IndexOfEventCardCollection == 3)
                {
                    // the tökös legény event is the 4th one
                    this.bl.ArriveAtRandomPosition();
                    this.bl.Refresh();
                    this.bl.IndexOfEventCardCollection = null;
                }
            }

            // we index the dict with the single elements
            else if (this.bl.IndexOfEventCardCollection == null)
            {
                this.bl.EventActions_Single[this.keyOfCurrentCard].Invoke();
            }

            // we index the array
            else
            {
                this.bl.NeptunActions[(int)this.bl.IndexOfEventCardCollection].Invoke();
            }
        }

        private void RbuttonConfirmEnroll_Click(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.bl.InitializeSubjectTransactions(false, false);
            SubjectWindow subw = new SubjectWindow(this.bl, false)
            {
                Background = this.playerColors[this.bl.Player.PuppetKey]
            };
            if (subw.ShowDialog() == true)
            {
                this.bl.Refresh(); // make the main window refresh
                this.bl.IndexOfEventCardCollection = null; // we have to set the var back to null
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.rbuttonConfirm.CheckIfPressed(e.GetPosition(this));
            if (this.rbuttonDeny != null)
            {
                this.rbuttonDeny.CheckIfPressed(e.GetPosition(this));
            }

            e.Handled = true;
        }
    }
}
