// <copyright file="MainWindow.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Beadando.View;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BL bl;

        private RenderedButton next;
        private RenderedButton roll;
        private RenderedButton options;

        // storing the chosen colors for the players, we could not use the standard resource dict for this as we want to use the same key as those used for the puppets themselves
        private Dictionary<string, SolidColorBrush> playerColors;

        public MainWindow(BL bl)
        {
            this.InitializeComponent();
            this.bl = bl;

            this.playerColors = new Dictionary<string, SolidColorBrush>
            {
                { "nik", Brushes.DodgerBlue },
                { "kando", Brushes.Gold },
                { "rejto", Brushes.LimeGreen }
            };

            // subscribing to connector events
            bl.Invalidate += (object sender, EventArgs eve) =>
            {
                this.InvalidateVisual();
            };
            bl.NotifyPlayer += (object sender, TransferEventArgs trans) =>
            {
                MessageBox.Show(trans.Load, $"Üzenet {bl.Player.Name} számára", MessageBoxButton.OK, MessageBoxImage.Information);
            };

            bl.GeneralNotification += (object sender, TransferEventArgs trans) =>
            {
                MessageBox.Show(trans.Load, "Általános Üzenet", MessageBoxButton.OK, MessageBoxImage.Information);
            };
            bl.EventCard += (object s, CardEventArgs eventArgs) =>
            {
                EventViewer ev = new EventViewer(eventArgs.CardTypeKey, bl, this.playerColors);
                bool closedWithPositive = (bool)ev.ShowDialog();
                if (closedWithPositive == true)
                {
                    // if the player wanted to buy a new subject, we show the subject dialog
                    if (eventArgs.CardTypeKey == "enroll")
                    {
                        bl.InitializeSubjectTransactions(false, false);
                    }
                }
            };

            bl.InitiateSubjectTransaction += (object sender, SubjectEventArgs subEve) =>
            {
                SubjectWindow subw = new SubjectWindow(bl, subEve.IsSubjectFree);
                subw.Background = this.playerColors[bl.Player.PuppetKey];
                subw.ShowDialog();

                // if (subw.ShowDialog() == true)
                // {
                    // InvalidateVisual();
                // }
            };

            bl.FinishedGame += (object s, EventArgs e) =>
            {
                WinWindow w = new WinWindow(bl, this);
                w.ShowDialog();
            };

            // how many elements the row is designed to hold

            // resourceNamesNormal = new string[] { "go", "event", "roll", "enroll", "uv", "randi", "neptun"};
            // resourceNamesSquare = new string[] { "megajanlott", "lead", "einstein" };
        }

       // protected override void OnKeyUp(KeyEventArgs e)
       // {
       //     //used for testing purposes ONLY
       //     base.OnKeyUp(e);
       //     if (e.Key == Key.Tab)
       //     {
       //         //bl.GoToPosition(bl.Rand.Next(0, bl.GameBoard.Count));
       //         bl.GoToPosition(2); //TESTING PURPOSES
       //         //bl.TakeStep(bl.Rand.Next(1, 7));
       //     }
       // }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Left:
                    this.bl.MoveHorizontally(this.bl.MovementSpeed);
                    this.InvalidateVisual();
                    this.bl.SetMetrics();
                    break;
                case Key.Right:
                    this.bl.MoveHorizontally(-this.bl.MovementSpeed);
                    this.InvalidateVisual();
                    this.bl.SetMetrics();
                    break;
                case Key.Up:
                    this.bl.MoveVertically(this.bl.MovementSpeed);
                    this.InvalidateVisual();
                    this.bl.SetMetrics();
                    break;
                case Key.Down:
                    this.bl.MoveVertically(-this.bl.MovementSpeed);
                    this.InvalidateVisual();
                    this.bl.SetMetrics();
                    break;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            // mySolidColorBrush.Color = Colors.LimeGreen;
            base.OnRender(drawingContext);

            // updating the start card center
            this.bl.GameBoard[0].Rect = this.bl.CalculatePrimaryPosition(new Point(this.bl.Met.StartPosition.X, this.bl.StartY), this.bl.Met.SquareCardMetric, this.bl.Met.SquareCardMetric);

            int indexer = 1;
            Pen myPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 3);

            // BACKGROUND --> ALWAYS RENDERED FIRST
            Rect myRect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            drawingContext.DrawRectangle((ImageBrush)this.Resources["wood"], null, myRect);

            // TODO background of the gameboard -->the lower lign does not go with the rest of the board
            Rect backgroundRect = new Rect((this.ActualWidth / 2) - (this.bl.Met.WidthOfBoard / 2) + this.bl.OffsetHorizontal, (this.ActualHeight / 2) - (this.bl.Met.HeightOfBoard / 2) +
                this.bl.OffsetVertical, this.bl.Met.WidthOfBoard, this.bl.Met.HeightOfBoard);

            drawingContext.DrawRectangle(Brushes.Blue, null, backgroundRect);

            this.DrawBoard(drawingContext, myRect, indexer, myPen);

            // PUPPETS --> RENDERED AFTER THE TRACK

            // the view does not know about Player type, just sees the data from bl
            for (int i = 0; i < this.bl.Players.Count; i++)
            {
                drawingContext.DrawEllipse((ImageBrush)this.Resources[this.bl.Players[i].PuppetKey], null, this.bl.Players[i].Currentposition, this.bl.PuppetDiameter, this.bl.PuppetDiameter);
            }

            // PLAYER UI
            Size playerUiSize = new Size(550, 450);
            Point startPoint = new Point((this.ActualWidth / 2) - (playerUiSize.Width / 2) + this.bl.OffsetHorizontal, ((this.ActualHeight / 2) - (playerUiSize.Height / 2)) + this.bl.OffsetVertical);
            Rect mainPlayerScreen = new Rect(startPoint, playerUiSize);

            drawingContext.DrawRoundedRectangle((ImageBrush)this.Resources["phone"], null, mainPlayerScreen, 5, 5);

            this.DrawPlayerUI(drawingContext, startPoint);

            RenderedTextblock rounds = new RenderedTextblock(this.playerColors[this.bl.Player.PuppetKey], Brushes.White, drawingContext, new Rect(this.ActualWidth - 120, this.ActualHeight - 270, 100, 40));
            rounds.DrawText(drawingContext, this.bl.RoundCounter.ToString());

            // drawing the block
            RenderedTextblock block = new RenderedTextblock(this.playerColors[this.bl.Player.PuppetKey], Brushes.White, drawingContext, new Rect(this.ActualWidth - 120, this.ActualHeight - 220, 100, 40));
            if (this.bl.RandomGeneratedNumber > 0)
            {
                block.DrawText(drawingContext, this.bl.RandomGeneratedNumber.ToString());
            }

            // drawing the roll button
            this.roll = new RenderedButton(drawingContext, new Rect(this.ActualWidth - 120, this.ActualHeight - 170, 100, 40), "Gurítok", this.playerColors[this.bl.Player.PuppetKey], Brushes.White, this.bl.RollButtonEnabled, 20);
            this.roll.Click += (object s, EventArgs e) =>
            {
                this.bl.GenerateRandomNumber();
                this.bl.GoToPosition(this.bl.RandomGeneratedNumber);
                this.bl.RollButtonEnabled = false; // we make the button uninteractable
            };

            // next player
            this.next = new RenderedButton(drawingContext, new Rect(this.ActualWidth - 120, this.ActualHeight - 120, 100, 40), "Következő", this.playerColors[this.bl.Player.PuppetKey], Brushes.White, true, 20);
            this.next.Click += (object sender, EventArgs eve) =>
            {
                this.bl.NextRound();
                this.InvalidateVisual();
            };

            this.options = new RenderedButton(drawingContext, new Rect(this.ActualWidth - 90, this.ActualHeight - 70, 50, 50), string.Empty, (ImageBrush)this.Resources["options"], Brushes.White, true, 20, false);
            this.options.Click += (object senderOptions, EventArgs eveOptions) =>
            {
                OptionsWindow opt = new OptionsWindow(this.bl, this);
                opt.ShowDialog();
            };
        }

        // the unfinished zoom would work on mousewheel, it actually did, but needed some polishing it did not have the time to do
        // private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        // {
        //    bl.Zoom(e.Delta/60);
        //    InvalidateVisual();
        // }
        private void DrawPlayerUI(DrawingContext drawingContext, Point uiRect)
        {
            Brush brush = this.playerColors[this.bl.Player.PuppetKey];
            FormattedText playerName = new FormattedText(
               this.bl.Player.Name,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                18,
                brush);

            drawingContext.DrawText(playerName, new Point(uiRect.X + 100, uiRect.Y + 210));

            FormattedText playerMoney = new FormattedText(
               this.bl.Player.Money.ToString(),
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                70,
                brush);

            drawingContext.DrawText(playerMoney, new Point(uiRect.X + 200, uiRect.Y + 260));
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // we have to check every button if it was  pressed
            // tried it with adding these elements ot a list and looping over it, but this does not work that way
            // (maybe because how preview is propagated)
            this.next.CheckIfPressed(e.GetPosition(this));
            this.roll.CheckIfPressed(e.GetPosition(this));
            this.options.CheckIfPressed(e.GetPosition(this));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.bl.IsTheGameStillOn() && MessageBox.Show(
                "Mentesz kilépés előtt? Ha nem, elveszik amit elértél",
                "Kilépsz?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                this.bl.Save();
            }
        }

        /// <summary>
        /// Draws the images of the board. It uses the images defined in the WPF Resource Dictionary reading the entries stored in the Dictionary GameBoard of BL.
        /// </summary>
        /// <param name="dc">Drawing Context</param>
        /// <param name="myRect">Rectangle to hold position info</param>
        /// <param name="indexer">This iterates through the Dictionary GameBoard of BL</param>
        /// <param name="myPen">A pen used to draw the edges of the card images</param>
        private void DrawBoard(DrawingContext dc, Rect myRect, int indexer, Pen myPen)
        {
            // THE TRACK --> ALWAYS RENDERED AFTER THE BACKGROUND
            // start column
            ImageBrush brush;
            myRect = new Rect(this.bl.Met.StartPosition.X, this.bl.StartY, this.bl.Met.SquareCardMetric, this.bl.Met.SquareCardMetric);
            dc.DrawRectangle((ImageBrush)this.Resources["start"], myPen, myRect);

            // lower horizontal part
            int changingWidth = this.bl.Met.NormalCardWidth;
            int correction = 0;
            Point holder; // this holds the calcuated points
            for (int i = 1; i <= this.bl.Met.NumberOfElementsInAHorizontalRow; i++)
            {
                // if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[this.bl.GameBoard[indexer].ImageKey];
                if (i % this.bl.Met.NumberOfElementsInAHorizontalRow == 0)
                {
                    changingWidth = this.bl.Met.SquareCardMetric;

                    // we have to put the square card a little further, because it is wider
                    correction = this.bl.Met.SquareCardMetric - this.bl.Met.NormalCardWidth;
                }

                holder = new Point(this.bl.Met.StartPosition.X - (i * this.bl.Met.NormalCardWidth) - correction, this.bl.StartY);
                this.bl.GameBoard[indexer++].Rect = this.bl.CalculatePrimaryPosition(holder, changingWidth, this.bl.Met.NormalCardHeight);
                myRect = new Rect(holder.X, holder.Y, changingWidth, this.bl.Met.NormalCardHeight);
                dc.DrawRectangle(brush, myPen, myRect);
            }

            // left vertical part
            changingWidth = this.bl.Met.NormalCardWidth;
            correction = 0;

            /*The vertical parts are tricky, because we want to use images here that are tilted by 90° */
            for (int i = 1; i <= this.bl.Met.NumberOfElementsInAVerticalRow; i++)
            {
                // save the imagekey of the gameboard
                string temp = this.bl.GameBoard[indexer].ImageKey;

                // we check if the image is not among the square ones, as they do not have tilted alternatives
                if (this.bl.ResourceNamesSquare.Contains(temp))
                {
                    brush = (ImageBrush)this.Resources[temp];
                }
                else
                {
                    // the string[]s for both the normal and tilted images have the same order
                    // so we we can look for the index of temp in the string[] of normal images
                    // and use its index to access the name of the tilted image
                    int indexInHorizontal = Array.FindIndex(this.bl.ResourceNamesNormal, p => p == temp);
                    brush = (ImageBrush)this.Resources[this.bl.ResourceNamesHorizontal[indexInHorizontal]];
                }

                if (i % this.bl.Met.NumberOfElementsInAVerticalRow == 0)
                {
                    changingWidth = this.bl.Met.SquareCardMetric;

                    // we have to put the square card a little further, because it is wider
                    correction = this.bl.Met.SquareCardMetric - this.bl.Met.NormalCardWidth;
                }

                holder = new Point(this.bl.LeftVerticalAlign, (this.bl.StartY - (i * this.bl.Met.NormalCardWidth)) - correction);
                this.bl.GameBoard[indexer++].Rect = this.bl.CalculatePrimaryPosition(holder, this.bl.Met.NormalCardHeight, changingWidth);
                myRect = new Rect(holder.X, holder.Y, this.bl.Met.NormalCardHeight, changingWidth);

                dc.DrawRectangle(brush, myPen, myRect);
            }

            // upper horizontal part
            changingWidth = this.bl.Met.NormalCardWidth;
            correction = 0;
            for (int i = 1; i <= this.bl.Met.NumberOfElementsInAHorizontalRow; i++)
            {
                // if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[this.bl.GameBoard[indexer].ImageKey];
                if (i % this.bl.Met.NumberOfElementsInAHorizontalRow == 0)
                {
                    changingWidth = this.bl.Met.SquareCardMetric;

                    // we have to put the square card a little further, because it is wider
                    correction = this.bl.Met.SquareCardMetric - this.bl.Met.NormalCardWidth;
                }

                holder = new Point(
                    (this.bl.LeftVerticalAlign + (i * this.bl.Met.NormalCardWidth))
                    + this.bl.Met.SquareCardMetric - this.bl.Met.NormalCardWidth, this.bl.UpperVerticalAlign);
                this.bl.GameBoard[indexer++].Rect = this.bl.CalculatePrimaryPosition(holder, changingWidth, this.bl.Met.NormalCardHeight);

                myRect = new Rect(holder.X, holder.Y, changingWidth, this.bl.Met.NormalCardHeight);
                dc.DrawRectangle(brush, myPen, myRect);
            }

            changingWidth = this.bl.Met.NormalCardWidth;
            correction = 0;
            for (int i = 1; i <= this.bl.Met.NumberOfElementsInAVerticalRow - 1; i++)
            {
                // save the imagekey of the gameboard
                string temp = this.bl.GameBoard[indexer].ImageKey;

                // we check if the image is not among the square ones, as they do not have tilted alternatives
                if (this.bl.ResourceNamesSquare.Contains(temp))
                {
                    brush = (ImageBrush)this.Resources[temp];
                }
                else
                {
                    // the string[]s for both the normal and tilted images have the same order
                    // so we we can look for the index of temp in the string[] of normal images
                    // and use its index to access the name of the tilted image
                    int indexInHorizontal = Array.FindIndex(this.bl.ResourceNamesNormal, p => p == temp);
                    brush = (ImageBrush)this.Resources[this.bl.ResourceNamesHorizontal[indexInHorizontal]];
                }

                holder = new Point(
                    this.bl.RightVerticalAlign,
                      (this.bl.StartY - (this.bl.Met.NumberOfElementsInAVerticalRow * this.bl.Met.NormalCardWidth)) + (i * this.bl.Met.NormalCardWidth));
                this.bl.GameBoard[indexer++].Rect = this.bl.CalculatePrimaryPosition(holder, this.bl.Met.NormalCardHeight, changingWidth);

                myRect = new Rect(holder.X, holder.Y, this.bl.Met.NormalCardHeight, changingWidth);
                dc.DrawRectangle(brush, myPen, myRect);
            }
        }
    }
}
