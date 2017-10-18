﻿using Beadando.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Random r;

        public MainWindow()
        {
            
            InitializeComponent();
            bl = new BL();
            bl.Invalidate += (object sender, EventArgs eve) => {
                InvalidateVisual();
            };

            r = new Random();
            //how many elements the row is designed to hold
            
            //resourceNamesNormal = new string[] { "go", "event", "roll", "enroll", "uv", "randi", "neptun"};
            //resourceNamesSquare = new string[] { "megajanlott", "lead", "einstein" };
           
            
        }


        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.Tab)
            {
                bl.GoToPosition(r.Next(0, bl.GameBoard.Count));
            }
        }
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

            //updating the start card center
            bl.GameBoard[0].Rect = bl.CalculatePrimaryPosition(new Point(bl.StartPosition, bl.LowerHorizontalAlign), BL.SquareCard.widthHeight, BL.SquareCard.widthHeight);
            ImageBrush brush;
            int indexer = 1;
            Pen myPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 3);

            //BACKGROUND --> ALWAYS RENDERED FIRST
            Rect myRect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            dc.DrawRectangle((ImageBrush)this.Resources["wood"], null, myRect);

            //THE TRACK --> ALWAYS RENDERED AFTER THE BACKGROUND
            //start column

            myRect = new Rect(bl.StartPosition, bl.LowerHorizontalAlign, BL.SquareCard.widthHeight, BL.SquareCard.widthHeight);
            dc.DrawRectangle((ImageBrush)this.Resources["start"], myPen, myRect);

            //lower horizontal part
            int changingWidth = BL.NormalCard.width;
            int correction = 0;
            Point holder; //this holds the calcuated points
            for (int i = 1; i <= Constants.numberOfElementsInAHorizontalRow_start; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[bl.GameBoard[indexer].ImageKey];
                if (i % Constants.numberOfElementsInAHorizontalRow_start == 0)
                {
                    //myRect = new Rect(Constants.startPosition - 
                    //    (i * Constants.NormalCard.width - (Constants.SquareCard.widthHeight - 
                    //    Constants.NormalCard.width) + Constants.SquareCard.widthHeight), Constants.lowerHorizontalAlign, 
                    //    Constants.SquareCard.widthHeight, Constants.SquareCard.widthHeight);
                    changingWidth = BL.SquareCard.widthHeight;

                    //we have to put the square card a little further, because it is wider
                    correction = BL.SquareCard.widthHeight - BL.NormalCard.width;

                }
                holder = new Point(bl.StartPosition - i * BL.NormalCard.width - correction, bl.LowerHorizontalAlign);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, changingWidth, BL.NormalCard.height);
                myRect = new Rect(holder.X, holder.Y, changingWidth, BL.NormalCard.height);
                // dc.DrawRectangle((ImageBrush)this.Resources[resourceNamesNormal[rand.Next(0,
                //     resourceNamesNormal.Length)]], myPen, myRect);
                 dc.DrawRectangle(brush, myPen, myRect);

            }

            
            //left vertical part
            changingWidth = BL.NormalCard.width;
            correction = 0;
            // Create a 45 rotate transform about the brush's center
            // and apply it to the brush's Transform property.
            //RotateTransform anotherRotateTransform = new RotateTransform();
            //anotherRotateTransform.CenterX = 87.5;
            //anotherRotateTransform.CenterY = 45;
            //anotherRotateTransform.Angle = 90;
            

            for (int i = 1; i <= Constants.numberOfElementsInAVerticalRow_start; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[bl.GameBoard[indexer].ImageKey];
                //brush.Transform = anotherRotateTransform;
                if (i % Constants.numberOfElementsInAVerticalRow_start == 0)
                {
                    changingWidth = BL.SquareCard.widthHeight;
                    //we have to put the square card a little further, because it is wider
                    correction = BL.SquareCard.widthHeight - BL.NormalCard.width;
                }
                holder = new Point(bl.LeftVerticalAlign, (bl.LowerHorizontalAlign - i * BL.NormalCard.width) - correction);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, BL.NormalCard.height, changingWidth);
                myRect = new Rect(holder.X, holder.Y, BL.NormalCard.height, changingWidth);
                //dc.DrawRectangle((ImageBrush)this.Resources[resourceNamesNormal[rand.Next(0,
                //    resourceNamesNormal.Length)]], myPen, myRect);
                
                dc.DrawRectangle(brush, myPen, myRect);

            }

            //upper horizontal part
            changingWidth = BL.NormalCard.width;
            correction = 0;
            for (int i = 1; i <= Constants.numberOfElementsInAHorizontalRow_start; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[bl.GameBoard[indexer].ImageKey];
                if (i % Constants.numberOfElementsInAHorizontalRow_start == 0)
                {

                    changingWidth = BL.SquareCard.widthHeight;

                    //we have to put the square card a little further, because it is wider
                    correction = BL.SquareCard.widthHeight - BL.NormalCard.width;

                }
                holder = new Point((bl.LeftVerticalAlign + i * BL.NormalCard.width)
                    + BL.SquareCard.widthHeight - BL.NormalCard.width, bl.UpperVerticalAlign);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, changingWidth, BL.NormalCard.height);

                myRect = new Rect(holder.X, 
                    holder.Y, 
                    changingWidth, BL.NormalCard.height);
                //dc.DrawRectangle((ImageBrush)this.Resources[resourceNamesNormal[rand.Next(0,
                //    resourceNamesNormal.Length)]], myPen, myRect);
                dc.DrawRectangle(brush, myPen, myRect);
            }


            changingWidth = BL.NormalCard.width;
            correction = 0;
            for (int i = 1; i <= Constants.numberOfElementsInAVerticalRow_start-1; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                brush = (ImageBrush)this.Resources[bl.GameBoard[indexer].ImageKey];
                // if (i % Constants.numberOfElementsInAVerticalRow_start == 0)
                // {
                //     changingWidth = SquareCard.widthHeight;
                //     //we have to put the square card a little further, because it is wider
                //     correction = SquareCard.widthHeight - NormalCard.width;
                // }
                holder = new Point(bl.RightVerticalAlign,
                      (bl.LowerHorizontalAlign - Constants.numberOfElementsInAVerticalRow_start * BL.NormalCard.width) + i * BL.NormalCard.width);
                bl.GameBoard[indexer++].Rect = bl.CalculatePrimaryPosition(holder, BL.NormalCard.height, changingWidth);

                myRect = new Rect(holder.X, 
                    holder.Y, BL.NormalCard.height, changingWidth);
                //dc.DrawRectangle((ImageBrush)this.Resources[resourceNamesNormal[rand.Next(0,
                //    resourceNamesNormal.Length)]], myPen, myRect);
                dc.DrawRectangle(brush, myPen, myRect);

            }




            //PUPPETS --> RENDERED AFTER THE TRACK
            //myRect = new Rect(Constants.startPosition, Constants.lowerHorizontalAlign, 50, 50);
            //System.Windows.Point center = new System.Windows.Point(startPosition + (SquareCard.widthHeight / 2), 
            //    lowerHorizontalAlign + (SquareCard.widthHeight / 2));
            //System.Windows.Point center = System.Windows.Point.Add(gameBoard[6].Rect, new Vector(50, NormalCard.height/2));

            dc.DrawEllipse((ImageBrush)this.Resources[bl.P.PuppetKey], null, bl.P.Currentposition, bl.PuppetDiameter, bl.PuppetDiameter);

            //center = new System.Windows.Point((startPosition + (SquareCard.widthHeight / 2)) - NormalCard.width, 
            //    lowerHorizontalAlign + (SquareCard.widthHeight / 2));
            //Point newCenter = Point.Subtract(center, new Vector(0, 60));

            //dc.DrawEllipse((ImageBrush)this.Resources["kando"], null, CalculateSecondaryPosition(gameBoard[0].Rect), puppetDiameter, puppetDiameter);

            //center = new System.Windows.Point((startPosition + (SquareCard.widthHeight / 2)) - NormalCard.width,
            //    lowerHorizontalAlign + (SquareCard.widthHeight / 2) - NormalCard.width);
            //newCenter = Point.Add(center, new Vector(0, 60));

            //dc.DrawEllipse((ImageBrush)this.Resources["rejto"], null, CalculateTertialPosition(gameBoard[0].Rect), puppetDiameter, puppetDiameter);

            //for (int i = 1; i <= numberOfElementsInAVerticalRow; i++)
            //{
            //    //if (i != 0 && i % numberOfElementsInAVerticalRow == 0)
            //    //{
            //    //    myRect = new Rect(Constants.lowerHorizontalAlign -
            //    //        (Constants.NormalCard.width - (Constants.SquareCard.widthHeight -
            //    //        Constants.NormalCard.height)),  i *Constants.lowerHorizontalAlign,
            //    //        Constants.SquareCard.widthHeight, Constants.SquareCard.widthHeight);
            //    //}
            //    //else
            //    //{
            //        myRect = new Rect(Constants.startPosition - 
            //            ((numberOfElementsInAHorizontalRow) * Constants.NormalCard.width) + 2*Constants.SquareCard.widthHeight,
            //            Constants.lowerHorizontalAlign - i*Constants.NormalCard.width, Constants.NormalCard.height, Constants.NormalCard.width);
            //    //}
            //    dc.DrawRectangle((ImageBrush)this.Resources[resourceNames[i - 1]], myPen, myRect);
            //}




            // //1st column
            // myRect = new Rect(Constants.startPosition- Constants.NormalCard.width, Constants.lowerHorizontalposition, Constants.NormalCard.width, Constants.NormalCard.height);
            // dc.DrawRectangle((ImageBrush)this.Resources["go"], myPen, myRect);
            //
            // //2nd column
            // myRect = new Rect(Constants.startPosition - 2 * Constants.NormalCard.width, Constants.lowerHorizontalposition, Constants.NormalCard.width, Constants.NormalCard.height);
            // dc.DrawRectangle((ImageBrush)this.Resources["event"], myPen, myRect);
            //
            // myRect = new Rect(Constants.startPosition -  3 * Constants.NormalCard.width, Constants.lowerHorizontalposition, Constants.NormalCard.width, Constants.NormalCard.height);
            // dc.DrawRectangle((ImageBrush)this.Resources["event"], myPen, myRect);
            //
            // myRect = new Rect(Constants.startPosition -  4 * Constants.NormalCard.width, Constants.lowerHorizontalposition, Constants.NormalCard.width, Constants.NormalCard.height);
            // dc.DrawRectangle((ImageBrush)this.Resources["event"], myPen, myRect);
            //
            //
            // myRect = new Rect(Constants.startPosition - 5 * Constants.NormalCard.width, Constants.lowerHorizontalposition, Constants.NormalCard.width, Constants.NormalCard.height);
            // dc.DrawRectangle((ImageBrush)this.Resources["event"], myPen, myRect);
            //
            // myRect = new Rect(Constants.startPosition - 6 * Constants.NormalCard.width, Constants.lowerHorizontalposition, Constants.NormalCard.width, Constants.NormalCard.height);
            // dc.DrawRectangle((ImageBrush)this.Resources["event"], myPen, myRect);
            //
            // myRect = new Rect(Constants.startPosition - 7 * Constants.NormalCard.width-60, Constants.lowerHorizontalposition, Constants.SquareCard.widthHeight, Constants.SquareCard.widthHeight);
            // dc.DrawRectangle((ImageBrush)this.Resources["start"], myPen, myRect);

            
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            bl.Zoom(e.Delta/60);
            InvalidateVisual();
        }

    }
}
