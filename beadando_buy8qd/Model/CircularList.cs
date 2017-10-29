using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.Model
{
    public class CircularList<T> : List<T>
    {
        int inc;
        public CircularList()
        {
            inc = 0;
        }

        public new T this[int i]
        {
            get
            {
                return base[i % this.Count];
            }
            set
            {
                base[i] = value;
            }
        }

        public T GetNextElement()
        {
            return this[inc++];
        }
    }
}
