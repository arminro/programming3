using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Beadando.ViewModel
{
    /// <summary>
    /// This class accompanies the viewmodel to store view-related data
    /// </summary>
    public class Metrics
    {
        public Metrics()
        {
            //marginX = (int)(SystemParameters.PrimaryScreenWidth * 0.08);
            //marginY = (int)(SystemParameters.PrimaryScreenHeight * 0.08);
            NumberOfElementsInAHorizontalRow = 10;
            NumberOfElementsInAVerticalRow = 5;
            WidthOfBoard = ((NumberOfElementsInAHorizontalRow - 2) * NormalCard.width) + 2 * SquareCard.widthHeight;
            HeightOfBoard = ((NumberOfElementsInAVerticalRow - 2) * NormalCard.width) + 2 * SquareCard.widthHeight;
            StartPosition = new Point((SystemParameters.PrimaryScreenWidth/2)+(WidthOfBoard/2) - NormalCard.width, ((SystemParameters.PrimaryScreenHeight/2)+(HeightOfBoard/2)) - NormalCard.width);
        }

        int widthOfBoard;
        int heightOfBoard;
        int numberOfElementsInAHorizontalRow;
        int numberOfElementsInAVerticalRow;
        //int marginX;
        //int marginY;
        Point startPosition;

        //public const int startPosition_start = 1065; //950
        //public const int lowerHorizontalAlign_start = 550;
        public int WidthOfBoard
        {
            get
            {
                return widthOfBoard;
            }

            set
            {
                widthOfBoard = value;
            }
        }

        public int HeightOfBoard
        {
            get
            {
                return heightOfBoard;
            }

            set
            {
                heightOfBoard = value;
            }
        }

        public int NumberOfElementsInAHorizontalRow
        {
            get
            {
                return numberOfElementsInAHorizontalRow;
            }

            set
            {
                numberOfElementsInAHorizontalRow = value;
            }
        }

        public int NumberOfElementsInAVerticalRow
        {
            get
            {
                return numberOfElementsInAVerticalRow;
            }

            set
            {
                numberOfElementsInAVerticalRow = value;
            }
        }

        public Point StartPosition
        {
            get
            {
                return startPosition;
            }

            set
            {
                startPosition = value;
            }
        }

        public class NormalCard
        {
            public static int height = 135; //180
            public static int width = 90; //120
        }

        public class SquareCard
        {
            public static int widthHeight = 135; //180
        }

    }
}
