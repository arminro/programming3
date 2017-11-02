using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beadando.Model;

namespace Beadando.ViewModel
{
    public class CardEventArgs : EventArgs
    {
        public string CardTypeKey { get; private set; }
        public Player CurrentPlayer { get; set; }
        public int? IndexNumberForCols { get; set; }


        public CardEventArgs(string cardTypeKey, Player currentPlayer, int? indexNumberForCols = null)
        {
            this.CardTypeKey = cardTypeKey;
            this.CurrentPlayer = currentPlayer;
            this.IndexNumberForCols = indexNumberForCols;
        }
    }
}
