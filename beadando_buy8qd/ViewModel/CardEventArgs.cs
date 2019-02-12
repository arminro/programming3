// <copyright file="CardEventArgs.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using Beadando.Model;

    /// <summary>
    /// Desc from EventArgs, pass on data to initiate the windows upon a player arriving at a card
    /// </summary>
    public class CardEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardEventArgs"/> class.
        /// </summary>
        /// <param name="cardTypeKey">the key of the card the player stands on</param>
        /// <param name="currentPlayer">reference to the player</param>
        /// <param name="indexNumberForCols">if the event has more than 1 outcomes, this indeicates which one is that from an avalabble pool of array</param>
        public CardEventArgs(string cardTypeKey, Player currentPlayer, int? indexNumberForCols = null)
        {
            this.CardTypeKey = cardTypeKey;
            this.CurrentPlayer = currentPlayer;
            this.IndexNumberForCols = indexNumberForCols;
        }

        /// <summary>
        /// Gets the key of the card the player stand on
        /// </summary>
        public string CardTypeKey { get; private set; }

        /// <summary>
        /// Gets or sets reference to the current player
        /// </summary>
        public Player CurrentPlayer { get; set; }

        /// <summary>
        /// Gets or sets the index of the event
        /// </summary>
        public int? IndexNumberForCols { get; set; }
    }
}
