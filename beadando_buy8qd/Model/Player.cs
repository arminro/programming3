using Beadando.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Beadando.Model
{
    public enum PlayerState
    {
        neutral = 0, rollagain = 1, missARound = 2, lost = 3
    }

    public class Player : Bindable
    {
        string puppetKey;
        Point currentposition;
        ObservableCollection<Subject> subjects; //array to store elements
        int currentCard; //storing the card the player sits on
        string name;
        int order;
        int money;
        PlayerState state;

        public Player(string puppetKey, int order, string name = "Player")
        {
            this.puppetKey = puppetKey;
            this.currentCard = 0;
            this.money = 20000;
            subjects = new ObservableCollection<Subject>();
            State = PlayerState.neutral;
            this.order = order;
            this.name = name;
        }

        public Player(string puppetKey, Point currentposition, int order,  string name = "Player")
        {
            this.puppetKey = puppetKey;
            this.currentposition = currentposition;
            this.money = 20000;
            subjects = new ObservableCollection<Subject>(); 

            this.currentCard = 0;
            this.name = name;
           
            name += $" {order}";
        }

        public Player()
        {
            //empty ctor for xml
        }

        public string PuppetKey
        {
            get
            {
                return puppetKey;
            }
            set
            {
                puppetKey = value;
                OnPropertyChanged();
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
                
            }
        }

        public ObservableCollection<Subject> Subjects
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
                OnPropertyChanged();
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

        public PlayerState State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
            }
        }

        //public bool Subjects { get => subjects; set => subjects = value; }

       // public override bool Equals(object obj)
       // {
       //     Player other = obj as Player;
       //     return other.Name == name && other.Order == order;
       // }
        //public override int GetHashCode()
        //{
        //    return this.Name.GetHashCode() ^ this.Order.GetHashCode();
        //}

        public override string ToString()
        {
            return string.Format($"{Name}[ {PuppetKey}]");
        }
    }
}
