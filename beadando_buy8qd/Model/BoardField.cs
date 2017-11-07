// <copyright file="BoardField.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System.Windows;

    /// <summary>
    /// The class representing a card
    /// </summary>
    public class BoardField
    {
        // int number;
        private string imageKey;
        private Point rect;
        private Player[] positions; // stores how many puppets are standing on it

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardField"/> class.
        /// </summary>
        /// <param name="imageKey">the key of the image drawn</param>
        public BoardField(string imageKey)
        {
            this.ImageKey = imageKey;
            this.positions = new Player[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardField"/> class.
        /// </summary>
        public BoardField()
        {
            this.positions = new Player[3];
        }

        /// <summary>
        /// Gets or sets the rect
        /// </summary>
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

        /// <summary>
        /// Gets or sets the key of the image
        /// </summary>
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

        /// <summary>
        /// Gets the next free position available to the player
        /// </summary>
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

        /// <summary>
        /// Initialies the data when a players arrives on a card
        /// </summary>
        /// <param name="p">the player</param>
        /// <returns>the position out of 3 the player will stand on</returns>
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

        /// <summary>
        /// Unregisters the player from a boardfield (card)
        /// </summary>
        /// <param name="p">reference to the player</param>
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
