// <copyright file="Subject.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    public class Subject
    {
        public Subject(string name, int price)
        {
            this.Name = name;
            this.Price = price;
        }

        public Subject()
        {
            // empty ctor for xml
        }

        public string Name { get; set; }

        public int Price { get; set; }
    }
}
