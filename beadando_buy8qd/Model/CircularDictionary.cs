using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.Model
{
    class CircularDictionary<TValue> : Dictionary<int, TValue>
    {
        public new TValue this[int i]
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
    }
}
