using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Beadando
{
    class BoardField
    {
        //int number;
        string imageKey;
        Point rect;

       
        public Point Rect
        {
            get
            {
                return rect;
            }

            set
            {
                rect = value;
            }
        }

        public string ImageKey
        {
            get
            {
                return imageKey;
            }

            set
            {
                imageKey = value;
            }
        }

        public BoardField(int number, string imageKey)
        {
            //this.number = number;
            this.ImageKey = imageKey;
        }

        public BoardField()
        {

        }

        //public int Number { get => number; set =>  number = value; }
        
    }
}
