// <copyright file="BL.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Text;
    using System.Windows;
    using System.Windows.Threading;
    using Beadando.Model;
    using Polenter.Serialization;

    /// <summary>
    /// The Business Logic part of the View Model
    /// </summary>
    public class BL : Bindable
    {
        private Metrics met;

        private int turn;  // marks the turns in the game

        private int roundCounter;

        private CircularDictionary<BoardField> gameBoard; // dict to hold the cards of the board

        private Dictionary<string, object> eventCardTexts; // dict to store the text elements for the cards

        private Dictionary<string, Action> eventActionsSingle; // dict to store the actions that has the same signature

        private Dictionary<int, string> eventMapper; // maps the string in the event resource dictionary to their corresponding order in constants

        private Action[] neptunActions; // storing actions for the neptun
        // holds the subjects for the players, we use a more flexible data type that is more flexible than arrays here so we can add and remove them runtime
        // also, the original lists do not need to be tempered with
        private int leftVerticalAlign;
        private int incrementAtMovement;
        private int upperVerticalAlign;
        private int rightVerticalAlign;

        private int numberOfElementsInAHorizontalRow;
        private int numberOfElementsInAVerticalRow;

        private int heigthOfTheBoard;
        private int widthOfTheBoard;
        private float puppetDiameter; // the diameter of the player puppet
        private float puppetDiameterChangeConstant; // the constant to which the diameter of the puppet changes

        private Dictionary<string, ObservableCollection<Subject>> subjects;

        private Random rand;

        private CircularList<Player> players;

        private string[] resourceNamesNormal;

        private string[] resourceNamesSquare;

        private string[] resourceNamesHorizontal;

        private int movementSpeed;

        private string[] greens;

        private string[] yellows;

        private string[] blues;

        private int? indexOfEventCardCollection;

        private string selectedItem;

        private Player p; // reference to the actual player

        private ObservableCollection<Subject> subjectsOfPlayer; // represents the subjects of the player

        private ObservableCollection<Subject> subjectsAvailableToPlayer; // represents the subjects available to the player
        private Subject selectedSubject; // represetns the subject the player selected in the subject window
        private bool canPlayerBuyIt;

        /// <summary>
        /// Initializes a new instance of the <see cref="BL"/> class.
        /// </summary>
        /// <param name="met">Reference to a metrics instance</param>
        public BL(Metrics met)
        {
            this.Met = met;
            this.greens = new string[] { "go", "start", "roll", "einstein" };

            // blacks = new string[] { "uv", "randi", };
            this.yellows = new string[] { "event" };
            this.blues = new string[] { "enroll", "megajanlott", "neptun", "lead" };

            this.StartX = (int)this.Met.StartPosition.X;
            this.StartY = (int)this.Met.StartPosition.Y;

            this.MovementSpeed = 10;
            this.PuppetDiameter = 30;
            this.IncrementAtMovement = 1;
            this.OffsetHorizontal = 0;
            this.OffsetVertical = 0;
            this.CanPlayerCallNextRound = true;

            // numberOfElementsInAHorizontalRow = met.NumberOfElementsInAHorizontalRow;
            // numberOfElementsInAVerticalRow = met.NumberOfElementsInAVerticalRow;
            this.ResourceNamesNormal = new string[] { "go", "event", "roll", "enroll", "uv", "randi", "neptun" };
            this.ResourceNamesSquare = new string[] { "megajanlott", "lead", "einstein" };
            this.ResourceNamesHorizontal = new string[] { "go_horizontal", "event_horizontal", "roll_horizontal", "enroll_horizontal", "uv_horizontal", "randi_horizontal", "neptun_horizontal" };
            this.Rand = new Random();
            this.GameBoard = new CircularDictionary<BoardField>();
            this.Subjects = new Dictionary<string, ObservableCollection<Subject>>();
            this.Players = new CircularList<Player>();
            this.SaveFolderPath = this.GetSaveDirectory();
            this.RollButtonEnabled = true;

            // adding the texts of the events from the data in MODEL
            this.EventCardTexts = new Dictionary<string, object>
            {
                { "go", Constants.Go },
                { "megajanlott", Constants.Megajanlott },
                { "start", Constants.Start },
                { "roll", Constants.Roll },
                { "randi", Constants.Randi },
                { "enroll", Constants.Enroll },
                { "einstein", Constants.Einstein },
                { "uv", Constants.AlpotProp },
                { "lead", Constants.Lead },
                { "event", Constants.Hallgatoevents },
                { "neptun", Constants.NeptunMessages }
            };

            this.EventActions_Single = new Dictionary<string, Action>
            {
                {
                    "start",
                    () =>
                    {
                        this.AddMoney(this.Player, 5000);
                        this.Refresh();
                    }
                },
                {
                    "roll",
                    () =>
                    {
                        this.Player.State = PlayerState.Rollagain;
                        this.NextRound();
                        this.Refresh();
                    }
                },
                {
                    "uv",
                    () =>
                    {
                        this.RemoveMoney(this.Player, 4000);
                        this.Refresh();
                    }
                },
                {
                    "randi",
                    () =>
                    {
                        this.RemoveMoney(this.Player, 5000);
                        this.Refresh();
                    }
                },
                {
                    "einstein",
                    () =>
                    {
                        this.AddMoney(this.Player, 20000);
                        this.Refresh();
                    }
                },
                {
                    "lead",
                    () =>
                    {
                        this.DismissRandomSubject();
                    }
                },
                {
                    "megajanlott",
                    () =>
    {
        this.InitializeSubjectTransactions(true, true);
    }
                }
            };
            this.NeptunActions = new Action[Constants.NeptunMessages.Length];

            this.NeptunActions[0] = () =>
            {
                this.Player.State = PlayerState.MissARound;
            };
            this.NeptunActions[1] = () =>
            {
                this.ArriveAtPosition(0);
                this.Refresh();
                this.EventCard?.Invoke(this, new CardEventArgs(this.GameBoard[this.Player.CurrentCard].ImageKey, this.Player));
            };
            this.NeptunActions[2] = () =>
            {
                this.Win();
            };
            this.NeptunActions[3] = () =>
            {
                this.RemoveMoney(this.Player, 7000);
                this.Refresh();
            };

            // loading the eventmapper with the names of the events to be invoked
            this.EventMapper = new Dictionary<int, string>();
            for (int i = 0; i < Constants.Hallgatoevents_Nevek.Length; i++)
            {
                this.EventMapper.Add(i, Constants.Hallgatoevents_Nevek[i]);
            }

            // millis = new List<long>();
            this.SetMetrics();

            // if we do not yet have elements in the gameboard, it is a new game, so generate it
            // if there are already elements, then it is a load and loading has already taken care of filling GB with data
            // P.Currentposition = GameBoard[0].Rect;
            this.Players = new CircularList<Player>
            {
                // adding the first player
                new Player("lel", 0, "Player 1")
            }; // the max number of players is 3
            this.PlayerTokens = new ObservableCollection<string>();
            for (int i = 0; i < Constants.PlayerTokens.Length; i++)
            {
                this.PlayerTokens.Add(Constants.PlayerTokens[i]);
            }

            // The roundcounter only ever increments if the turn changed
            this.PropertyChanged += (object sender, PropertyChangedEventArgs prop) =>
            {
                if (prop.PropertyName == "Turn")
                {
                    if (this.Turn % this.Players.Count == 0)
                    {
                        this.RoundCounter++;
                    }
                }
            };
        }

        /// <summary>
        /// provides a way to invalidate the visual from hte view
        /// </summary>
        public event EventHandler Invalidate;

        /// <summary>
        /// Provides a way to invoke the event in the view
        /// </summary>
        public event EventHandler<CardEventArgs> EventCard;

        /// <summary>
        /// Provides a way to invoke the subjects window
        /// </summary>
        public event EventHandler<SubjectEventArgs> InitiateSubjectTransaction;

        /// <summary>
        /// Provides a way to notify the player of an event
        /// </summary>
        public event EventHandler<TransferEventArgs> NotifyPlayer;

        /// <summary>
        /// Provides a way to close the open windows
        /// </summary>
        public event EventHandler<EventArgs> CloseOpenWindows;

        /// <summary>
        /// Signals that the game is now finished
        /// </summary>
        public event EventHandler<EventArgs> FinishedGame;

        /// <summary>
        /// Provides a way to push general notifications concerning the gameplay itself
        /// </summary>
        public event EventHandler<TransferEventArgs> GeneralNotification;

        /// <summary>
        /// Provides a way to push notifications that only the load window will reply to
        /// </summary>
        public event EventHandler<TransferEventArgs> LoadNotification;

        /// <summary>
        /// Gets or sets  the item selected by the user in the New Game win
        /// </summary>
        public string SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.selectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the turn of the players
        /// </summary>
        public int Turn
        {
            get
            {
                return this.turn;
            }

            set
            {
                this.turn = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the height of the gaming area
        /// </summary>
        public int HeigthOfTheBoard
        {
            get
            {
                return this.heigthOfTheBoard;
            }

            set
            {
                this.heigthOfTheBoard = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the gaming area
        /// </summary>
        public int WidthOfTheBoard
        {
            get
            {
                return this.widthOfTheBoard;
            }

            set
            {
                this.widthOfTheBoard = value;
            }
        }

        /// <summary>
        /// Gets or sets the instance of the Metric class
        /// </summary>
        public Metrics Met
        {
            get
            {
                return this.met;
            }

            set
            {
                this.met = value;
            }
        }

        /// <summary>
        /// Gets or sets the resources used to populate normal card names
        /// </summary>
        public string[] ResourceNamesNormal
        {
            get
            {
                return this.resourceNamesNormal;
            }

            set
            {
                this.resourceNamesNormal = value;
            }
        }

        /// <summary>
        /// Gets or sets the resources used to populate square card names
        /// </summary>
        public string[] ResourceNamesSquare
        {
            get
            {
                return this.resourceNamesSquare;
            }

            set
            {
                this.resourceNamesSquare = value;
            }
        }

        /// <summary>
        /// Gets or sets the resources used to populate horizontally aligned card names
        /// </summary>
        public string[] ResourceNamesHorizontal
        {
            get
            {
                return this.resourceNamesHorizontal;
            }

            set
            {
                this.resourceNamesHorizontal = value;
            }
        }

        /// <summary>
        /// Gets or sets the start coordinate X of the game area
        /// </summary>
        public int StartX { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the palyer can call for the next round
        /// </summary>
        public bool CanPlayerCallNextRound { get; set; }

        /// <summary>
        /// Gets or sets the start coordinate Y of the game area
        /// </summary>
        public int StartY { get; set; }

        /// <summary>
        /// Gets or sets the rounds shown to the player
        /// </summary>
        public int RoundCounter
        {
            get
            {
                return this.roundCounter;
            }

            set
            {
                this.roundCounter = value;
            }
        }

        /// <summary>
        /// Gets or sets the dict storing the subjects of each player
        /// </summary>
        public ObservableCollection<Subject> SubjectsOfPlayer
        {
            get
            {
                return this.subjectsOfPlayer;
            }

            set
            {
                this.subjectsOfPlayer = value;
            }
        }

        /// <summary>
        /// Gets or sets the temp var storing subjects that can be acquired by the player on a subject dialog session
        /// </summary>
        public ObservableCollection<Subject> SubjectsAvailableToPlayer
        {
            get
            {
                return this.subjectsAvailableToPlayer;
            }

            set
            {
                this.subjectsAvailableToPlayer = value;
            }
        }

        /// <summary>
        /// Gets or sets the temp var storing the subject that the player selected
        /// </summary>
        public Subject SelectedSubject
        {
            get
            {
                return this.selectedSubject;
            }

            set
            {
                this.selectedSubject = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the player has enough money to buy a subject
        /// </summary>
        public bool CanPlayerBuyIt
        {
            get
            {
                return this.canPlayerBuyIt;
            }

            set
            {
                this.canPlayerBuyIt = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the text shown to the winner
        /// </summary>
        public string Winner { get; set; }

        /// <summary>
        /// Gets or sets the temp var that stores the index of one of the cards when the player steps on a card with multiple outcomes (yellow event, neptun cards)
        /// </summary>
        public int? IndexOfEventCardCollection
        {
            get
            {
                return this.indexOfEventCardCollection;
            }

            set
            {
                this.indexOfEventCardCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the var to which the roll button enabled property is bound
        /// </summary>
        public bool RollButtonEnabled { get; set; }

        /// <summary>
        /// Gets or sets a temp var storing all the elements that are going to be serialized by Save()
        /// </summary>
        public List<object> Ser { get; set; }

        /// <summary>
        /// Gets or sets the string resresenting the selected path of the save
        /// </summary>
        public string SelectedPath { get; set; }

        /// <summary>
        /// Gets or sets a temp col storing all the saves for the load window
        /// </summary>
        public ObservableCollection<string> Saves { get; set; }

        /// <summary>
        /// Gets or sets the path for the save folder in documents
        /// </summary>
        public string SaveFolderPath { get; set; }

        /// <summary>
        /// Gets or sets the storing the names of the players
        /// </summary>
        public ObservableCollection<string> PlayerTokens { get; set; }

        /// <summary>
        /// Gets or sets the player seleced by the user on a session of new game
        /// </summary>
        public Player NewGameSelectedPlayer { get; set; }

        /// <summary>
        /// Gets or setsa random number
        /// </summary>
        public int RandomGeneratedNumber { get; set; }

        /// <summary>
        /// Gets or sets the player of the game
        /// </summary>
        public Player Player
        {
            get
            {
                return this.p;
            }

            set
            {
                this.p = value;
            }
        }

        /// <summary>
        /// Gets or sets the int storing the X of the game board
        /// </summary>
        public int LeftVerticalAlign
        {
            get
            {
                return this.leftVerticalAlign;
            }

            set
            {
                this.leftVerticalAlign = value;
            }
        }

        /// <summary>
        /// Gets or sets a var used to coordinate the movement animation
        /// </summary>
        public int IncrementAtMovement
        {
            get
            {
                return this.incrementAtMovement;
            }

            set
            {
                this.incrementAtMovement = value;
            }
        }

        /// <summary>
        /// Gets or sets the var used in drawing the upper area of the game board
        /// </summary>
        public int UpperVerticalAlign
        {
            get
            {
                return this.upperVerticalAlign;
            }

            set
            {
                this.upperVerticalAlign = value;
            }
        }

        /// <summary>
        /// Gets or sets the var used in drawing the right vertical area of the game board
        /// </summary>
        public int RightVerticalAlign
        {
            get
            {
                return this.rightVerticalAlign;
            }

            set
            {
                this.rightVerticalAlign = value;
            }
        }

        /// <summary>
        /// Gets or sets the var storing the diameter of the puppets
        /// </summary>
        public float PuppetDiameter
        {
            get
            {
                return this.puppetDiameter;
            }

            set
            {
                this.puppetDiameter = value;
            }
        }

        /// <summary>
        /// Gets or sets the var used in changeing the dimater of the puppet in relation to the gameboard, would have been used in Zoom()
        /// </summary>
        public float PuppetDiameterChangeConstant
        {
            get
            {
                return this.puppetDiameterChangeConstant;
            }

            set
            {
                this.puppetDiameterChangeConstant = value;
            }
        }

        /// <summary>
        /// Gets or sets he var storing the movement speed
        /// </summary>
        public int MovementSpeed
        {
            get
            {
                return this.movementSpeed;
            }

            set
            {
                this.movementSpeed = value;
            }
        }

        /// <summary>
        /// Gets or sets the Circular dict storing gameboard-related data
        /// </summary>
        public CircularDictionary<BoardField> GameBoard
        {
            get
            {
                return this.gameBoard;
            }

            set
            {
                this.gameBoard = value;
            }
        }

        /// <summary>
        /// Gets the circular list storing players
        /// </summary>
        public CircularList<Player> Players
        {
            get
            {
                return this.players;
            }

            private set
            {
                this.players = value;
            }
        }

        /// <summary>
        /// Gets or sets the values responsible for moving the ui with the board on the x axis
        /// </summary>
        public int OffsetHorizontal { get; set; }

        /// <summary>
        /// Gets or sets the values responsible for moving the ui with the board on the y axis
        /// </summary>
        public int OffsetVertical { get; set; }

        /// <summary>
        /// Gets or sets the collection storing the texts for the events either as plain string, string[] or int[]
        /// </summary>
        public Dictionary<string, object> EventCardTexts
        {
            get
            {
                return this.eventCardTexts;
            }

            set
            {
                this.eventCardTexts = value;
            }
        }

        /// <summary>
        /// Gets or sets the actions happening when the player arrives on the corresponding cards
        /// </summary>
        public Dictionary<string, Action> EventActions_Single
        {
            get
            {
                return this.eventActionsSingle;
            }

            set
            {
                this.eventActionsSingle = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection storing all the subjects in the game
        /// </summary>
        public Dictionary<string, ObservableCollection<Subject>> Subjects
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
        /// Gets or sets the number of cards in a horizontal row
        /// </summary>
        public int NumberOfElementsInAHorizontalRow
        {
            get
            {
                return this.numberOfElementsInAHorizontalRow;
            }

            set
            {
                this.numberOfElementsInAHorizontalRow = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of elements in a vertical row
        /// </summary>
        public int NumberOfElementsInAVerticalRow
        {
            get
            {
                return this.numberOfElementsInAVerticalRow;
            }

            set
            {
                this.numberOfElementsInAVerticalRow = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection that maps the string in the event resource dictionary to their corresponding order in constants. Now that I see it, it might have been a plain ass array as well.
        /// </summary>
        public Dictionary<int, string> EventMapper
        {
            get
            {
                return this.eventMapper;
            }

            set
            {
                this.eventMapper = value;
            }
        }

        /// <summary>
        /// Gets or sets the text of the turorial shown to the player on clicking the Jatekszabalyok button
        /// </summary>
        public string TutorialText { get; set; }

        /// <summary>
        /// Gets or sets the collection storing the actions that are invoked when the player steps on a card
        /// </summary>
        public Action[] NeptunActions
        {
            get
            {
                return this.neptunActions;
            }

            set
            {
                this.neptunActions = value;
            }
        }

        /// <summary>
        /// Gets or sets the class responsible for generating random numbers
        /// </summary>
        public Random Rand
        {
            get
            {
                return this.rand;
            }

            set
            {
                this.rand = value;
            }
        }

        /// <summary>
        /// Gets or sets the temp dictionary used for serializing the gameboard
        /// </summary>
        public Dictionary<int, BoardField> GameBoard_TempForSerilaize { get; set; }

        /// <summary>
        /// Clears the previous data when the user presses the MEGSE button
        /// </summary>
        public void ClearPreviousData()
        {
            this.GameBoard.Clear();
            this.Players.Clear();
            this.Subjects.Clear();
        }

        /// <summary>
        /// Set the text displayed on the tutorial window
        /// </summary>
        public void SetTutorialText()
        {
            // sets the toturial text if it has not been set yet
            if (this.TutorialText == null)
            {
                this.TutorialText = Constants.TutorialProp;
            }
        }

        /// <summary>
        /// Set the data for the subjectwindow to handle
        /// </summary>
        /// <param name="isSubjectForFree">Checks if the player is about to get the subject for free</param>
        /// <param name="weNeedEvent">Checks if the window is to be invoked in the view or in the view model, T for view model</param>
        public void InitializeSubjectTransactions(bool isSubjectForFree, bool weNeedEvent)
        {
            /*If we need to initiate a new subject transaction from a view element,
             we must deal with actually showing the windows right where we are going to use them and
             pass false to weneedevent.
             If we initiate it from the view model, we need the even to tunnel the showing of the windows to a view element and
             we pass true to weneedevent.*/
            // we initialize the data needed to show the subjects
            this.SubjectsAvailableToPlayer = this.Subjects[this.Player.PuppetKey];
            this.SubjectsOfPlayer = this.Player.Subjects;
            this.SelectedSubject = null;
            this.CanPlayerBuyIt = true;

            if (weNeedEvent)
            {
                this.InitiateSubjectTransaction?.Invoke(this, new SubjectEventArgs(this.Player, this.SubjectsAvailableToPlayer, isSubjectForFree));
            }
        }

        /// <summary>
        /// Checks if the player has enough money to buy a subject
        /// </summary>
        /// <param name="v">The subject, passed as an objectm since the view has no idea of a class defined in the model</param>
        /// <param name="isIsForFree">Check if the subject is for free</param>
        public void CanPlayerBuySubject(object v, bool isIsForFree)
        {
            // if the subject is for free, we can buy it regardless of the aount of money we have
            if (isIsForFree)
            {
                this.CanPlayerBuyIt = true;
            }
            else
            {
                Subject s = v as Subject;
                if (this.Player.Money - s.Price > 0)
                {
                    this.CanPlayerBuyIt = true;
                }
                else
                {
                    this.CanPlayerBuyIt = false;
                }
            }
        }

        /// <summary>
        /// Deletes a save file
        /// </summary>
        /// <param name="selectedPath">the full path of the save file</param>
        public void DeleteSave(string selectedPath)
        {
            if (selectedPath != null)
            {
                FileInfo info = new FileInfo(selectedPath);
                info.Delete();
                this.Saves.Remove(selectedPath);
            }
            else
            {
                this.LoadNotification?.Invoke(this, new TransferEventArgs("Nem jelöltél ki semmit!"));
            }
        }

        /// <summary>
        /// Checks if the player can get course for free
        /// </summary>
        /// <param name="indexNumberForCols">the index of the specific event in the array events to be compared with the index of the player</param>
        /// <returns>T if the index point to the player playing with the approporiate puppet</returns>
        public bool IsFreeCourseValid(int? indexNumberForCols)
        {
            // if the player has all the subjects, getting a new one is not relevant
            if (this.Player.Subjects.Count == 3)
            {
                this.NotifyPlayer?.Invoke(null, new TransferEventArgs("Az összes tárgyad megvan, így erre nincs szükséged!"));
                return false;
            }

            // the named array in constants contains event names that all contains the name of the corresponing puppetkey (nik, rejto, kando) except for tokos legeny
            return Constants.Hallgatoevents_Nevek[(int)indexNumberForCols].Contains(this.Player.PuppetKey);
        }

        /// <summary>
        /// Sets the win text and fire the event for winning
        /// </summary>
        public void Win()
        {
            this.Winner = $"GRATULÁLUNK, {this.Player.Name}\nMEGNYERTED A JÁTÉKOT!";
            this.FinishedGame?.Invoke(null, null);
        }

        /// <summary>
        /// It is used for the Go function to get the position out of the go string
        /// </summary>
        /// <param name="textToBeDisplayed">the text if the window to be displayed, when the plyers steps on the menj elore card </param>
        /// <returns>the number by which the player should advance</returns>
        public int GetPositionFromGostring(string textToBeDisplayed)
        {
            // we get the number from the text and return with the number of the card that number far from the player's current card
            // we know that the last character is the numberic, oc, we could test all the characters and stop where we find a number
            return (int)char.GetNumericValue(textToBeDisplayed.Last());
        }

        /// <summary>
        /// Registers the plyer into a board field (card)
        /// </summary>
        public void ArriveAtRandomPosition()
        {
            this.ArriveAtPosition(this.Rand.Next(0, this.GameBoard.Count));
        }

        /// <summary>
        /// Sets the basic metrics of the gameboard in terms of its width, height and base of positions
        /// </summary>
        public void SetMetrics()
        {
            // the rect marking the outer edge of the left vertical row
            this.LeftVerticalAlign = this.StartX - ((this.Met.NumberOfElementsInAHorizontalRow * this.Met.NormalCardWidth)
                + (this.Met.SquareCardMetric - this.Met.NormalCardWidth));

            // the rect marking the upper edge of the board
            this.UpperVerticalAlign = this.StartY -
                (this.Met.NormalCardWidth * this.Met.NumberOfElementsInAVerticalRow)
                - (this.Met.SquareCardMetric - this.Met.NormalCardWidth);
            this.RightVerticalAlign = this.LeftVerticalAlign + ((this.Met.NumberOfElementsInAHorizontalRow - 1) * this.Met.NormalCardWidth) + this.Met.SquareCardMetric;
        }

        /// <summary>
        /// Seeks for a pre-specified Directory in Documents, creates it, if it does not exist, then saves critical data
        /// </summary>
        public void Save()
        {
            StringBuilder saveFolderPath = new StringBuilder(this.SaveFolderPath);

             this.GameBoard_TempForSerilaize = new Dictionary<int, BoardField>();

             foreach (KeyValuePair<int, BoardField> board in this.GameBoard)
             {
                this.GameBoard_TempForSerilaize.Add(board.Key, board.Value);
             }

            // we store the data in one place, so that the shiny serializer can serialize it into 1 file
            this.Ser = new List<object>
            {
                this.Players,
                this.Subjects,
                this.Player,
                this.RoundCounter,
                this.GameBoard_TempForSerilaize
            };
            SharpSerializer serializer = new SharpSerializer();

            saveFolderPath.Append($"\\{DateTime.Now.ToString("yyyy.MM.dd HH_mm_ss")}.xml");
            serializer.Serialize(this.Ser, saveFolderPath.ToString());
        }

        /// <summary>
        /// Fills a list with save files
        /// </summary>
        public void Peek()
        {
            string docPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Diplomazz Okosan";

            this.Saves = new ObservableCollection<string>();

            // adding all the files found in the saves directory to Saves
            DirectoryInfo info = new DirectoryInfo(docPath);
            foreach (var item in info.GetFiles())
            {
                this.Saves.Add(item.FullName);
            }
        }

        /// <summary>
        /// Modifies the underlying data regarding the position of the player alongside the horizontal plain
        /// </summary>
        /// <param name="offset">The value by which the items are moved</param>
        public void MoveHorizontally(int offset)
        {
            this.StartX += offset;

            foreach (Player player in this.Players)
            {
                player.Currentposition = Point.Add(player.Currentposition, new Vector(offset, 0));
            }

            this.OffsetHorizontal += offset;
            this.SetMetrics();
        }

        /// <summary>
        /// Modifies the underlying data regarding the position of the player alongside the vertical plain
        /// </summary>
        /// <param name="offset">The value by which the items are moved</param>
        public void MoveVertically(int offset)
        {
            this.StartY += offset;
            foreach (Player player in this.Players)
            {
                player.Currentposition = Point.Add(player.Currentposition, new Vector(0, offset));
            }

            this.OffsetVertical += offset;
            this.SetMetrics();
        }

        /// <summary>
        /// Goes to the specified position from the current one.
        /// </summary>
        public void GenerateRandomNumber()
        {
            this.RandomGeneratedNumber = this.Rand.Next(1, 7);
        }

        /// <summary>
        /// Advances the player to the given position
        /// </summary>
        /// <param name="position">The number of steps the player has to take</param>
        public void GoToPosition(int position)
        {
            /*We have to keep stepping from the current position, but the number of steps has to start from 0*/
            int positionHolder = this.Player.CurrentCard; // we start off from the current position of the player
            int startingPosition = this.Player.CurrentCard;
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);
            int incr = 0;

            // if the goal position is in the same round (eg. from 9 to 12) we only have to take just as many steps as is there difference
            // if the goal position is in another round (eg. from 25 to 1)
            // we have to take as many steps as it takes to get to the start + the number of steps taking us to the goal card
            // int boundary = position > Player.CurrentCard ? position - Player.CurrentCard : (GameBoard.Count - Player.CurrentCard) + position;
            int boundary = position;
            timer.Interval = TimeSpan.FromMilliseconds(422); // 422 //kókány: 328.10526
            timer.Start();
            timer.Tick += (o, args) =>
            {
                // we get the number of cards between the player and the destination card

                // we check the next free position available on the destination
                if (incr <= boundary)
                {
                    // we do not calculate the next free position for the card the player stands on
                    this.Step(
                        positionHolder,
                         positionHolder != startingPosition ? this.gameBoard[positionHolder].NextFreePosition : 0);
                    positionHolder++;
                    incr++;
                    this.CanPlayerCallNextRound = false;
                }
                else
                {
                    timer.Stop();

                    // the --positionHolder means that we have to consider that at the end of the last iteration of the Tick...
                    // ...the positionHOlder will point to the card AFTER the one the player stands on
                    this.Player.CurrentCard = this.GetPlayerCardNumber(this.Player, this.gameBoard[--positionHolder].NextFreePosition);
                    this.GameBoard[this.Player.CurrentCard].ArriveAtPosition(this.Player);

                    // we signal that an event has to happen here that a view can handle anyway s/he wants (MVVM <3)
                    this.EventCard?.Invoke(this, new CardEventArgs(this.GameBoard[this.Player.CurrentCard].ImageKey, this.Player));
                    this.CanPlayerCallNextRound = true; // now the player can call for the next round
                    this.Refresh(); // refresh the screen which sets the buttons
                }
            };
        }

        /// <summary>
        /// Calculates the coordinates of the next step, which results in a vector towards the destination. This player makes this one step.
        /// </summary>
        /// <param name="positionNumber">The position on which the player stands</param>
        /// <param name="orderOnTheCard">The order of the player within a card</param>
        public void Step(int positionNumber, int orderOnTheCard)
        {
            // we tell the card on which the player priviously stood that s/he is leaving
            if (this.Player.CurrentCard == positionNumber)
            {
                this.GameBoard[this.Player.CurrentCard].DepartFromposition(this.Player);
            }

            Point goalPosition;

            goalPosition = this.GetPositionOfNextStep(positionNumber, orderOnTheCard);

            Vector v = new Vector(goalPosition.X - this.Player.Currentposition.X, goalPosition.Y - this.Player.Currentposition.Y);

            DispatcherTimer t = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };

            t.Start();

            int c = 0;
            int divide = 20;

            t.Tick += (o, args) =>
            {
                if (c >= divide)
                {
                    t.Stop();
                }
                else
                {
                    this.Player.Currentposition = Point.Add(this.Player.Currentposition, v / divide);
                    this.Refresh(); // we just need a way to convey the invalidate visual from here
                }

                c++;
            };
        }

        /// <summary>
        /// Calculates the next player
        /// </summary>
        public void NextRound()
        {
            // we check if there are players in the game
            if (this.IsTheGameStillOn())
            {
                // in the case of a re-roll, we have to inspect the current player, in all other cases, the next player matters
                if (this.Player.State == PlayerState.Rollagain)
                {
                    this.Player.State = PlayerState.Neutral;
                }
                else if (this.Players.WhatIsNext().State == PlayerState.Neutral)
                {
                    this.Player = this.Players.GetNextElement();
                    this.Turn++;
                }

                // if the next player has to miss the turn, we set back its state bc s/he is going to miss this round
                // then ask for the player coming after
                else if (this.Players.WhatIsNext().State == PlayerState.MissARound)
                {
                    this.Players.WhatIsNext().State = PlayerState.Neutral; // set the state back
                    this.Player = this.Players.GetNextElement(2); // ask for the player after the one who misses a round
                    this.Turn += 2; // now one of the players missed a round, so it takes only 2 players to complete a round
                }
                else if (this.Players.WhatIsNext().State == PlayerState.Lost)
                {
                    this.Player = this.Players.GetNextElement(2); // ask for the player after the one who misses a round
                    this.Turn += 2;
                }

                this.RollButtonEnabled = true;
                this.RandomGeneratedNumber = 0; // at the start of every new round, we set this back to 0
            }
            else
            {
                this.GeneralNotification?.Invoke(this, new TransferEventArgs("Sajnos egyikőtök sincs már játékban, így a játék véget ért"));
                this.Quit();
            }
        }

        /// <summary>
        /// Calculates the coordinates of the main position (order = 0) within a card
        /// </summary>
        /// <param name="cornerRect">The coordinates of the corner of the card</param>
        /// <param name="widthOfCurrentCard">The width of the current card</param>
        /// <param name="heightOfCurrentCard">The height of the current card</param>
        /// <returns>The point that is at the center of the carc</returns>
        public Point CalculatePrimaryPosition(Point cornerRect, int widthOfCurrentCard, int heightOfCurrentCard)
        {
            // center
            // valid for square as well, since their width/height is the same as the normal height
            return Point.Add(cornerRect, new Vector(widthOfCurrentCard / 2, heightOfCurrentCard / 2));
        }

        /// <summary>
        /// Calculates the secondary position (order = 1) of the card
        /// </summary>
        /// <param name="centerOfCard">The center of the card, the primary position</param>
        /// <returns>The secondary position, i.e., a position slightly heigher than the center</returns>
        public Point CalculateSecondaryPosition(Point centerOfCard)
        {
            // up
            return Point.Subtract(centerOfCard, new Vector(0, 60));
        }

        /// <summary>
        /// Caculates the tertial position (order = 2) of the card
        /// </summary>
        /// <param name="centerOfCard">The center of the card, the primary position</param>
        /// <returns>The tertial position, i.e., a position slightly below the center</returns>
        public Point CalculateTertialPosition(Point centerOfCard)
        {
            // down
            return Point.Add(centerOfCard, new Vector(0, 60));
        }

        /// <summary>
        /// Decides if the card with the given index is a square card or not
        /// </summary>
        /// <param name="indexer">The index of the card</param>
        /// <returns>T if the card with the given index is in the collection storing the names of square cards</returns>
        public bool CardIsSquare(int indexer)
        {
            return this.ResourceNamesSquare.Contains(this.GameBoard[indexer].ImageKey);
        }

        /// <summary>
        /// Generates the order of the cards by providing data to the collection GameBoard
        /// </summary>
        public void GenerateOrderOfCards()
        {
            int indexer = 0;
            this.Rand = new Random();

            this.GameBoard.Add(indexer++, new BoardField() { Rect = this.CalculatePrimaryPosition(this.Met.StartPosition, this.Met.SquareCardMetric, this.Met.SquareCardMetric), ImageKey = "start" });

            BoardField b; // reference to a board item so that we can store elements in it
            // generating cards for lower horizontal row
            for (int i = 1; i <= this.Met.NumberOfElementsInAHorizontalRow; i++)
            {
                b = new BoardField();
                if (i % this.Met.NumberOfElementsInAHorizontalRow == 0)
                {
                    // temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = this.ResourceNamesSquare[this.Rand.Next(0, this.ResourceNamesSquare.Length)];
                }
                else
                {
                    // temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = this.ResourceNamesNormal[this.Rand.Next(0, this.ResourceNamesNormal.Length)];
                }

                // add boardcard to the collection of boardscards
                this.GameBoard.Add(indexer++, b);
            }

            // generating cards for left vertical row
            for (int i = 1; i <= this.Met.NumberOfElementsInAVerticalRow; i++)
            {
                b = new BoardField();
                if (i % this.Met.NumberOfElementsInAVerticalRow == 0)
                {
                    // temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = this.ResourceNamesSquare[this.Rand.Next(0, this.ResourceNamesSquare.Length)];
                }
                else
                {
                    // temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = this.ResourceNamesNormal[this.Rand.Next(0, this.ResourceNamesNormal.Length)];
                }

                this.GameBoard.Add(indexer++, b);
            }

            // generating cards for lower horizontal row
            for (int i = 1; i <= this.Met.NumberOfElementsInAHorizontalRow; i++)
            {
                b = new BoardField();
                if (i % this.Met.NumberOfElementsInAHorizontalRow == 0)
                {
                    // temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = this.ResourceNamesSquare[this.Rand.Next(0, this.ResourceNamesSquare.Length)];
                }
                else
                {
                    // temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = this.ResourceNamesNormal[this.Rand.Next(0, this.ResourceNamesNormal.Length)];
                }

                this.GameBoard.Add(indexer++, b);
            }

            // generating cards for right vertical row

            // we have the -1, bc we already have the start card, so we do not need to generate it here
            for (int i = 1; i <= this.Met.NumberOfElementsInAVerticalRow - 1; i++)
            {
                b = new BoardField();
                if (i % this.Met.NumberOfElementsInAVerticalRow == 0)
                {
                    // temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = this.ResourceNamesSquare[this.Rand.Next(0, this.ResourceNamesSquare.Length)];
                }
                else
                {
                    // temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = this.ResourceNamesNormal[this.Rand.Next(0, this.ResourceNamesNormal.Length)];
                }

                this.GameBoard.Add(indexer++, b);
            }

            // to make sure that the game can be won, 1 of the cards are switched to a win card randomly
            // we start form 1 bc the start cannot be switched
            this.GameBoard[this.Rand.Next(1, this.GameBoard.Count)].ImageKey = "enroll";
        }

        /// <summary>
        /// Decides on which category the card with the given key belongs to
        /// </summary>
        /// <param name="cardKey">The key of the card</param>
        /// <returns>string</returns>
        public string EventCategroySelector(string cardKey)
        {
            if (this.greens.Contains(cardKey))
            {
                return "green";
            }
            else if (this.blues.Contains(cardKey))
            {
                return "blue";
            }
            else if (this.yellows.Contains(cardKey))
            {
                return "yellow";
            }
            else
            {
                return "black";
            }
        }

        /// <summary>
        /// Recreates the desired objects from the serialized xml file
        /// </summary>
        /// <param name="fullFilePath">The path of the file</param>
        /// <returns>T if the load was successful</returns>
        public bool Load(string fullFilePath)
        {
            if (fullFilePath != null)
            {
                try
                {
                    this.ClearPreviousData();

                    // deserializing persistent data
                    SharpSerializer serializer = new SharpSerializer();
                    this.Ser = (List<object>)serializer.Deserialize(fullFilePath);
                    this.Players = (CircularList<Player>)this.Ser[0];
                    this.Subjects = (Dictionary<string, ObservableCollection<Subject>>)this.Ser[1];
                    this.Player = (Player)this.Ser[2];
                    this.RoundCounter = (int)this.Ser[3];
                    this.GameBoard_TempForSerilaize = new Dictionary<int, BoardField>();
                    this.GameBoard_TempForSerilaize = (Dictionary<int, BoardField>)this.Ser[4];

                    // getting data out of the normal dictionary
                    foreach (KeyValuePair<int, BoardField> item in this.GameBoard_TempForSerilaize)
                    {
                        this.GameBoard.Add(item.Key, item.Value);
                    }

                    this.GameBoard_TempForSerilaize = null;
                    return true;
                }
                catch (ApplicationException e)
                {
                    this.GeneralNotification?.Invoke(this, new TransferEventArgs(e.Message));
                    return false;
                }
                finally
                {
                    // we set the selected path back, so we can re-use it
                    this.SelectedPath = null;
                }
            }
            else
            {
                this.LoadNotification?.Invoke(this, new TransferEventArgs("Csak úgy tudsz betölteni, ha ki is jelölsz valamit!"));
                return false;
            }
        }

        /// <summary>
        /// Gets out the a pre-specified values from the dictionary that containts event texts and formats it to be displayed. I used Tuples instead of obj[] bc they are cool AF.
        /// </summary>
        /// <param name="cardKey">The key of the card that triggered the event</param>
        /// <returns>A Tuple that stores the string which will be the text, an int which will be the fontsize and an int? which indeicates that if we deal with an array, which element we want to use form it</returns>
        public Tuple<string, int, int?> GetTextToDisplay(string cardKey)
        {
            StringBuilder builder = new StringBuilder();
            object temp = this.EventCardTexts[cardKey];
            Type t = temp.GetType();
            int? indexHolder = null; // represents the index of the collections, but only if it is a collection

            int boundary = 0; // the number of characters allowed in 1 line

            if (t == typeof(string))
            {
                builder = new StringBuilder(temp as string);
            }
            else if (t == typeof(string[]))
            {
                string[] tempArray = temp as string[];

                indexHolder = this.Rand.Next(0, tempArray.Length);
                builder = new StringBuilder(tempArray[(int)indexHolder]);
            }
            else if (t == typeof(int[]))
            {
                int[] tempArray = temp as int[];
                indexHolder = this.Rand.Next(0, tempArray.Length);
                builder = new StringBuilder($"Lépj előre: {tempArray[(int)indexHolder]}");
            }

            // based on the number of characters, we assign a fontsize as well
            int fontsize = 16;

            if (builder.Length < 50)
            {
                fontsize = 30;
            }
            else if (builder.Length >= 50 && builder.Length < 100)
            {
                fontsize = 20;
            }

            // based on the fontsize, we determine how many characters should be in a line (in a not so mathematical manner)
            switch (fontsize)
            {
                case 16:
                    boundary = 40;
                    break;
                case 20:
                    boundary = 30;
                    break;

                default:
                    boundary = 25;
                    break;
            }

            // we append the string with \n in the proper places so they appear to be formatted
            int buffer = 0; // this is used to track the length of the line
            for (int i = 0; i < builder.Length; i++)
            {
                buffer++;
                if (builder[i] == '\n')
                {
                    buffer = 0; // if we arrived at a new line, we reset the buffer
                }

                if (buffer % boundary == 0 && buffer != 0)
                {
                    // we look for the first space character and append the text there
                    // this will be found before the last character to be displayed, so we will count backwards
                    // this way the words will appear to be correct
                    int j = i;
                    while (j > 0 && builder[j] != ' ')
                    {
                        j--;
                    }

                    if (builder[j] != '\n')
                    {
                        builder.Insert(j, "\n");
                        builder.Remove(j + 1, 1); // we delete the space after the word, becuase we no longer need it due to the new line
                    }
                }
            }

            // else it remains 12
            return Tuple.Create<string, int, int?>(builder.ToString(), fontsize, indexHolder);
        }

        /// <summary>
        /// Ads a subject to the subjects of the player
        /// </summary>
        /// <param name="p">reference ot the player</param>
        /// <param name="subject">reference to the subject to be added</param>
        /// <param name="free">indicates if the subject is for free</param>
        public void AddSubjectToPlayer(Player p, Subject subject, bool free)
        {
            p.Subjects.Add(subject); // adding to the subject of the player
            this.Subjects[p.PuppetKey].Remove(subject); // remove the item from the list of availables
            if (!free)
            {
                this.RemoveMoney(p, subject.Price);
            }

            // we check if the player could win now
            if (this.CheckWinningConditions(p))
            {
                this.Win();
            }
        }

        /// <summary>
        /// Refreshes the UI whatever the implementation might be
        /// </summary>
        public void Refresh()
        {
            this.Invalidate?.Invoke(null, null);
        }

        /// <summary>
        /// The player will appear on the designated position
        /// </summary>
        /// <param name="pos">The number of the card the player is about to arrive</param>
        public void ArriveAtPosition(int pos)
        {
            this.GameBoard[this.Player.CurrentCard].DepartFromposition(this.Player); // leave the current card
            this.Player.Currentposition = this.GetPositionOfNextStep(pos, this.GameBoard[pos].NextFreePosition); // set current position
            this.Player.CurrentCard = pos; // set current card
            this.GameBoard[this.Player.CurrentCard].ArriveAtPosition(this.Player); // arrive at position
        }

        /// <summary>
        /// Ads money to the player
        /// </summary>
        /// <param name="p">reference to the player</param>
        /// <param name="sum">the sum of money given to the player</param>
        public void AddMoney(Player p, int sum)
        {
            p.Money += sum;
        }

        /// <summary>
        /// Adds a player to the collection of players
        /// </summary>
        public void AddPlayer()
        {
            if (this.Players.Count < 3)
            {
                // we add a player with bogus puppetkey that we will change later
                this.Players.Add(new Player("lel", this.Players.Count + 1, $"Player {this.Players.Count + 1}"));
            }
        }

        /// <summary>
        /// Cheks if all the added players have valid data
        /// </summary>
        /// <returns>T if all the players have valid data</returns>
        public bool ValidateStartNewGame()
        {
            if (this.Players != null && this.Players.Count > 0)
            {
                int idx = 0;

                // we added players with a bogus puppetkey that we change when the user selects its puppetkey
                while (idx < this.Players.Count && this.Players[idx].PuppetKey != "lel")
                {
                    idx++;
                }

                return idx >= this.Players.Count;
            }

            return false;
        }

        /// <summary>
        /// Deletes a player from the collection
        /// </summary>
        /// <param name="p">reference to the player</param>
        public void DeletePlayer(Player p)
        {
            if (this.Players.Count > 1)
            {
                this.Players.Remove(p);
            }
        }

        /// <summary>
        /// This must run before any gameplay either new or loaded
        /// </summary>
        public void InitializeGame()
        {
            if (this.GameBoard.Count == 0)
            {
                this.GenerateOrderOfCards();
            }

            // if it is a new game, we will start it from 1 instead of 0
            // if it is a load, the loading has already loaded data, so it will not be 0
            if (this.RoundCounter == 0)
            {
                this.RoundCounter++;
            }
        }

        /// <summary>
        /// This must run before any NEW game, but not before loaded ones
        /// </summary>
        public void InitializeStartOfNewGame()
        {
            foreach (Player p in this.Players)
            {
                // we only load the subjects we need
                if (!this.Subjects.ContainsKey(p.PuppetKey))
                {
                    this.Subjects.Add(p.PuppetKey, new ObservableCollection<Subject>());
                }

                // only works because we habe 3 subjects
                if (this.Subjects[p.PuppetKey].Count < 3)
                {
                    switch (p.PuppetKey)
                    {
                        case "nik":
                            for (int i = 0; i < Constants.Nik_Targyak.Length; i++)
                            {
                                // adding the subjects with  prices
                                this.Subjects["nik"].Add(new Subject(Constants.Nik_Targyak[i], (i + 1) * 1000));
                            }

                            break;

                        case "rejto":
                            for (int i = 0; i < Constants.Rejto_Targyak.Length; i++)
                            {
                                // adding the subjects with  prices
                                this.Subjects["rejto"].Add(new Subject(Constants.Rejto_Targyak[i], (i + 1) * 1000));
                            }

                            break;
                        case "kando":
                            for (int i = 0; i < Constants.Kando_Targyak.Length; i++)
                            {
                                // adding the subjects with  prices
                                this.Subjects["kando"].Add(new Subject(Constants.Kando_Targyak[i], (i + 1) * 1000));
                            }

                            break;
                        default:
                            break;
                    }
                }
            }

            // adding the players to the first card
            for (int i = 0; i < this.Players.Count; i++)
            {
                this.Players[i].CurrentCard = 0;
                BoardField start = this.GameBoard[this.Players[i].CurrentCard];
                this.Players[i].Currentposition = this.GetPositionOfNextStep(0, start.NextFreePosition);
                this.GameBoard[this.Players[i].CurrentCard].ArriveAtPosition(this.Players[i]);
            }

            // set the first player
            this.Player = this.Players[0];
        }

        /// <summary>
        /// Changes the key of the puppet as used in the new game window
        /// </summary>
        /// <param name="p">non-generic reference to the player</param>
        /// <param name="selected">non-generic reference to the selected subject</param>
        public void ChangePuppetKey(object p, object selected)
        {
            // we have to do casting as well as remapping the user-fruendly names to the keyes we used
            // of course we could just changed the keyes themselves, but then we needed to change it everywhere
            string temp = string.Empty;
            string selectedString = selected as string;
            switch (selectedString)
            {
                case "NIK":
                    temp = "nik";
                    break;
                case "KVK":
                    temp = "kando";
                    break;
                case "RKK":
                    temp = "rejto";
                    break;
                default:
                    break;
            }

                (p as Player).PuppetKey = temp;
        }

        /// <summary>
        /// Checks if a player with the puppetkey alread exists. Used for validating that only 1 type of player can be used
        /// </summary>
        /// <param name="name">name of the player</param>
        /// <returns>if the collection of players already has 1 of this type</returns>
        public bool CheckIfApplicable(string name)
        {
            string temp = string.Empty;
            switch (name)
            {
                case "NIK":
                    temp = "nik";
                    break;
                case "KVK":
                    temp = "kando";
                    break;
                case "RKK":
                    temp = "rejto";
                    break;
                default:
                    break;
            }

            return this.Players.FirstOrDefault(pl => pl.PuppetKey == temp) != null;
        }

        /// <summary>
        /// Closes the windows that subscribe to it
        /// </summary>
        public void CloseWindows()
        {
            // we signal that the game now begins
            this.CloseOpenWindows?.Invoke(null, null);
        }

        /// <summary>
        /// Removes a random subject from the subjects collection of the player or signals that the player has no subjects yet
        /// </summary>
        public void DismissRandomSubject()
        {
            string message = string.Empty;
            if (this.Player.Subjects.Count > 0)
            {
                Subject sub = this.Player.Subjects[this.Rand.Next(0, this.Player.Subjects.Count)];
                this.DismissSubject(sub, this.Player);
                message = $"Eltávolítottuk {sub.Name} nevű tárgyadat!";
            }
            else
            {
                message = "Még nincsenek tárgyaid, ezért ez rád nem érvényes!";
            }

            this.NotifyPlayer?.Invoke(null, new TransferEventArgs(message));
        }

        /// <summary>
        /// Deletes the subject added as a parameter
        /// </summary>
        /// <param name="sub">reference to the subject</param>
        /// <param name="player">reference ot the player</param>
        public void DismissSubject(Subject sub, Player player)
        {
            player.Subjects.Remove(sub);
            this.Subjects[player.PuppetKey].Add(sub);
        }

        /// <summary>
        /// Advances the player with the given number of steps, unused in this build
        /// </summary>
        /// <param name="step">the numner of steps the player has to go</param>
        public void TakeStep(int step)
        {
            this.GoToPosition(this.Player.CurrentCard + step);
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        public void Quit()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Checks if any of the players are in game; returns true if the game can go on
        /// </summary>
        /// <returns>If there are players who did not loose</returns>
        public bool IsTheGameStillOn()
        {
            int i = 0;
            while (i < this.Players.Count && this.Players[i].State == PlayerState.Lost)
            {
                i++;
            }

            return i < this.Players.Count;
        }

        /// <summary>
        /// Checks if the player satisfies the conditions for winning; returns true if the player does
        /// </summary>
        /// <param name="p">The player</param>
        /// <returns>If the player has won</returns>
        public bool CheckWinningConditions(Player p)
        {
            return p.Subjects.Count == 3 && p.Money >= 0;
        }

        /// <summary>
        /// Gets the defualt save folder from the win environment var
        /// </summary>
        /// <returns>Return the string path of the save folder</returns>
        private string GetSaveDirectory()
        {
            // getting the locetion of the Documents
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string saveFolder = "Diplomazz Okosan";
            StringBuilder saveFolderFullPath = new StringBuilder(docPath);
            saveFolderFullPath.Append($"\\{saveFolder}");

            // creating directory if it does not exist
            if (!Directory.Exists(saveFolderFullPath.ToString()))
            {
                Directory.CreateDirectory(saveFolderFullPath.ToString());
            }

            return saveFolderFullPath.ToString();
        }

        /// <summary>
        /// Formatting the save files to look more visually appealing to the user
        /// </summary>
        /// <param name="unformatted">The full path of the save file</param>
        /// <returns>The formatted string to show</returns>
        private string FormatForSave(string unformatted)
        {
            // split the text
            string[] temp = unformatted.Split('\\');

            // now we have only the name
            StringBuilder builder = new StringBuilder(temp[temp.Length - 1]);

            // we clip the ".xml" part
            builder.Remove(builder.Length - 4, 4);

            // we replace the '_' s with ':'s to make it more user-friendly
            builder.Replace('_', ':');
            return builder.ToString();
        }

        // this was to be a feature, but I could not finish it in time :(
        // public void Zoom(int offset)
        // {
        //     //if we multiply both the height and the width by the same number
        //     //the normal ratio is distorted (since the values are approaching each other)
        //     //5/3 --> the original height x width is 135x90 which means that height : width = 2 / 3
        //     //height has to change more than width, so it has to increase by the offsset + the difference in their ratio
        //     //this is taking 1 2/3 of the offset which is 5/3 * offset
        //
        //     Metrics.NormalCard.Height += (offset * 5 / 3);
        //     Met.NormalCardWidth += offset;
        //     Met.SquareCardMetric += (offset * 5 / 3);
        //     //StartY -= offset;
        //     //StartPosition -= offset;
        //
        //     float temp = ((float)(Met.NormalCardWidth + offset) / (float)Met.NormalCardWidth);
        //     PuppetDiameter *= temp;
        //
        //     //refreshing the position of the puppet so that it moves with the board (the changing board would move away from it)
        //     Player.Currentposition = GameBoard[Player.CurrentCard].Rect;
        //     SetMetrics();
        // }

        /// <summary>
        /// Returns the number of the card the player currently stands on
        /// </summary>
        /// <param name="p">The player</param>
        /// <param name="positionOrder">The order of the player standing on the card</param>
        /// <returns>The number of the card the player stands on</returns>
        private int GetPlayerCardNumber(Player p, int positionOrder)
        {
            return this.GameBoard.Where(g =>
            this.IsThePlayerInTheVicinity(this.GetOrderRect(g.Value.Rect, positionOrder), p.Currentposition) == true)
            .FirstOrDefault().Key;
        }

        /// <summary>
        /// Based on the order, calculates which position the player stands on
        /// </summary>
        /// <param name="originalRect">The positon 0 (center) of the cards</param>
        /// <param name="order">The order of the player on the card</param>
        /// <returns>The exact point the player should stand on</returns>
        private Point GetOrderRect(Point originalRect, int order)
        {
            switch (order)
            {
                case 0:
                    return originalRect;
                case 1:
                    return this.CalculateSecondaryPosition(originalRect);
                case 2:
                    return this.CalculateTertialPosition(originalRect);
                default:
                    return originalRect;
            }
        }

        private bool IsThePlayerInTheVicinity(Point cardRect, Point playerCurrentPosition)
        {
            // the player stands on the card if it is witin a diameter of 10 units
            int vicinity = 10;
            double temp = Math.Abs(Point.Subtract(cardRect, playerCurrentPosition).Length);
            return temp <= vicinity;
        }

        /// <summary>
        /// Calculates the position of the next step
        /// </summary>
        /// <param name="positionNumber">The number of the card the player is about to arrive on</param>
        /// <param name="orderInCard">The order of the player within the card indexed from 0</param>
        /// <returns>Point position of the next step</returns>
        private Point GetPositionOfNextStep(int positionNumber, int orderInCard)
        {
            switch (orderInCard)
            {
                // if the player is the 1st to stand on the card
                case 0:
                    return this.GameBoard[positionNumber].Rect;

                // if the player is 2nd on the card
                case 1:
                    return this.CalculateSecondaryPosition(this.GameBoard[positionNumber].Rect);

                // otherwise, which is if the player is 3rd on the card
                default:
                   return this.CalculateTertialPosition(this.GameBoard[positionNumber].Rect);
            }
        }

        private void RemoveMoney(Player p, int sum)
        {
            if ((p.Money - sum) > 0)
            {
                p.Money -= sum;
            }
            else
            {
                p.State = PlayerState.Lost;
                this.NotifyPlayer?.Invoke(this, new TransferEventArgs("Sajnos a Te számodra véget ért a játék :("));
                this.NextRound();
            }
        }
    }
}
