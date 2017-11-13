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

        /// <summary>
        /// Initializes a new instance of the <see cref="Metrics"/> class.
        /// </summary>
        /// <param name="playerAreaX">The width of the available player area</param>
        /// <param name="playerAreaY">The height of the available player area</param>
        public Metrics(double playerAreaX, double playerAreaY)
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
            this.StartPosition = new Point((playerAreaX / 2) + (this.WidthOfBoard / 2) - this.NormalCardWidth, ((playerAreaY / 2) + (this.HeightOfBoard / 2)) - this.NormalCardWidth);
        }

        // int marginX;
        // int marginY;

        // public const int startPosition_start = 1065; //950
        // public const int lowerHorizontalAlign_start = 550;

        /// <summary>
        /// Gets or sets the width of the gameboard
        /// </summary>
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

        /// <summary>
        ///  Gets or sets the height of the ganeboard
        /// </summary>
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

        /// <summary>
        ///  Gets or sets the number of elements the gameboard will have in a row
        /// </summary>
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

        /// <summary>
        ///  Gets or sets the number of elements the gameboard will have in a column
        /// </summary>
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

        /// <summary>
        ///  Gets or sets the position from which the board will be drawn
        /// </summary>
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

        /// <summary>
        ///  Gets or sets the height of a normal card
        /// </summary>
        public int NormalCardHeight { get; set; }

        /// <summary>
        ///  Gets or sets the width of a normal card
        /// </summary>
        public int NormalCardWidth { get; set; }

        /// <summary>
        ///  Gets or sets the number equaivalent to the width and height of a squarecard
        /// </summary>
        public int SquareCardMetric { get; set; }
    }
}
