using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.ViewModel
{
    class CardEventArgs : EventArgs
    {
        public string CardTypeKey { get; private set; }

        public CardEventArgs(string cardTypeKey)
        {
            this.CardTypeKey = cardTypeKey;
        }
    }
}
