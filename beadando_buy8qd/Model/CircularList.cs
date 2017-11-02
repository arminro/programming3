using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.Model
{
    public class CircularList<T> : ObservableCollection<T>
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

        public T GetNextElement(int number = 1)
        {
            return this[inc+=number];
        }

        public T WhatIsNext()
        {
            return this[inc + 1];
        }
    }
}
