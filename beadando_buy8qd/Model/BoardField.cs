// <copyright file="BoardField.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System.Windows;

    public class BoardField
    {
        // int number;
        private string imageKey;
        private Point rect;
        private Player[] positions; // stores how many puppets are standing on it

        public BoardField(string imageKey)
        {
            this.ImageKey = imageKey;
            this.positions = new Player[3];
        }

        public BoardField()
        {
            this.positions = new Player[3];
        }

        public Point Rect
        {
            get
            {
                return this.rect;
            }

            set
            {
                this.rect = value;
            }
        }

        public string ImageKey
        {
            get
            {
                return this.imageKey;
            }

            set
            {
                this.imageKey = value;
            }
        }

        public int NextFreePosition
        {
            get
            {
                int idx = 0;

                // if there is free space (most likely will be), we return the first one
                while (idx < this.positions.Length && this.positions[idx] != null)
                {
                    idx++;
                }

                return idx;
            }
        }

        public int ArriveAtPosition(Player p)
        {
            // insert the player stepping on the card on the 1st empty place
            // the func will be invoked on the same card as well, so
            // if it is already in the list, we just return that position
            int idx = 0;
            while (idx < this.positions.Length && (this.positions[idx] != null && !this.positions[idx].Equals(p)))
            {
                idx++;
            }

            if (idx < this.positions.Length && this.positions[idx] != p)
            {
                this.positions[idx] = p;
            }

            // we return which position the new player arrived on
            return idx;
        }

        public void DepartFromposition(Player p)
        {
            for (int i = 0; i < this.positions.Length; i++)
            {
                if (this.positions[i] == null)
                {
                    continue;
                }
                else if (this.positions[i].Equals(p))
                {
                    this.positions[i] = null;
                    return;
                }
            }
        }
    }
}
