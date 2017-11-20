// <copyright file="Player.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.Model
{
    using System.Collections.ObjectModel;
    using System.Windows;

    /// <summary>
    /// Enum tracking the state of the player
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// This is the base
        /// </summary>
        Neutral = 0,

        /// <summary>
        /// The player can roll again in the same turn
        /// </summary>
        Rollagain = 1,

        /// <summary>
        /// The player will skip a turn
        /// </summary>
        MissARound = 2,

        /// <summary>
        /// The player is no longer in the game, but others might be
        /// </summary>
        Lost = 3
    }

    /// <summary>
    /// A class representing the players of the game
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="puppetKey">The key of the puppet the player controls</param>
        /// <param name="order">The order in the collection storing all the players, used to write default names (and at some point, was part of a GetHashCode override)</param>
        /// <param name="name">The name of the player</param>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="puppetKey">the key of the puppet the player controls</param>
        /// <param name="currentposition">The position the player is intented to stand on</param>
        /// <param name="order">the player's order in the collection storing all the players </param>
        /// <param name="name">the player's name</param>
        public Player(string puppetKey, Point currentposition, int order,  string name = "Player")
        {
            this.puppetKey = puppetKey;
            this.currentposition = currentposition;
            this.money = 10000;
            this.subjects = new ObservableCollection<Subject>();

            this.currentCard = 0;
            this.name = name;

            name += $" {order}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// Empty ctor for xml
        /// </summary>
        public Player()
        {
            // empty ctor for xml
        }

        /// <summary>
        /// Gets or sets the key of the type of puppet (nik, rkk, kvk)
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current position of the playyer
        /// </summary>
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

        /// <summary>
        /// Gets or sets the list of subjects the player has
        /// </summary>
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

        /// <summary>
        /// Gets or sets the number of the card the player currently stands on
        /// </summary>
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

        /// <summary>
        /// Gets or sets the name of the player
        /// </summary>
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

        /// <summary>
        /// Gets the order var representing the order of the player in the collection that holds all the players
        /// </summary>
        public int Order
        {
            get
            {
                return this.order;
            }
        }

        /// <summary>
        /// Gets or sets the amoun of money available to the player
        /// </summary>
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

        /// <summary>
        /// Gets or sets the state of the player
        /// </summary>
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

        /// <summary>
        /// The tostring ovveride of Player
        /// </summary>
        /// <returns>a string representing the player</returns>
        public override string ToString()
        {
            return string.Format($"{this.Name}[ {this.PuppetKey}]");
        }
    }
}
