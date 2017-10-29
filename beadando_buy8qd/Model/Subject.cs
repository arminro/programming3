using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.Model
{
    public class Subject
    {
        public Subject(string name, int price)
        {
            this.Name = name;
            this.Price = price;
        }

        public string Name { get; set; }
        public int Price { get; set; }

    }
}
