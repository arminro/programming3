// <copyright file="CardEventArgs.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using Beadando.Model;

    public class CardEventArgs : EventArgs
    {
        public CardEventArgs(string cardTypeKey, Player currentPlayer, int? indexNumberForCols = null)
        {
            this.CardTypeKey = cardTypeKey;
            this.CurrentPlayer = currentPlayer;
            this.IndexNumberForCols = indexNumberForCols;
        }

        public string CardTypeKey { get; private set; }

        public Player CurrentPlayer { get; set; }

        public int? IndexNumberForCols { get; set; }
    }
}
