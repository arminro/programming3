using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Beadando.View
{
    class RenderedButton
    {
        Rect dimensions;
        string text;
        Brush background;
        Brush textColor;
        int fontSize;
        public event EventHandler<EventArgs> Click;

        public RenderedButton(DrawingContext dc, Rect dimensions, string text, Brush background, Brush textColor, int fontSize = 16)
        {
            this.dimensions = dimensions;
            this.text = text;
            this.background = background;
            this.fontSize = fontSize;

            dc.DrawRoundedRectangle(background, new Pen(Brushes.DarkGray, 2), dimensions, 5, 5);
            FormattedText buttonText = new FormattedText(
               text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface("Impact"),
                fontSize, textColor);
            //we have to consider how long is the string to display as well as its fontsize
            dc.DrawText(buttonText, new Point(dimensions.X + (dimensions.Width)/2-(text.Length*fontSize/4), 
                dimensions.Y + 8));
            
        }



        public Rect Dimensions
        {
            get
            {
                return dimensions;
            }

            
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public int FontSize
        {
            get
            {
                return fontSize;
            }

            set
            {
                fontSize = value;
            }
        }

        public void CheckIfPressed(Point p)
        {
            if(p.X >= dimensions.X && p.X <= dimensions.X+ dimensions.Width &&
                p.Y >= dimensions.Y && p.Y <= dimensions.Y + dimensions.Height)
            {
                Click?.Invoke(this, new EventArgs());
            }
        }
    }
}
