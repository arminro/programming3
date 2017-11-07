// <copyright file="RenderedButton.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// A rendered visual element mimicig the capabilities of a wpf button
    /// </summary>
    public class RenderedButton
    {
        private Rect dimensions;
        private string text;

        private int fontSize;
        private bool enabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderedButton"/> class.
        /// </summary>
        /// <param name="dc">the drawing context</param>
        /// <param name="dimensions">represents the sarting points, width and height of the button</param>
        /// <param name="text">the ext the button will display</param>
        /// <param name="background">the brush used as the bavkground</param>
        /// <param name="textColor">the brush zsed as the color of the text</param>
        /// <param name="enabled">indicates wether a button is usable or not</param>
        /// <param name="fontSize">the fontsize</param>
        /// <param name="needBorder">bool to check if the button needs any borders</param>
        public RenderedButton(DrawingContext dc, Rect dimensions, string text, Brush background, Brush textColor, bool enabled, int fontSize, bool needBorder = true)
        {
            this.dimensions = dimensions;
            this.text = text;
            this.fontSize = fontSize;
            this.enabled = enabled;

            // drawing the background part
            dc.DrawRoundedRectangle(enabled == true ? background : Brushes.Gray,  needBorder == true ? new Pen(Brushes.DarkGray, 2) : null, dimensions, 5, 5);
            FormattedText buttonText = new FormattedText(
               text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                fontSize,
                textColor);

            // we have to consider how long is the string to display as well as its fontsize
            dc.DrawText(buttonText, new Point(dimensions.X + (dimensions.Width / 2) - (text.Length * fontSize / 4), dimensions.Y + 8));
        }

        /// <summary>
        /// the click event firing upon a mouse click
        /// </summary>
        public event EventHandler<EventArgs> Click; // click event to be fired when we detect that the mouse clicked on the button

        /// <summary>
        /// Gets the dimensions
        /// </summary>
        public Rect Dimensions
        {
            get
            {
                return this.dimensions;
            }
        }

        /// <summary>
        ///  Gets or sets the button text
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                this.text = value;
            }
        }

        /// <summary>
        ///  Gets or sets the fontsize of the button
        /// </summary>
        public int FontSize
        {
            get
            {
                return this.fontSize;
            }

            set
            {
                this.fontSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether  the button is clickable
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }

            set
            {
                this.enabled = value;
            }
        }

        /// <summary>
        /// Basic detection for button click event
        /// </summary>
        /// <param name="p">The point on the screen the mouse cliced on</param>
        public void CheckIfPressed(Point p)
        {
            if (this.Enabled && p.X >= this.dimensions.X && p.X <= this.dimensions.X + this.dimensions.Width &&
                p.Y >= this.dimensions.Y && p.Y <= this.dimensions.Y + this.dimensions.Height)
            {
                this.Click?.Invoke(this, new EventArgs());
            }
        }
    }
}
