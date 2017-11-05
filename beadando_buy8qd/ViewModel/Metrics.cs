// <copyright file="Metrics.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System.Windows;

    /// <summary>
    /// This class accompanies the viewmodel to store view-related data
    /// </summary>
    public class Metrics
    {
        private int widthOfBoard;
        private int heightOfBoard;
        private int numberOfElementsInAHorizontalRow;
        private int numberOfElementsInAVerticalRow;
        private Point startPosition;

        public Metrics()
        {
            this.NormalCardHeight = 135;
            this.NormalCardWidth = 90;
            this.SquareCardMetric = 135; // this is the sane of NormalCardHeight, but it makes sense to store in a variable to clarify the code
            // marginX = (int)(SystemParameters.PrimaryScreenWidth * 0.08);
            // marginY = (int)(SystemParameters.PrimaryScreenHeight * 0.08);
            this.NumberOfElementsInAHorizontalRow = 10;
            this.NumberOfElementsInAVerticalRow = 5;
            this.WidthOfBoard = ((this.NumberOfElementsInAHorizontalRow - 2) * this.NormalCardWidth) + (2 * this.SquareCardMetric);
            this.HeightOfBoard = ((this.NumberOfElementsInAVerticalRow - 2) * this.NormalCardWidth) + (2 * this.SquareCardMetric);
            this.StartPosition = new Point((SystemParameters.PrimaryScreenWidth / 2) + (this.WidthOfBoard / 2) - this.NormalCardWidth, ((SystemParameters.PrimaryScreenHeight / 2) + (this.HeightOfBoard / 2)) - this.NormalCardWidth);
        }

        // int marginX;
        // int marginY;

        // public const int startPosition_start = 1065; //950
        // public const int lowerHorizontalAlign_start = 550;
        public int WidthOfBoard
        {
            get
            {
                return this.widthOfBoard;
            }

            set
            {
                this.widthOfBoard = value;
            }
        }

        public int HeightOfBoard
        {
            get
            {
                return this.heightOfBoard;
            }

            set
            {
                this.heightOfBoard = value;
            }
        }

        public int NumberOfElementsInAHorizontalRow
        {
            get
            {
                return this.numberOfElementsInAHorizontalRow;
            }

            set
            {
                this.numberOfElementsInAHorizontalRow = value;
            }
        }

        public int NumberOfElementsInAVerticalRow
        {
            get
            {
                return this.numberOfElementsInAVerticalRow;
            }

            set
            {
                this.numberOfElementsInAVerticalRow = value;
            }
        }

        public Point StartPosition
        {
            get
            {
                return this.startPosition;
            }

            set
            {
                this.startPosition = value;
            }
        }

        public int NormalCardHeight { get; set; }

        public int NormalCardWidth { get; set; }

        public int SquareCardMetric { get; set; }
    }
}
