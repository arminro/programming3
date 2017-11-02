using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.ViewModel
{

    public class TransferEventArgs : EventArgs
    {
        public string Load { get; set; }
        public TransferEventArgs(string load)
        {
            this.Load = load;
        }
    }
}
