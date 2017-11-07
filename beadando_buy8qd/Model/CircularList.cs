// <copyright file="CircularList.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Descendent from ObservableCol of T, it is implemented in the fashion of a circular buffer
    /// </summary>
    /// <typeparam name="T">Generic type param</typeparam>
    public class CircularList<T> : ObservableCollection<T>
    {
        private int inc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularList{T}"/> class.
        /// ctor setting the incrementor to 0
        /// </summary>
        public CircularList()
        {
            this.inc = 0;
        }

        /// <summary>
        /// Indexer that restarts the counting of indeces when over-indexed
        /// </summary>
        /// <param name="i">The index of the elements retrieved</param>
        /// <returns>The element of T indexed</returns>
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

        /// <summary>
        /// Returns the next element in the collection
        /// </summary>
        /// <param name="number">the index of the desired element defaulted at 1</param>
        /// <returns>an element of T with the index of number</returns>
        public T GetNextElement(int number = 1)
        {
            return this[this.inc += number];
        }

        /// <summary>
        /// Returns the next element w/o incrementing the inner indexer
        /// </summary>
        /// <returns>the next element of T in the collection</returns>
        public T WhatIsNext()
        {
            return this[this.inc + 1];
        }
    }
}
