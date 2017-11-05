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
        public CircularDictionary()
        {
        }

        protected CircularDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // this should be implemented if im to ever serialize this
        }

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
