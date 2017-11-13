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

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="bl">reference to the business logic instance</param>
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
                SubjectWindow subw = new SubjectWindow(bl, subEve.IsSubjectFree)
                {
                    Background = this.playerColors[bl.Player.PuppetKey]
                };
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
        }

        /// <summary>
        /// handles keystrokes: up, down, left, right
        /// </summary>
        /// <param name="e">info about the pressed key</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // stylecop made me do it
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
                case Key.None:
                    break;
                case Key.Cancel:
                    break;
                case Key.Back:
                    break;
                case Key.Tab:
                    break;
                case Key.LineFeed:
                    break;
                case Key.Clear:
                    break;
                case Key.Return:
                    break;
                case Key.Pause:
                    break;
                case Key.Capital:
                    break;
                case Key.KanaMode:
                    break;
                case Key.JunjaMode:
                    break;
                case Key.FinalMode:
                    break;
                case Key.HanjaMode:
                    break;
                case Key.Escape:
                    break;
                case Key.ImeConvert:
                    break;
                case Key.ImeNonConvert:
                    break;
                case Key.ImeAccept:
                    break;
                case Key.ImeModeChange:
                    break;
                case Key.Space:
                    break;
                case Key.Prior:
                    break;
                case Key.Next:
                    break;
                case Key.End:
                    break;
                case Key.Home:
                    break;
                case Key.Select:
                    break;
                case Key.Print:
                    break;
                case Key.Execute:
                    break;
                case Key.Snapshot:
                    break;
                case Key.Insert:
                    break;
                case Key.Delete:
                    break;
                case Key.Help:
                    break;
                case Key.D0:
                    break;
                case Key.D1:
                    break;
                case Key.D2:
                    break;
                case Key.D3:
                    break;
                case Key.D4:
                    break;
                case Key.D5:
                    break;
                case Key.D6:
                    break;
                case Key.D7:
                    break;
                case Key.D8:
                    break;
                case Key.D9:
                    break;
                case Key.A:
                    break;
                case Key.B:
                    break;
                case Key.C:
                    break;
                case Key.D:
                    break;
                case Key.E:
                    break;
                case Key.F:
                    break;
                case Key.G:
                    break;
                case Key.H:
                    break;
                case Key.I:
                    break;
                case Key.J:
                    break;
                case Key.K:
                    break;
                case Key.L:
                    break;
                case Key.M:
                    break;
                case Key.N:
                    break;
                case Key.O:
                    break;
                case Key.P:
                    break;
                case Key.Q:
                    break;
                case Key.R:
                    break;
                case Key.S:
                    break;
                case Key.T:
                    break;
                case Key.U:
                    break;
                case Key.V:
                    break;
                case Key.W:
                    break;
                case Key.X:
                    break;
                case Key.Y:
                    break;
                case Key.Z:
                    break;
                case Key.LWin:
                    break;
                case Key.RWin:
                    break;
                case Key.Apps:
                    break;
                case Key.Sleep:
                    break;
                case Key.NumPad0:
                    break;
                case Key.NumPad1:
                    break;
                case Key.NumPad2:
                    break;
                case Key.NumPad3:
                    break;
                case Key.NumPad4:
                    break;
                case Key.NumPad5:
                    break;
                case Key.NumPad6:
                    break;
                case Key.NumPad7:
                    break;
                case Key.NumPad8:
                    break;
                case Key.NumPad9:
                    break;
                case Key.Multiply:
                    break;
                case Key.Add:
                    break;
                case Key.Separator:
                    break;
                case Key.Subtract:
                    break;
                case Key.Decimal:
                    break;
                case Key.Divide:
                    break;
                case Key.F1:
                    break;
                case Key.F2:
                    break;
                case Key.F3:
                    break;
                case Key.F4:
                    break;
                case Key.F5:
                    break;
                case Key.F6:
                    break;
                case Key.F7:
                    break;
                case Key.F8:
                    break;
                case Key.F9:
                    break;
                case Key.F10:
                    break;
                case Key.F11:
                    break;
                case Key.F12:
                    break;
                case Key.F13:
                    break;
                case Key.F14:
                    break;
                case Key.F15:
                    break;
                case Key.F16:
                    break;
                case Key.F17:
                    break;
                case Key.F18:
                    break;
                case Key.F19:
                    break;
                case Key.F20:
                    break;
                case Key.F21:
                    break;
                case Key.F22:
                    break;
                case Key.F23:
                    break;
                case Key.F24:
                    break;
                case Key.NumLock:
                    break;
                case Key.Scroll:
                    break;
                case Key.LeftShift:
                    break;
                case Key.RightShift:
                    break;
                case Key.LeftCtrl:
                    break;
                case Key.RightCtrl:
                    break;
                case Key.LeftAlt:
                    break;
                case Key.RightAlt:
                    break;
                case Key.BrowserBack:
                    break;
                case Key.BrowserForward:
                    break;
                case Key.BrowserRefresh:
                    break;
                case Key.BrowserStop:
                    break;
                case Key.BrowserSearch:
                    break;
                case Key.BrowserFavorites:
                    break;
                case Key.BrowserHome:
                    break;
                case Key.VolumeMute:
                    break;
                case Key.VolumeDown:
                    break;
                case Key.VolumeUp:
                    break;
                case Key.MediaNextTrack:
                    break;
                case Key.MediaPreviousTrack:
                    break;
                case Key.MediaStop:
                    break;
                case Key.MediaPlayPause:
                    break;
                case Key.LaunchMail:
                    break;
                case Key.SelectMedia:
                    break;
                case Key.LaunchApplication1:
                    break;
                case Key.LaunchApplication2:
                    break;
                case Key.Oem1:
                    break;
                case Key.OemPlus:
                    break;
                case Key.OemComma:
                    break;
                case Key.OemMinus:
                    break;
                case Key.OemPeriod:
                    break;
                case Key.Oem2:
                    break;
                case Key.Oem3:
                    break;
                case Key.AbntC1:
                    break;
                case Key.AbntC2:
                    break;
                case Key.Oem4:
                    break;
                case Key.Oem5:
                    break;
                case Key.Oem6:
                    break;
                case Key.Oem7:
                    break;
                case Key.Oem8:
                    break;
                case Key.Oem102:
                    break;
                case Key.ImeProcessed:
                    break;
                case Key.System:
                    break;
                case Key.OemAttn:
                    break;
                case Key.OemFinish:
                    break;
                case Key.OemCopy:
                    break;
                case Key.OemAuto:
                    break;
                case Key.OemEnlw:
                    break;
                case Key.OemBackTab:
                    break;
                case Key.Attn:
                    break;
                case Key.CrSel:
                    break;
                case Key.ExSel:
                    break;
                case Key.EraseEof:
                    break;
                case Key.Play:
                    break;
                case Key.Zoom:
                    break;
                case Key.NoName:
                    break;
                case Key.Pa1:
                    break;
                case Key.OemClear:
                    break;
                case Key.DeadCharProcessed:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// renders the gaming area
        /// </summary>
        /// <param name="drawingContext">reference tp the drawing conext</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            // updating the start card center
            this.bl.GameBoard[0].Rect = this.bl.CalculatePrimaryPosition(new Point(this.bl.Met.StartPosition.X, this.bl.StartY), this.bl.Met.SquareCardMetric, this.bl.Met.SquareCardMetric);

            int indexer = 1;
            Pen myPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 3);

            this.RenderBackground(drawingContext);

            this.DrawBoard(drawingContext, indexer, myPen);

            this.DrawPlayers(drawingContext);

            this.DrawPlayerUI(drawingContext);

            this.DrawButtons(drawingContext);
        }

        private void DrawButtons(DrawingContext drawingContext)
        {
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
                this.bl.RollButtonEnabled = false; // we make the button uninteractable
                this.bl.GenerateRandomNumber();
                this.bl.GoToPosition(this.bl.RandomGeneratedNumber);

                // this.bl.GoToPosition(1); TESTING
            };

            // next player
            this.next = new RenderedButton(drawingContext, new Rect(this.ActualWidth - 120, this.ActualHeight - 120, 100, 40), "Következő", this.playerColors[this.bl.Player.PuppetKey], Brushes.White, this.bl.CanPlayerCallNextRound, 20);
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

        private void DrawPlayers(DrawingContext drawingContext)
        {
            // PUPPETS --> RENDERED AFTER THE TRACK
            // the view does not know about Player type, just sees the data from bl
            for (int i = 0; i < this.bl.Players.Count; i++)
            {
                drawingContext.DrawEllipse((ImageBrush)this.Resources[this.bl.Players[i].PuppetKey], null, this.bl.Players[i].Currentposition, this.bl.PuppetDiameter, this.bl.PuppetDiameter);
            }
        }

        // the unfinished zoom would work on mousewheel, it actually did, but needed some polishing it did not have the time to do
        // private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        // {
        //    bl.Zoom(e.Delta/60);
        //    InvalidateVisual();
        // }
        private void DrawPlayerUI(DrawingContext drawingContext)
        {
            // PLAYER UI
            Size playerUiSize = new Size(550, 450);
            Point startPoint = new Point((this.ActualWidth / 2) - (playerUiSize.Width / 2) + this.bl.OffsetHorizontal, ((this.ActualHeight / 2) - (playerUiSize.Height / 2)) + this.bl.OffsetVertical);
            Rect mainPlayerScreen = new Rect(startPoint, playerUiSize);

            drawingContext.DrawRoundedRectangle((ImageBrush)this.Resources["phone"], null, mainPlayerScreen, 5, 5);

            Brush brush = this.playerColors[this.bl.Player.PuppetKey];
            FormattedText playerName = new FormattedText(
               this.bl.Player.Name,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                18,
                brush);

            drawingContext.DrawText(playerName, new Point(startPoint.X + 100, startPoint.Y + 210));

            FormattedText playerMoney = new FormattedText(
               this.bl.Player.Money.ToString(),
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                70,
                brush);

            drawingContext.DrawText(playerMoney, new Point(startPoint.X + 200, startPoint.Y + 260));
        }

        private void RenderBackground(DrawingContext drawingContext)
        {
            // BACKGROUND --> ALWAYS RENDERED FIRST
            Rect myRect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            drawingContext.DrawRectangle((ImageBrush)this.Resources["wood"], null, myRect);

            Rect backgroundRect = new Rect(
                (this.ActualWidth / 2) - (this.bl.Met.WidthOfBoard / 2) + this.bl.OffsetHorizontal,
                (this.ActualHeight / 2) - (this.bl.Met.HeightOfBoard / 2) + this.bl.OffsetVertical,
                this.bl.Met.WidthOfBoard,
                this.bl.Met.HeightOfBoard);

            drawingContext.DrawRectangle(Brushes.Blue, null, backgroundRect);
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
        /// <param name="indexer">This iterates through the Dictionary GameBoard of BL</param>
        /// <param name="myPen">The pen used to draw the edges</param>
        private void DrawBoard(DrawingContext dc, int indexer, Pen myPen)
        {
            // THE TRACK --> ALWAYS RENDERED AFTER THE BACKGROUND
            // start column
            ImageBrush brush;

            Rect myRect = new Rect(this.bl.StartX, this.bl.StartY, this.bl.Met.SquareCardMetric, this.bl.Met.SquareCardMetric);

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

                holder = new Point(this.bl.StartX - (i * this.bl.Met.NormalCardWidth) - correction, this.bl.StartY);
                this.bl.GameBoard[indexer].Rect = this.bl.CalculatePrimaryPosition(holder, changingWidth, this.bl.Met.NormalCardHeight);
                myRect = new Rect(holder.X, holder.Y, changingWidth, this.bl.Met.NormalCardHeight);
                dc.DrawRectangle(brush, myPen, myRect);

                // drawing the number of the card, we treat square cards differently, as they do not have the number on their edges
                Vector numberVector;
                if (this.bl.CardIsSquare(indexer))
                {
                    numberVector = new Vector(-40, 60);
                }
                else
                {
                    numberVector = new Vector(0, 60);
                }

                this.DrawText(dc, Point.Subtract(this.bl.GameBoard[indexer].Rect, numberVector), indexer.ToString());
                indexer++;
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
                this.bl.GameBoard[indexer].Rect = this.bl.CalculatePrimaryPosition(holder, this.bl.Met.NormalCardHeight, changingWidth);
                myRect = new Rect(holder.X, holder.Y, this.bl.Met.NormalCardHeight, changingWidth);
                dc.DrawRectangle(brush, myPen, myRect);

                // drawing the number of the card
                Vector numberVector;
                if (this.bl.CardIsSquare(indexer))
                {
                    numberVector = new Vector(-42, -40);
                }
                else
                {
                    numberVector = new Vector(-43, 15);
                }

                this.DrawText(dc, Point.Subtract(this.bl.GameBoard[indexer].Rect, numberVector), indexer.ToString());
                indexer++;
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
                this.bl.GameBoard[indexer].Rect = this.bl.CalculatePrimaryPosition(holder, changingWidth, this.bl.Met.NormalCardHeight);

                myRect = new Rect(holder.X, holder.Y, changingWidth, this.bl.Met.NormalCardHeight);
                dc.DrawRectangle(brush, myPen, myRect);

                // drawing the number of the card
                Vector numberVector;
                if (this.bl.CardIsSquare(indexer))
                {
                    numberVector = new Vector(60, -40);
                }
                else
                {
                    numberVector = new Vector(10, -40);
                }

                this.DrawText(dc, Point.Subtract(this.bl.GameBoard[indexer].Rect, numberVector), indexer.ToString());
                indexer++;
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
                this.bl.GameBoard[indexer].Rect = this.bl.CalculatePrimaryPosition(holder, this.bl.Met.NormalCardHeight, changingWidth);

                myRect = new Rect(holder.X, holder.Y, this.bl.Met.NormalCardHeight, changingWidth);
                dc.DrawRectangle(brush, myPen, myRect);

                // drawing the number of the card, we do not need to check the square cards here, bc we only have normal horizontal ones
                Vector numberVector = new Vector(60, 10);
                this.DrawText(dc, Point.Subtract(this.bl.GameBoard[indexer].Rect, numberVector), indexer.ToString());
                indexer++;
            }
        }

        private void DrawText(DrawingContext drawingContext, Point origin, string textToBeDisplayed)
        {
            FormattedText eventText = new FormattedText(
              textToBeDisplayed,
              CultureInfo.CurrentUICulture,
              FlowDirection.LeftToRight,
              new Typeface("Impact"),
              20,
              Brushes.White);

            drawingContext.DrawText(eventText, origin);
        }
    }
}
