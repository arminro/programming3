// <copyright file="Player.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System.Collections.ObjectModel;
    using System.Windows;

    public enum PlayerState
    {
        Neutral = 0, Rollagain = 1, MissARound = 2, Lost = 3
    }

    public class Player
    {
        private string puppetKey;
        private Point currentposition;
        private ObservableCollection<Subject> subjects; // array to store elements
        private int currentCard; // storing the card the player sits on
        private string name;
        private int order;
        private int money;
        private PlayerState state;

        public Player(string puppetKey, int order, string name = "Player")
        {
            this.puppetKey = puppetKey;
            this.currentCard = 0;
            this.money = 10000;
            this.subjects = new ObservableCollection<Subject>();
            this.State = PlayerState.Neutral;
            this.order = order;
            this.name = name;
        }

        public Player(string puppetKey, Point currentposition, int order,  string name = "Player")
        {
            this.puppetKey = puppetKey;
            this.currentposition = currentposition;
            this.money = 20000;
            this.subjects = new ObservableCollection<Subject>();

            this.currentCard = 0;
            this.name = name;

            name += $" {order}";
        }

        public Player()
        {
            // empty ctor for xml
        }

        public string PuppetKey
        {
            get
            {
                return this.puppetKey;
            }

            set
            {
                this.puppetKey = value;
            }
        }

        public Point Currentposition
        {
            get
            {
                return this.currentposition;
            }

            set
            {
                this.currentposition = value;
            }
        }

        public ObservableCollection<Subject> Subjects
        {
            get
            {
                return this.subjects;
            }

            set
            {
                this.subjects = value;
            }
        }

        public int CurrentCard
        {
            get
            {
                return this.currentCard;
            }

            set
            {
                this.currentCard = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;

                // OnPropertyChanged();
            }
        }

        public int Order
        {
            get
            {
                return this.order;
            }
        }

        public int Money
        {
            get
            {
                return this.money;
            }

            set
            {
                this.money = value;
            }
        }

        public PlayerState State
        {
            get
            {
                return this.state;
            }

            set
            {
                this.state = value;
            }
        }

        // public bool Subjects { get => subjects; set => subjects = value; }

       // public override bool Equals(object obj)
       // {
       //     Player other = obj as Player;
       //     return other.Name == name && other.Order == order;
       // }
        // public override int GetHashCode()
        // {
        //    return this.Name.GetHashCode() ^ this.Order.GetHashCode();
        // }
        public override string ToString()
        {
            return string.Format($"{this.Name}[ {this.PuppetKey}]");
        }
    }
}
