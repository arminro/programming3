using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.ViewModel
{
    public class SerializWrapper<T, K>
    {
        T Arg1;
        K Arg2;

        public SerializWrapper()
        {
                
        }

        public SerializWrapper(T arg1, K arg2)
        {
            Arg1 = arg1;
            Arg2 = arg2;
        }
    }
}
