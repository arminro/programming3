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
using System.Windows.Navigation;
using System.Drawing;

namespace Beadando
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        
        int leftVerticalAlign;
        string[] resourceNames;

        public TempVM VM { get; set; }
       
        public MainWindow()
        {
            InitializeComponent();
            VM = new TempVM();
            this.DataContext = VM;
            //how many elements the row is designed to hold
            
            resourceNames = new string[] { "go", "event", "event", "roll", "roll", "roll", "roll", "enroll", "roll", "megajanlott" };

            //the rect marking the outer edge of the left vertical row
            leftVerticalAlign = Constants.startPosition - ((Constants.numberOfElementsInAHorizontalRow) * Constants.NormalCard.width 
                + (Constants.SquareCard.widthHeight - Constants.NormalCard.width));
        }

        protected override void OnRender(DrawingContext dc)
        {
            //SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            //mySolidColorBrush.Color = Colors.LimeGreen;
            

            System.Windows.Media.Pen myPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Blue, 3);

            //BACKGROUND --> ALWAYS RENDERED FIRST
            Rect myRect = new Rect(0, 0, this.ActualWidth, this.ActualHeight);
            dc.DrawRectangle((ImageBrush)this.Resources["table"], null, myRect);

            //THE TRACK --> ALWAYS RENDERED AFTER THE BACKGROUND
            //start column
            myRect = new Rect(Constants.startPosition, Constants.lowerHorizontalAlign, Constants.SquareCard.widthHeight, Constants.SquareCard.widthHeight);
            dc.DrawRectangle((ImageBrush)this.Resources["start"], myPen, myRect);

            int changingWidth = Constants.NormalCard.width;
            int correction = 0;
            for (int i = 1; i <= Constants.numberOfElementsInAHorizontalRow; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                if(i % Constants.numberOfElementsInAHorizontalRow == 0)
                {
                    //myRect = new Rect(Constants.startPosition - 
                    //    (i * Constants.NormalCard.width - (Constants.SquareCard.widthHeight - 
                    //    Constants.NormalCard.width) + Constants.SquareCard.widthHeight), Constants.lowerHorizontalAlign, 
                    //    Constants.SquareCard.widthHeight, Constants.SquareCard.widthHeight);
                    changingWidth = Constants.SquareCard.widthHeight;

                    //we have to put the square card a little further, because it is wider
                    correction = Constants.SquareCard.widthHeight - Constants.NormalCard.width;

                }

                myRect = new Rect(Constants.startPosition - i * Constants.NormalCard.width - correction, Constants.lowerHorizontalAlign, changingWidth, Constants.NormalCard.height);
                dc.DrawRectangle((ImageBrush)this.Resources[resourceNames[i-1]], myPen, myRect);
            }

            changingWidth = Constants.NormalCard.width;
            correction = 0;
            for (int i = 1; i <= Constants.numberOfElementsInAVerticalRow; i++)
            {
                //if we reached the last in the row (and it cannot be 0 bc of the starting idx)
                if (i % Constants.numberOfElementsInAVerticalRow == 0)
                {
                
                    changingWidth = Constants.SquareCard.widthHeight;
                
                    //we have to put the square card a little further, because it is wider
                    correction = Constants.SquareCard.widthHeight - Constants.NormalCard.width;
                
                }

                myRect = new Rect(leftVerticalAlign, (Constants.lowerHorizontalAlign - i * Constants.NormalCard.width) - correction, Constants.NormalCard.height, changingWidth);
                dc.DrawRectangle((ImageBrush)this.Resources[resourceNames[i - 1]], myPen, myRect);
            }

            //PUPPETS --> RENDERED AFTER THE TRACK
            //myRect = new Rect(Constants.startPosition, Constants.lowerHorizontalAlign, 50, 50);
            System.Windows.Point center = new System.Windows.Point(Constants.startPosition + (Constants.SquareCard.widthHeight / 2), 
                Constants.lowerHorizontalAlign + (Constants.SquareCard.widthHeight / 2));

            dc.DrawEllipse((ImageBrush)this.Resources["nik"], null, center, 40, 40);

            center = new System.Windows.Point((Constants.startPosition + (Constants.SquareCard.widthHeight / 2)) - 100, 
                Constants.lowerHorizontalAlign + (Constants.SquareCard.widthHeight / 2));

            dc.DrawEllipse((ImageBrush)this.Resources["kando"], null, center, 40, 40);

            center = new System.Windows.Point((Constants.startPosition + (Constants.SquareCard.widthHeight / 2)) - 100,
                Constants.lowerHorizontalAlign + (Constants.SquareCard.widthHeight / 2) - 80);

            dc.DrawEllipse((ImageBrush)this.Resources["rejto"], null, center, 40, 40);

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


    }
}
