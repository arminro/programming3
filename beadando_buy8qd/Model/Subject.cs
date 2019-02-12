// <copyright file="Subject.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    /// <summary>
    /// The class represents the subjects that can be enrolled by the players
    /// </summary>
    public class Subject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Subject"/> class.
        /// </summary>
        /// <param name="name">the name of the subject</param>
        /// <param name="price">the price of the subject</param>
        public Subject(string name, int price)
        {
            this.Name = name;
            this.Price = price;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Subject"/> class.
        /// </summary>
        public Subject()
        {
            // empty ctor for xml
        }

        /// <summary>
        /// Gets or sets the name of the subject
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price of the subject
        /// </summary>
        public int Price { get; set; }
    }
}
