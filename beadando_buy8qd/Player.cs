using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Beadando
{
    class Player
    {
        public Bitmap Bitmap { get; set; }
        public Player(Bitmap b)
        {
            this.Bitmap = b;
        }
    }
}
