using Beadando.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Beadando
{
    class Player : Bindable
    {
        string puppetKey;
        Point currentposition;
        bool[] subjects; //array to store elements
        int currentCard; //storing the card the player sits on
        string name;
        int order;
        int money;

        public Player(string puppetKey, string name = "Player")
        {
            this.puppetKey = puppetKey;
            this.currentCard = 0;
            this.money = 20000;
            subjects = new bool[5]; //there are 5 subs
            for (int i = 0; i < subjects.Length; i++)
            {
                subjects[i] = false;
            }
        }

        public Player(string puppetKey, Point currentposition, int order,  string name = "Player")
        {
            this.puppetKey = puppetKey;
            this.currentposition = currentposition;
            this.money = 20000;
            subjects = new bool[3]; //there are 3 subs
            for (int i = 0; i < subjects.Length; i++)
            {
                subjects[i] = false;
            }

            this.currentCard = 0;
            this.name = name;
            this.order = order;
            name += $" {order}";
        }

        public string PuppetKey
        {
            get
            {
                return puppetKey;
            }

          
        }

        public Point Currentposition
        {
            get
            {
                return currentposition;
            }

            set
            {
                currentposition = value;
                OnPropertyChanged();
            }
        }

        public bool[] Subjects
        {
            get
            {
                return subjects;
            }

        }

        public int CurrentCard
        {
            get
            {
                return currentCard;
            }

            set
            {
                currentCard = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public int Order
        {
            get
            {
                return order;
            }

           
        }

        public int Money
        {
            get
            {
                return money;
            }

            set
            {
                money = value;
            }
        }

        public void AddSubject(int subjectNumber)
        {
            subjects[subjectNumber] = true;
        }
        //public bool Subjects { get => subjects; set => subjects = value; }

        public override bool Equals(object obj)
        {
            Player other = obj as Player;
            return other.Name == name && other.Order == order;
        }
        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.Order.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format($"{Name}[ {PuppetKey}]");
        }
    }
}
