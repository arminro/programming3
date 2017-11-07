// <copyright file="RenderedTextblock.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// A class with similarly dashing abilities like the rendered button
    /// </summary>
    public class RenderedTextblock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderedTextblock"/> class.
        /// </summary>
        /// <param name="background">the background of the tb</param>
        /// <param name="textColor">the brush for the text appearing on the tb</param>
        /// <param name="dc">reference to the drawing context</param>
        /// <param name="dimensions">represents the starting point, width and heiht of the tb</param>
        public RenderedTextblock(Brush background, Brush textColor, DrawingContext dc, Rect dimensions)
        {
            this.Background = background;
            this.TextColor = textColor;

            this.Dimensions = dimensions;

            /*Yes, this is copy/pasted from rendered button*/
            dc.DrawRoundedRectangle(this.Background, new Pen(Brushes.DarkGray, 2), this.Dimensions, 5, 5);
        }

        /// <summary>
        /// Gets or sets the background
        /// </summary>
        public Brush Background { get; set; }

        /// <summary>
        /// Gets or sets the dimensions
        /// </summary>
        public Rect Dimensions { get; set; }

        /// <summary>
        /// Gets or sets the textcolor
        /// </summary>
        public Brush TextColor { get; set; }

        /// <summary>
        /// Draws the the text of the tb, has to be called outside of the class
        /// </summary>
        /// <param name="dc">reference to the drawing context</param>
        /// <param name="text">The text appearing on the tb</param>
    public void DrawText(DrawingContext dc, string text)
        {
            FormattedText textblockText = new FormattedText(
                    text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Impact"),
                    16,
                    this.TextColor);

            // we have to consider how long is the string to display as well as its fontsize
            dc.DrawText(textblockText, new Point(this.Dimensions.X + (this.Dimensions.Width / 2) - (text.Length * 4), this.Dimensions.Y + 8));
        }
    }
}
