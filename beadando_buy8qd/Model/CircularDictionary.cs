// <copyright file="CircularDictionary.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A dictionary descendant with INT keys that is implemented the way a circular buffer would be
    /// </summary>
    /// <typeparam name="TValue">The generic param holding the value. It is not possible to change the key from INT. </typeparam>
    [Serializable] // style cop made me do this
    public class CircularDictionary<TValue> : Dictionary<int, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDictionary{TValue}"/> class.
        /// Empty ctor for xml
        /// </summary>
        public CircularDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDictionary{TValue}"/> class.
        /// </summary>
        /// <param name="info">The serialization info as required by style cop</param>
        /// <param name="context">the context of the stream service as required by style cop</param>
        protected CircularDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // this should be implemented if im to ever serialize this
        }

        /// <summary>
        /// The indexer of the collection
        /// </summary>
        /// <param name="i">the index of the desired element</param>
        /// <returns>return an element of TValue with the index of i</returns>
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
