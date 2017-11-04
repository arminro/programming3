using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.Model
{

    /// <summary>
    /// A dictionary descendant with INT keys that is implemented the way a circular buffer would be
    /// </summary>
    /// <typeparam name="TValue">The generic param holding the value. It is not possible to change the key from INT. </typeparam>
    public class CircularDictionary<TValue> : Dictionary<int, TValue>
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
