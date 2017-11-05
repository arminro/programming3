// <copyright file="CircularList.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System.Collections.ObjectModel;

    public class CircularList<T> : ObservableCollection<T>
    {
        private int inc;

        public CircularList()
        {
            this.inc = 0;
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
            return this[this.inc += number];
        }

        public T WhatIsNext()
        {
            return this[this.inc + 1];
        }
    }
}
