using Beadando.View;
using Beadando.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace Beadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {

        BL bl;
        
        RenderedButton next;
        RenderedButton roll;
        RenderedButton options;
       
        
       

        //storing the chosen colors for the players, we could not use the standard resource dict for this as we want to use the same key as those used for the puppets themselves
        Dictionary<string, SolidColorBrush> playerColors;



        public MainWindow(BL bl)
        {
            
            InitializeComponent();
            this.bl = bl;
            
            playerColors = new Dictionary<string, SolidColorBrush>
            {
                { "nik", Brushes.DodgerBlue },
                { "kando", Brushes.Gold },
                { "rejto", Brushes.LimeGreen }
            };
          
            //subscribing to connector events

            
            bl.Invalidate += (object sender, EventArgs eve) => {
                InvalidateVisual();
            };
            bl.NotifyPlayer += (object sender, TransferEventArgs trans) =>
            {
                MessageBox.Show(trans.Load, $"Üzenet {bl.Player.Name} számára", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            
            };

            bl.GeneralNotification += (object sender, TransferEventArgs trans) =>
            {
                MessageBox.Show(trans.Load, "Általános Üzenet",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            };
            bl.EventCard += (object s, CardEventArgs eventArgs) =>
            {
                EventViewer ev = new EventViewer(eventArgs.CardTypeKey, bl, playerColors);
                bool closedWithPositive = (bool)ev.ShowDialog();
                if(closedWithPositive == true)
                {

                    //if the player wanted to buy a new subject, we show the subject dialog
                    if (eventArgs.CardTypeKey == "enroll")
                    {
                        bl.InitializeSubjectTransactions(false, false);
                    }

                }
                
            };

            bl.InitiateSubjectTransaction += (object sender, SubjectEventArgs subEve) =>
            {
                SubjectWindow subw = new SubjectWindow(bl, subEve.IsSubjectFree);
                subw.Background = playerColors[bl.Player.PuppetKey];
                subw.ShowDialog();
                //if (subw.ShowDialog() == true)
                //{
                    //InvalidateVisual();
                //}

            };

            bl.FinishedGame += (object s, EventArgs e) =>
            {
                WinWindow w = new WinWindow(bl, this);
                w.ShowDialog();
            };
            
           
            //how many elements the row is designed to hold

            //resourceNamesNormal = new string[] { "go", "event", "roll", "enroll", "uv", "randi", "neptun"};
            //resourceNamesSquare = new string[] { "megajanlott", "lead", "einstein" };
            
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
                    bl.MoveHorizontally(bl.MovementSpeed);
                    InvalidateVisual();
                    bl.SetMetrics();
                    break;
                case Key.Right:
                    bl.MoveHorizontally(-bl.MovementSpeed);
                    InvalidateVisual();
                    bl.SetMetrics();
                    break;
                case Key.Up:
                    bl.MoveVertically(bl.MovementSpeed);
                    InvalidateVisual();
                    bl.SetMetrics();
                    break;
                case Key.Down:
                    bl.MoveVertically(-bl.MovementSpeed);
                    InvalidateVisual();
                    bl.SetMetrics();
                    break;
            }
        }





       


        protected override void OnRender(DrawingContext dc)
        {
            //SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            //mySolidColorBrush.Color = Colors.LimeGreen;
            base.OnRender(dc);
            //updating the start card center
            bl.GameBoard[0].Rect = bl.CalculatePrimaryPosition(new Point(bl.Met.StartPosition.X, bl.StartY), Metrics.SquareCard.widthHeight, Metrics.SquareCard.widthHeight);
            
            int indexer = 1;
            Pen myPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 3);
            
            //BACKGROUND --> ALWAYS RENDERED FIRST
            Rect myRect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            dc.DrawRectangle((ImageBrush)this.Resources["wood"], null, myRect);

            //TODO background of the gameboard
            Rect backgroundRect = new Rect((this.ActualWidth / 2)-(bl.Met.WidthOfBoard/2), (this.ActualHeight / 2)-(bl.Met.HeightOfBoard/2), bl.Met.WidthOfBoard, bl.Met.HeightOfBoard);

            dc.DrawRectangle(Brushes.Blue, null, backgroundRect);


            DrawBoard(dc, myRect, indexer, myPen);
            


            //PUPPETS --> RENDERED AFTER THE TRACK

            //the view does not know about Player type, just sees the data from bl
            for (int i = 0; i < bl.Players.Count; i++)
            {
                dc.DrawEllipse((ImageBrush)this.Resources[bl.Players[i].PuppetKey], null, bl.Players[i].Currentposition, bl.PuppetDiameter, bl.PuppetDiameter);
            }



            //PLAYER UI
            Size playerUiSize = new Size(550, 450);
            Point startPoint = new Point(((ActualWidth / 2) - (playerUiSize.Width / 2) + bl.OffsetHorizontal), ((ActualHeight / 2) - (playerUiSize.Height / 2)) + bl.OffsetVertical);
            Rect mainPlayerScreen = new Rect(startPoint, playerUiSize);
            
            dc.DrawRoundedRectangle((ImageBrush)this.Resources["phone"], null, mainPlayerScreen, 5, 5);
            
            DrawPlayerUI(dc, startPoint);

            RenderedTextblock rounds = new RenderedTextblock(playerColors[bl.Player.PuppetKey], Brushes.White, dc, new Rect(ActualWidth - 120, ActualHeight - 270, 100, 40));
            rounds.DrawText(dc, bl.RoundCounter.ToString());

            //drawing the block
            RenderedTextblock block = new RenderedTextblock(playerColors[bl.Player.PuppetKey], Brushes.White, dc, new Rect(ActualWidth - 120, ActualHeight - 220, 100, 40));
            if (bl.RandomGeneratedNumber > 0)
            {
                block.DrawText(dc, bl.RandomGeneratedNumber.ToString());
            }


            //drawing the roll button
            roll = new RenderedButton(dc, new Rect(ActualWidth - 120, ActualHeight - 170, 100, 40), "Gurítok", playerColors[bl.Player.PuppetKey], Brushes.White, bl.RollButtonEnabled, 20);
            roll.Click += (object s, EventArgs e) =>
            {
                bl.GenerateRandomNumber();
                bl.GoToPosition(bl.RandomGeneratedNumber);
                bl.RollButtonEnabled = false; //we make the button uninteractable
            };
           

            //next player
            next = new RenderedButton(dc, new Rect(ActualWidth-120, ActualHeight-120, 100, 40), "Következő", playerColors[bl.Player.PuppetKey], Brushes.White, true, 20);
            next.Click += (object sender, EventArgs eve) =>
            {
                bl.NextRound();
                InvalidateVisual();
            };

            options = new RenderedButton(dc, new Rect(ActualWidth - 90, ActualHeight - 70, 50, 50), "", (ImageBrush)Resources["options"], Brushes.White, true, 20, false);
            options.Click += (object senderOptions, EventArgs eveOptions) =>
            {
                OptionsWindow opt = new OptionsWindow(bl, this);
                opt.ShowDialog();
            };



        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bl.Zoom(e.Delta/60);
            InvalidateVisual();
        }

        private void DrawPlayerUI(DrawingContext drawingContext, Point uiRect)
        {
            Brush brush = playerColors[bl.Player.PuppetKey];
            FormattedText playerName = new FormattedText(
               bl.Player.Name,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                18, brush);

            drawingContext.DrawText(playerName, new Point((uiRect.X + 100), (uiRect.Y + 210)));

            FormattedText playerMoney = new FormattedText(
               bl.Player.Money.ToString(),
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                70, brush);

            drawingContext.DrawText(playerMoney, new Point((uiRect.X + 200), (uiRect.Y + 260)));
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //we have to check every button if it was  pressed
            //tried it with adding these elements ot a list and looping over it, but this does not work that way 
            //(maybe because how preview is propagated)                                                                                     
            next.CheckIfPressed(e.GetPosition(this));
            roll.CheckIfPressed(e.GetPosition(this));
            options.CheckIfPressed(e.GetPosition(this));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (bl.IsTheGameStillOn() && MessageBox.Show("Mentesz kilépés előtt? Ha nem, elveszik amit elértél", 
                "Kilépsz?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                bl.Save(); 
            }
        }


        /// <summary>
        /// Draws the images of the board. It uses the images defined in the WPF Resource Dictionary reading the entries stored in the Dictionary GameBoard of BL.
        /// </summary>
        /// <param name="dc">Drawing Context</param>
        /// <param name="myRect">Rectangle to hold position info</param>
        /// <param name="indexer">This iterates through the Dictionary GameBoard of BL</param>
        /// <param name="myPen">A pen used to draw the edges of the card images</param>
        void DrawBoard(DrawingContext dc, Rect myRect, int indexer, Pen myPen)
        {
            //THE TRACK --> ALWAYS RENDERED AFTER THE BACKGROUND
            //start column
            ImageBrush brush;
            myRect = new Rect(bl.Met.StartPosition.X, bl.StartY, Metrics.SquareCard.widthHeight, Metrics.SquareCard.widthHeight);
            dc.DrawRectangle((ImageBrush)this.Resources["start"], myPen, myRect);



            //lower horizontal part
            int changingWidth = Metrics.NormalCard.width;
            int correction = 0;
            Point holder; //this holds the calcuated points
            for (int i = 1; i <= bl.Met.NumberOfElementsInAHorizontalRow; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[bl.GameBoard[indexer].ImageKey];
                if (i % bl.Met.NumberOfElementsInAHorizontalRow == 0)
                {
                    changingWidth = Metrics.SquareCard.widthHeight;

                    //we have to put the square card a little further, because it is wider
                    correction = Metrics.SquareCard.widthHeight - Metrics.NormalCard.width;

                }
                holder = new Point(bl.Met.StartPosition.X - i * Metrics.NormalCard.width - correction, bl.StartY);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, changingWidth, Metrics.NormalCard.height);
                myRect = new Rect(holder.X, holder.Y, changingWidth, Metrics.NormalCard.height);
                dc.DrawRectangle(brush, myPen, myRect);

            }


            //left vertical part
            changingWidth = Metrics.NormalCard.width;
            correction = 0;


            /*The vertical parts are tricky, because we want to use images here that are tilted by 90° */
            for (int i = 1; i <= bl.Met.NumberOfElementsInAVerticalRow; i++)
            {



                //save the imagekey of the gameboard
                string temp = bl.GameBoard[indexer].ImageKey;

                //we check if the image is not among the square ones, as they do not have tilted alternatives
                if (bl.ResourceNamesSquare.Contains(temp))
                {
                    brush = (ImageBrush)this.Resources[temp];
                }
                else
                {
                    //the string[]s for both the normal and tilted images have the same order
                    //so we we can look for the index of temp in the string[] of normal images
                    //and use its index to access the name of the tilted image
                    int indexInHorizontal = Array.FindIndex(bl.ResourceNamesNormal, p => p == temp);
                    brush = (ImageBrush)this.Resources[bl.ResourceNamesHorizontal[indexInHorizontal]];
                }

                if (i % bl.Met.NumberOfElementsInAVerticalRow == 0)
                {
                    changingWidth = Metrics.SquareCard.widthHeight;
                    //we have to put the square card a little further, because it is wider
                    correction = Metrics.SquareCard.widthHeight - Metrics.NormalCard.width;
                }
                holder = new Point(bl.LeftVerticalAlign, (bl.StartY - i * Metrics.NormalCard.width) - correction);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, Metrics.NormalCard.height, changingWidth);
                myRect = new Rect(holder.X, holder.Y, Metrics.NormalCard.height, changingWidth);

                dc.DrawRectangle(brush, myPen, myRect);

            }

            //upper horizontal part
            changingWidth = Metrics.NormalCard.width;
            correction = 0;
            for (int i = 1; i <= bl.Met.NumberOfElementsInAHorizontalRow; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[bl.GameBoard[indexer].ImageKey];
                if (i % bl.Met.NumberOfElementsInAHorizontalRow == 0)
                {

                    changingWidth = Metrics.SquareCard.widthHeight;

                    //we have to put the square card a little further, because it is wider
                    correction = Metrics.SquareCard.widthHeight - Metrics.NormalCard.width;

                }
                holder = new Point((bl.LeftVerticalAlign + i * Metrics.NormalCard.width)
                    + Metrics.SquareCard.widthHeight - Metrics.NormalCard.width, bl.UpperVerticalAlign);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, changingWidth, Metrics.NormalCard.height);

                myRect = new Rect(holder.X,
                    holder.Y,
                    changingWidth, Metrics.NormalCard.height);
                dc.DrawRectangle(brush, myPen, myRect);
            }


            changingWidth = Metrics.NormalCard.width;
            correction = 0;
            for (int i = 1; i <= bl.Met.NumberOfElementsInAVerticalRow - 1; i++)
            {
                //save the imagekey of the gameboard
                string temp = bl.GameBoard[indexer].ImageKey;

                //we check if the image is not among the square ones, as they do not have tilted alternatives
                if (bl.ResourceNamesSquare.Contains(temp))
                {
                    brush = (ImageBrush)this.Resources[temp];
                }
                else
                {
                    //the string[]s for both the normal and tilted images have the same order
                    //so we we can look for the index of temp in the string[] of normal images
                    //and use its index to access the name of the tilted image
                    int indexInHorizontal = Array.FindIndex(bl.ResourceNamesNormal, p => p == temp);
                    brush = (ImageBrush)this.Resources[bl.ResourceNamesHorizontal[indexInHorizontal]];
                }
                holder = new Point(bl.RightVerticalAlign,
                      (bl.StartY - bl.Met.NumberOfElementsInAVerticalRow * Metrics.NormalCard.width) + i * Metrics.NormalCard.width);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, Metrics.NormalCard.height, changingWidth);

                myRect = new Rect(holder.X,
                    holder.Y, Metrics.NormalCard.height, changingWidth);
                dc.DrawRectangle(brush, myPen, myRect);

            }
        }

    }
}
