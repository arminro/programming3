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
        Player[] positions; //stores how many puppets are standing on it
       
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
            positions = new Player[3];
        }

        public BoardField()
        {
            positions = new Player[3];
        }

        //TODO sometimes this doesnt work
        public int ArriveAtPosition(Player p)
            {
            //insert the player stepping on the card on the 1st empty place
            //the func will be invoked on the same card as well, so
            //if it is already in the list, we just return that position
            int idx = 0;
            while (idx < positions.Length && (positions[idx] != null && !positions[idx].Equals(p)))
            {
                idx++;
            }
            if(idx < positions.Length && positions[idx] != p)
            {
                positions[idx] = p;
            }

            //we return which position the new player arrived on
            return idx;
        }

        public void DepartFromposition(Player p)
        {
            
            for (int i = 0; i < positions.Length; i++)
            {
                if(positions[i] == null)
                {
                    continue;
                }
                else if(positions[i].Equals(p))
                {
                    positions[i] = null;
                    return;
                }
            }
        }

        public int GetNextFreePosition()
        {
            int idx = 0;
            //if there is free space (most likely will be), we return the first one
            while (idx < positions.Length && positions[idx] != null)
            {
                idx++;
            }
            return idx;
        }

        //public int Number { get => number; set =>  number = value; }
        
    }
}
