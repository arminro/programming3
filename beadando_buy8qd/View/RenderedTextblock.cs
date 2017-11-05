// <copyright file="RenderedTextblock.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    public class RenderedTextblock
    {
        public RenderedTextblock(Brush background, Brush textColor, DrawingContext dc, Rect dimensions)
        {
            this.Background = background;
            this.TextColor = textColor;

            this.Dimensions = dimensions;

            /*Yes, this is copy/pasted from rendered button*/
            dc.DrawRoundedRectangle(this.Background, new Pen(Brushes.DarkGray, 2), this.Dimensions, 5, 5);
        }

        public Brush Background { get; set; }

        public Rect Dimensions { get; set; }

        public Brush TextColor { get; set; }

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
