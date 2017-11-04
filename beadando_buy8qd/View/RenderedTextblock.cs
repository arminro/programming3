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
    class RenderedTextblock
    {


        public RenderedTextblock(Brush background, Brush textColor, DrawingContext dc, Rect dimensions)
        {
            this.Background = background;
            this.TextColor = textColor;
          
            this.Dimensions = dimensions;

           
            /*Yes, this is copy/pasted from rendered button*/
            dc.DrawRoundedRectangle(Background, new Pen(Brushes.DarkGray, 2), Dimensions, 5, 5);




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
                    16, TextColor);

            //we have to consider how long is the string to display as well as its fontsize
            dc.DrawText(textblockText, new Point(Dimensions.X + (Dimensions.Width) / 2 - (text.Length * 4),
                Dimensions.Y + 8));

        }

    }
}
