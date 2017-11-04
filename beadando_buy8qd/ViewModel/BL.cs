using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Beadando.Model;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Polenter.Serialization;
using System.ComponentModel;


namespace Beadando.ViewModel
{
    public class BL : Bindable
    {

        public BL()
        {
            
            StartPosition = 1065;
            LowerHorizontalAlign = 550;
            MovementSpeed = 10;
            PuppetDiameter = 30;
            IncrementAtMovement = 1;
            OffsetHorizontal = 0;
            OffsetVertical = 0;
            numberOfElementsInAHorizontalRow = Constants.numberOfElementsInAHorizontalRow_start;
            numberOfElementsInAVerticalRow = Constants.numberOfElementsInAVerticalRow_start;
            resourceNamesNormal = new string[] { "go", "event", "roll", "enroll", "uv", "randi", "neptun" };
            resourceNamesSquare = new string[] { "megajanlott", "lead", "einstein" };

            greens = new string[] { "go", "start", "roll", "einstein" };
            blacks = new string[] { "uv", "randi", };
            yellows = new string[] { "event" };
            blues = new string[] { "enroll", "megajanlott", "neptun", "lead" };
            Rand = new Random();
            GameBoard = new CircularDictionary<BoardField>();
            Subjects = new Dictionary<string, ObservableCollection<Subject>>();
            Players = new CircularList<Player>();
            SaveFolderPath = GetSaveDirectory();
            RollButtonEnabled = true;


            //adding the texts of the events from the data in MODEL
            EventCardTexts = new Dictionary<string, object>
            {
                { "go", Constants.go },
                { "megajanlott", Constants.megajanlott},
                { "start", Constants.start },
                { "roll", Constants.roll},
                { "randi", Constants.randi },
                { "enroll", Constants.enroll },
                { "einstein", Constants.einstein },
                { "uv", Constants.uv },
                { "lead", Constants.lead },
                { "event", Constants.hallgatoevents },
                { "neptun", Constants.neptunMessages }
            };


            EventActions_Single = new Dictionary<string, Action>();
            EventActions_Single.Add("start", 
                () => 
                {
                    this.AddMoney(this.Player, 5000);
                    Refresh();
                });
            EventActions_Single.Add("roll",
               () =>
               {
                   this.Player.State = PlayerState.rollagain;
               });
            EventActions_Single.Add("uv",
               () =>
               {
                   this.RemoveMoney(this.Player, 4000);
                   Refresh();
               });
            EventActions_Single.Add("randi",
               () =>
               {
                   this.RemoveMoney(this.Player, 5000);
                   Refresh();
               });
            EventActions_Single.Add("einstein",
               () =>
               {
                   this.AddMoney(this.Player, 20000);
                   Refresh();
               });
            EventActions_Single.Add("lead",
               () =>
               {
                   this.DismissRandomSubject();
               });
            EventActions_Single.Add("megajanlott", () =>
            {
                InitializeSubjectTransactions(true, true);
            });
            NeptunActions = new Action[Constants.neptunMessages.Length];
           
            
            NeptunActions[0] = () =>
            {
                this.Player.State = PlayerState.missARound;
            };
            NeptunActions[1] = () =>
            {
                this.ArriveAtPosition(0);
                this.Refresh();
                EventCard?.Invoke(this, new CardEventArgs(GameBoard[Player.CurrentCard].ImageKey, Player));
            };
            NeptunActions[2] = () =>
            {
                Win();
            };
            NeptunActions[3] = () =>
            {
                this.RemoveMoney(Player, 7000);
                this.Refresh();
            };





            //loading the eventmapper with the names of the events to be invoked
            EventMapper = new Dictionary<int, string>();
            for (int i = 0; i < Constants.hallgatoevents_nevek.Length; i++)
            {
                EventMapper.Add(i, Constants.hallgatoevents_nevek[i]);
            }

           
            //millis = new List<long>();
            
            SetMetrics();
            //if we do not yet have elements in the gameboard, it is a new game, so generate it
            //if there are already elements, then it is a load and loading has already taken care of filling GB with data
            //P.Currentposition = GameBoard[0].Rect;
            Players = new CircularList<Player>(); //the max number of players is 3
            //Players.Add(new Player("nik", GameBoard[0].Rect, 0, "Nikes kocka"));
            ////GameBoard[0].ArriveAtPosition(Players[0]);
            //
            //Players.Add(new Player("kando", CalculateSecondaryPosition(GameBoard[0].Rect), 1, "Részeg Kandós"));
            ////GameBoard[0].ArriveAtPosition(Players[1]);
            //
            //Players.Add(new Player("rejto", CalculateTertialPosition(GameBoard[0].Rect), 2, "Cuki rejtős lány <3 <3"));
            //GameBoard[0].ArriveAtPosition(Players[2]);

            //AddSubjectToPlayer(Players[0], Subjects["nik"][0], false); ADDING SUBJECT FOR TEST
            //adding the first player
            Players.Add(new Player("lel", 0, "Player 1"));
            PlayerTokens = new ObservableCollection<string>();
            for (int i = 0; i < Constants.playerTokens.Length; i++)
            {
                PlayerTokens.Add(Constants.playerTokens[i]);
            }
            SelectedItem = PlayerTokens[0];

           

            PropertyChanged += (object sender, PropertyChangedEventArgs prop) =>
            {
                
                if(prop.PropertyName == "Turn")
                {
                    if (Turn % Players.Count == 0)
                    {
                        RoundCounter++;
                    }
                }
            };
        }

        public void DeleteSave(string selectedPath)
        {
            if (selectedPath != null)
            {
                FileInfo info = new FileInfo(selectedPath);
                info.Delete();
                Saves.Remove(selectedPath); 
            }
            else
            {
                GeneralNotification?.Invoke(this, new TransferEventArgs("Nem jelöltél ki semmit!"));
            }
        }

        public bool IsFreeCourseValid(int? indexNumberForCols)
        {
            //if the player has all the subjects, getting a new one is not relevant
            if (Player.Subjects.Count == 3)
            {
                NotifyPlayer?.Invoke(null, new TransferEventArgs("Az összes tárgyad megvan, így erre nincs szükséged!"));
                return false;
            }
            /*in the events array, we used the same order as in the collection Players,  
             so we only have to see if the current player has the same index as the given number*/
            return indexNumberForCols == Players.IndexOf(Player); //todo check if this works after changing to OC
        }

        CircularDictionary<BoardField> gameBoard; //dict to hold the cards of the board
        Dictionary<string, object> eventCardTexts; //dict to store the text elements for the cards
        Dictionary<string, Action> eventActions_single; //dict to store the actions that has the same signature
        Dictionary<int, string> eventMapper; //maps the string in the event resource dictionary to their corresponding order in constants
        Action[] neptunActions; //storing actions for the neptun
        //holds the subjects for the players, we use a more flexible data type that is more flexible than arrays here so we can add and remove them runtime
        //also, the original lists do not need to be tempered with
        Dictionary<string, ObservableCollection<Subject>> subjects;
        Random rand;
        CircularList<Player> players;
        public event EventHandler Invalidate; //provides a way to invalidate the visual from hte view
        public event EventHandler<CardEventArgs> EventCard; //provides a way to invoke the event in the view
        public event EventHandler<SubjectEventArgs> InitiateSubjectTransaction; //provides a way to invoke the subjects window
        public event EventHandler<TransferEventArgs> NotifyPlayer;
        public event EventHandler<EventArgs> CloseOpenWindows;
        public event EventHandler<EventArgs> FinishedGame;
        public event EventHandler<TransferEventArgs> GeneralNotification;
        #region CardMetrics
        public class NormalCard
        {
            public static int height = 135; //180
            public static int width = 90; //120
        }

        public class SquareCard
        {
            public static int widthHeight = 135; //180
        }


        #endregion
        int turn;  //marks the turns in the game

        int roundCounter;
        
        public int RoundCounter
        {
            get
            {
               
                return roundCounter;
            }

            set
            {
                roundCounter = value;
            }
        }
        Player p; //reference to the actual player

        #region SubjectWindowItems
        ObservableCollection<Subject> subjectsOfPlayer; //represents the subjects of the player


        ObservableCollection<Subject> subjectsAvailableToPlayer; //represents the subjects available to the player
        Subject selectedSubject; //represetns the subject the player selected in the subject window
        bool canPlayerBuyIt;

    


        public ObservableCollection<Subject> SubjectsOfPlayer
        {
            get
            {
                return subjectsOfPlayer;
            }

            set
            {
                subjectsOfPlayer = value;
            }
        }

        public ObservableCollection<Subject> SubjectsAvailableToPlayer
        {
            get
            {
                return subjectsAvailableToPlayer;
            }

            set
            {
                subjectsAvailableToPlayer = value;
            }
        }

        public Subject SelectedSubject
        {
            get
            {
                return selectedSubject;
            }

            set
            {
                selectedSubject = value;
            }
        }
        public bool CanPlayerBuyIt
        {
            get
            {
                return canPlayerBuyIt;
            }

            set
            {
                canPlayerBuyIt = value;
                OnPropertyChanged();
            }
        }

        public void InitializeSubjectTransactions(bool isSubjectForFree, bool weNeedEvent)
        {
            /*If we need to initiate a new subject transaction from a view element,
             we must deal with actually showing the windows right where we are going to use them and
             pass false to weneedevent.
             If we initiate it from the view model, we need the even to tunnel the showing of the windows to a view element and
             we pass true to weneedevent.*/
            //we initialize the data needed to show the subjects
            SubjectsAvailableToPlayer = Subjects[Player.PuppetKey];
            SubjectsOfPlayer = Player.Subjects;
            SelectedSubject = null;
            CanPlayerBuyIt = true;
            if (weNeedEvent)
            {
                InitiateSubjectTransaction?.Invoke(this, new SubjectEventArgs(Player, SubjectsAvailableToPlayer, isSubjectForFree));

            }
        }


        public void CanPlayerBuySubject(object v)
        {
            Subject s = (v as Subject);
            if(Player.Money - s.Price > 0)
            {
                CanPlayerBuyIt = true;
            }
            else
            {
                CanPlayerBuyIt = false;
            }

        }



        #endregion

        int leftVerticalAlign;
        int incrementAtMovement;
        int upperVerticalAlign;
        int rightVerticalAlign;
        int startPosition;
        int lowerHorizontalAlign;
        int numberOfElementsInAHorizontalRow;
        int numberOfElementsInAVerticalRow;

        float puppetDiameter; //the diameter of the player puppet
        float puppetDiameterChangeConstant; //the constant to which the diameter of the puppet changes


        #region WinWindow

        public string Winner { get; set; }

        public void Win()
        {
            Winner = $"GRATULÁLUNK, {Player.Name}\nMEGNYERTED A JÁTÉKOT!";
            FinishedGame?.Invoke(null, null);
        }
        

        #endregion

        public string[] resourceNamesNormal;
        public string[] resourceNamesSquare;

        

        string[] resourceNamesNormalLeft;

       

        string[] resourceNamesNormalRight;

        int movementSpeed;


        #region EventViewer
        string[] greens;
        string[] blacks;
        string[] yellows;
        string[] blues;

        int? indexOfEventCardCollection;
        public int? IndexOfEventCardCollection
        {
            get
            {
                return indexOfEventCardCollection;
            }

            set
            {
                indexOfEventCardCollection = value;
            }
        }

        public int GetPositionFromGostring(string textToBeDisplayed)
        {
            //we get the number from the text and return with the number of the card that number far from the player's current card
            //we know that the last character is the numberic, oc, we could test all the characters and stop where we find a number
            return (int)Char.GetNumericValue(textToBeDisplayed.Last());
        }

        public void ArriveAtRandomPosition()
        {
            ArriveAtPosition(Rand.Next(0, GameBoard.Count));
        }

        #endregion

        public bool RollButtonEnabled { get; set; }

        //List<long> millis; 

        public Player Player
        {
            get
            {
                return p;
            }

            set
            {
                p = value;
            }
        }

        public int LeftVerticalAlign
        {
            get
            {
                return leftVerticalAlign;
            }

            set
            {
                leftVerticalAlign = value;
            }
        }

        public int IncrementAtMovement
        {
            get
            {
                return incrementAtMovement;
            }

            set
            {
                incrementAtMovement = value;
            }
        }

        public int UpperVerticalAlign
        {
            get
            {
                return upperVerticalAlign;
            }

            set
            {
                upperVerticalAlign = value;
            }
        }

        public int RightVerticalAlign
        {
            get
            {
                return rightVerticalAlign;
            }

            set
            {
                rightVerticalAlign = value;
            }
        }

        public int StartPosition
        {
            get
            {
                return startPosition;
            }

            set
            {
                startPosition = value;
            }
        }

        public int LowerHorizontalAlign
        {
            get
            {
                return lowerHorizontalAlign;
            }

            set
            {
                lowerHorizontalAlign = value;
            }
        }

        public float PuppetDiameter
        {
            get
            {
                return puppetDiameter;
            }

            set
            {
                puppetDiameter = value;
            }
        }

        public float PuppetDiameterChangeConstant
        {
            get
            {
                return puppetDiameterChangeConstant;
            }

            set
            {
                puppetDiameterChangeConstant = value;
            }
        }

        public int MovementSpeed
        {
            get
            {
                return movementSpeed;
            }

            set
            {
                movementSpeed = value;
            }
        }
        [XmlIgnore]
        public CircularDictionary<BoardField> GameBoard
        {
            get
            {
                return gameBoard;
            }

            set
            {
                gameBoard = value;
            }
        }
       
        public CircularList<Player> Players
        {
            get
            {
                return players;
            }

            private set
            {
                players= value;
            }
        }
        //the values responsible for moving the phone 
        public int OffsetHorizontal { get;  set; }
        public int OffsetVertical { get;  set; }


        [XmlIgnore]
        public Dictionary<string, object> EventCardTexts
        {
            get
            {
                return eventCardTexts;
            }

            set
            {
                eventCardTexts = value;
            }
        }
        [XmlIgnore]
        public Dictionary<string, Action> EventActions_Single
        {
            get
            {
                return eventActions_single;
            }

            set
            {
                eventActions_single = value;
            }
        }

        [XmlIgnore]
        public Dictionary<string, ObservableCollection<Subject>> Subjects
        {
            get
            {
                return subjects;
            }

            set
            {
                subjects = value;
            }
        }

        public int NumberOfElementsInAHorizontalRow
        {
            get
            {
                return numberOfElementsInAHorizontalRow;
            }

            set
            {
                numberOfElementsInAHorizontalRow = value;
            }
        }

        public int NumberOfElementsInAVerticalRow
        {
            get
            {
                return numberOfElementsInAVerticalRow;
            }

            set
            {
                numberOfElementsInAVerticalRow = value;
            }
        }
        [XmlIgnore]
        public Dictionary<int, string> EventMapper
        {
            get
            {
                return eventMapper;
            }

            set
            {
                eventMapper = value;
            }
        }
        [XmlIgnore]
        public Action[] NeptunActions
        {
            get
            {
                return neptunActions;
            }

            set
            {
                neptunActions = value;
            }
        }

        public Random Rand
        {
            get
            {
                return rand;
            }

            set
            {
                rand = value;
            }
        }






        //public Player P { get => p; private set => p = value; }
        //public int LeftVerticalAlign { get => LeftVerticalAlign1; set => LeftVerticalAlign1 = value; }
        //public int IncrementAtMovement { get => IncrementAtMovement1; set => IncrementAtMovement1 = value; }
        //internal Dictionary<int, BoardField> GameBoard { get => gameBoard; set => gameBoard = value; }
        //public int LeftVerticalAlign1 { get => leftVerticalAlign; set => leftVerticalAlign = value; }
        //public int IncrementAtMovement1 { get => incrementAtMovement; set => incrementAtMovement = value; }
        //public int UpperVerticalAlign { get => upperVerticalAlign; set => upperVerticalAlign = value; }
        //public int RightVerticalAlign { get => rightVerticalAlign; set => rightVerticalAlign = value; }
        //public int StartPosition { get => startPosition; set => startPosition = value; }
        //public int LowerHorizontalAlign { get => lowerHorizontalAlign; set => lowerHorizontalAlign = value; }
        //public float PuppetDiameter { get => puppetDiameter; set => puppetDiameter = value; }
        //public float PuppetDiameterChangeConstant { get => puppetDiameterChangeConstant; set => puppetDiameterChangeConstant = value; }
        //public string[] ResourceNamesSquare { get => resourceNamesSquare; set => resourceNamesSquare = value; }
        //public string[] ResourceNamesNormal { get => resourceNamesNormal; set => resourceNamesNormal = value; }
        //public string[] ResourceNamesNormalLeft { get => resourceNamesNormalLeft; set => resourceNamesNormalLeft = value; }
        //public string[] ResourceNamesNormalRight { get => resourceNamesNormalRight; set => resourceNamesNormalRight = value; }
        //public int MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

        public void SetMetrics()
        {
            //the rect marking the outer edge of the left vertical row
            LeftVerticalAlign = StartPosition - ((Constants.numberOfElementsInAHorizontalRow_start) * NormalCard.width
                + (SquareCard.widthHeight - NormalCard.width));

            //the rect marking the upper edge of the board
            UpperVerticalAlign = LowerHorizontalAlign -
                (NormalCard.width * Constants.numberOfElementsInAVerticalRow_start)
                - (SquareCard.widthHeight - NormalCard.width);
            RightVerticalAlign = LeftVerticalAlign + (Constants.numberOfElementsInAHorizontalRow_start - 1) * NormalCard.width + SquareCard.widthHeight;

        }

        #region Save/Load
        /// <summary>
        /// Seeks for a pre-specified Directory in Documents, creates it, if it does not exist, then saves critical data
        /// </summary>
        public void Save()
        {
            StringBuilder saveFolderPath = new StringBuilder(SaveFolderPath);

            GameBoard_TempForSerilaize = new Dictionary<int, BoardField>();

            foreach (KeyValuePair<int, BoardField> board in GameBoard)
            {
                GameBoard_TempForSerilaize.Add(board.Key, board.Value);
            }
            //dictionaries cannot be serialized in this version of .Net
            //instatiating dictioanry helper lists
            //Keys = new List<string>();
            //Values = new List<ObservableCollection<Subject>>();
            //
            //foreach (KeyValuePair<string, ObservableCollection<Subject>> pair in Subjects)
            //{
            //    Keys.Add(pair.Key);
            //    Values.Add(pair.Value);
            //}
            //
            //Ser = new List<object>();
            //Ser.Add(Players);
            //Ser.Add(Keys);
            //Ser.Add(Values);
            //DateTime time = DateTime.Now;
            //saveFolderFullPath.Append($"\\{DateTime.Now.ToString("yyyy.MM.dd HH_mm_ss")}");
            //Directory.CreateDirectory(saveFolderFullPath.ToString());
            //
            //Serializer<CircularList<Player>>.Serialize(Players, saveFolderFullPath.ToString() + $"\\players.xml");
            //Serializer<List<string>>.Serialize(Keys, saveFolderFullPath.ToString() + $"\\subjects_keys.xml");
            //Serializer<List<ObservableCollection<Subject>>>.Serialize(Values, saveFolderFullPath.ToString() + $"\\subjects_values.xml");
            //we store the data in one place, so that the shiny serializer can serialize it into 1 file
            Ser = new List<object>
            {
                Players,
                Subjects,
                Player,
                RoundCounter,
                GameBoard_TempForSerilaize
            };
            SharpSerializer serializer = new SharpSerializer();


            saveFolderPath.Append($"\\{DateTime.Now.ToString("yyyy.MM.dd HH_mm_ss")}.xml");
            serializer.Serialize(Ser, saveFolderPath.ToString());
            //serializeDictionary.Add(Players);
            //    serializeMe, saveFolderFullPath.Append($"\\{DateTime.Now}.xml").ToString());


        }

        /// <summary>
        /// Fills a list with save files
        /// </summary>
        public void Peek()
        {
            string docPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Diplomazz Okosan";
           
            Saves = new ObservableCollection<string>();

            //adding all the files found in the saves directory to Saves
            DirectoryInfo info = new DirectoryInfo(docPath);
            foreach (var item in info.GetFiles())
            {
                Saves.Add(item.FullName);
            }

        }
        /// <summary>
        /// Gets the defualt save folder from the win environment var
        /// </summary>
        /// <returns></returns>
        string GetSaveDirectory()
        {
            //getting the locetion of the Documents
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string saveFolder = "Diplomazz Okosan";
            StringBuilder saveFolderFullPath = new StringBuilder(docPath);
            saveFolderFullPath.Append($"\\{saveFolder}");

            //creating directory if it does not exist
            if (!Directory.Exists(saveFolderFullPath.ToString()))
            {
                Directory.CreateDirectory(saveFolderFullPath.ToString());
            }

            return saveFolderFullPath.ToString();
        }
        /*for some reason, the polenter serializer cannot save my circular dictionary implementation
         so this temp dict is used to save the data, strangely enough, my circular list means no problem*/
        public Dictionary<int, BoardField> GameBoard_TempForSerilaize { get; set; } 

        public bool Load(string fullFilePath)
        {
            
            //SharpSerializer serializer = new SharpSerializer();
            //object temp = serializer.Deserialize(fullFilePath);
            if (fullFilePath != null)
            {
                try
                {
                    //deserializing persistent data
                    SharpSerializer serializer = new SharpSerializer();
                    Ser = (List<object>)serializer.Deserialize(fullFilePath);
                    Players = (CircularList<Player>)Ser[0];
                    Subjects = (Dictionary<string, ObservableCollection<Subject>>)Ser[1];
                    Player = (Player)Ser[2];
                    RoundCounter = (int)Ser[3];
                    GameBoard_TempForSerilaize = new Dictionary<int, BoardField>();
                    GameBoard_TempForSerilaize = (Dictionary<int, BoardField>)Ser[4];
                    //getting data out of the normal dictionary
                    foreach (KeyValuePair<int, BoardField> item in GameBoard_TempForSerilaize)
                    {
                        GameBoard.Add(item.Key, item.Value);
                    }
                    GameBoard_TempForSerilaize = null;
                    return true;
                }
                catch (Exception e)
                {
                    GeneralNotification?.Invoke(this, new TransferEventArgs(e.Message));
                    return false;
                }
                finally
                {
                    //we set the selected path back, so we can re-use it
                    SelectedPath = null;
                }

            }
            else
            {
                GeneralNotification?.Invoke(this, new TransferEventArgs("Csak úgy tudsz betölteni, ha ki is jelölsz valamit!"));
                return false;
            }
            

        }

        //Subject[] Except(Subject[] ar1, Subject[] ar2)
        //{
        //    Subject[] except = new Subject[ar1.Length > ar2.Length ? ar1.Length : ar2.Length];
        //    int idx = 0;
        //    for (int i = 0; i < ar2.Length; i++)
        //    {
        //        if (!ar1.Contains(ar2[i]))
        //        {
        //            except[idx++] = ar2[i];
        //        }
        //    }
        //    int temp = 0;
        //    
        //    while(temp < idx && except[temp] != null)
        //    {
        //        temp++;
        //    }
        //    string[] ret = new string[temp];
        //    for (int i = 0; i < ret.Length; i++)
        //    {
        //        ret[i] = except[i];
        //    }
        //    return ret;
        //}

        public List<object> Ser { get; set; }
        public string SelectedPath { get; set; }
        public ObservableCollection<string> Saves { get; set; }
        public string SaveFolderPath { get; set; }

        public int RandomGeneratedNumber { get; set; }
        
        string FormatForSave(string unformatted)
        {
            //split the text
            string[] temp = unformatted.Split('\\');

            //now we have only the name
            StringBuilder builder = new StringBuilder(temp[temp.Length - 1]);

            //we clip the ".xml" part
            builder.Remove(builder.Length - 4, 4);

            //we replace the '_' s with ':'s to make it more user-friendly
            builder.Replace('_', ':');
            return builder.ToString();

        }
        #endregion
        public void Zoom(int offset)
        {
            //if we multiply both the height and the width by the same number
            //the normal ratio is distorted (since the values are approaching each other)
            //5/3 --> the original height x width is 135x90 which means that height : width = 2 / 3
            //height has to change more than width, so it has to increase by the offsset + the difference in their ratio
            //this is taking 1 2/3 of the offset which is 5/3 * offset

            NormalCard.height += (offset * 5 / 3);
            NormalCard.width += offset;
            SquareCard.widthHeight += (offset * 5 / 3);
            //LowerHorizontalAlign -= offset; 
            //StartPosition -= offset; 

            float temp = ((float)(NormalCard.width + offset) / (float)NormalCard.width);
            PuppetDiameter *= temp;

            //refreshing the position of the puppet so that it moves with the board (the changing board would move away from it)
            Player.Currentposition = GameBoard[Player.CurrentCard].Rect;
            SetMetrics();
        }

        public void MoveHorizontally(int offset)
        {
            StartPosition += offset;
            //megcsinálni az összes bábura
            foreach (Player player in Players)
            {
                player.Currentposition = Point.Add(player.Currentposition, new Vector(offset, 0));
            }
            OffsetHorizontal += offset;
            SetMetrics();
        }

        public void MoveVertically(int offset)
        {
            LowerHorizontalAlign += offset;
            foreach (Player player in Players)
            {
                player.Currentposition = Point.Add(player.Currentposition, new Vector(0, offset));
            }
            OffsetVertical += offset;
            SetMetrics();
        }
        /// <summary>
        /// Goes to the specified position from the current one. 
        /// </summary>
        /// <param name="position">The key of the destination card</param>
        public void GenerateRandomNumber()
        {
            RandomGeneratedNumber = Rand.Next(1, 7);
        }
        
        public void GoToPosition(int position)
        {
            /*We have to keep stepping from the current position, but the number of steps has to start from 0*/
            int positionHolder = Player.CurrentCard; //we start off from the current position of the player
            int startingPosition = Player.CurrentCard;
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);
            int incr = 0;
            //if the goal position is in the same round (eg. from 9 to 12) we only have to take just as many steps as is there difference
            //if the goal position is in another round (eg. from 25 to 1)
            //we have to take as many steps as it takes to get to the start + the number of steps taking us to the goal card
            //int boundary = position > Player.CurrentCard ? position - Player.CurrentCard : (GameBoard.Count - Player.CurrentCard) + position;
            int boundary = position;
            timer.Interval = TimeSpan.FromMilliseconds(422); //422 //kókány: 328.10526
            timer.Start();
            timer.Tick += (o, args) => {
           
                //we get the number of cards between the player and the destination card
                
                //we check the next free position available on the destination
                if (incr <= boundary)
                {
                    //we do not calculate the next free position for the card the player stands on
                    Step(positionHolder,
                         positionHolder != startingPosition ?  gameBoard[positionHolder].GetNextFreePosition() : 0);
                    positionHolder++;
                    incr++;
                }
                else
                {
                    timer.Stop();
                    //the --positionHolder means that we have to consider that at the end of the last iteration of the Tick...
                    //...the positionHOlder will point to the card AFTER the one the player stands on
                    Player.CurrentCard = GetPlayerCardNumber(Player, gameBoard[--positionHolder].GetNextFreePosition());
                    //MessageBox.Show($"Now I stand on card{Player.CurrentCard}");
                    //MessageBox.Show($"Average run stat: {millis.Average().ToString()}");
                    GameBoard[Player.CurrentCard].ArriveAtPosition(Player);

                    //we signal that an event has to happen here that a view can handle anyway s/he wants (MVVM <3)
                    EventCard?.Invoke(this, new CardEventArgs(GameBoard[Player.CurrentCard].ImageKey, Player));
                    //NextRound();
                    
                    
                }
            };
            //Step(31);
            


        }

        int GetPlayerCardNumber(Player p, int positionOrder)
        {
            return GameBoard.Where(g => 
            IsThePlayerInTheVicinity(GetOrderRect(g.Value.Rect, positionOrder), p.Currentposition) == true)
            .FirstOrDefault().Key;
        }

        /// <summary>
        /// Based on the order, calculates which position the player stands on
        /// </summary>
        /// <param name="originalRect">The positon 0 (center) of the cards</param>
        /// <param name="order">The order of the player on the card</param>
        /// <returns></returns>
        Point GetOrderRect(Point originalRect, int order)
        {
            switch (order)
            {
                case 0:
                    return originalRect;
                case 1:
                    return CalculateSecondaryPosition(originalRect);
                case 2:
                    return CalculateTertialPosition(originalRect);
                default:
                    return originalRect;
                    
            }
        }

        bool IsThePlayerInTheVicinity(Point cardRect, Point playerCurrentPosition)
        {
            //the player stands on the card if it is witin a diameter of 10 units
            int vicinity = 10;
            double temp = Math.Abs(Point.Subtract(cardRect, playerCurrentPosition).Length);
            return  temp<= vicinity;
        }

        /// <summary>
        /// Calculates the position of the next step
        /// </summary>
        /// <param name="positionNumber">The number of the card the player is about to arrive on</param>
        /// <param name="orderInCard">The order of the player within the card indexed from 0</param>
        /// <returns></returns>
        Point GetPositionOfNextStep(int positionNumber, int orderInCard)
        {
            switch (orderInCard)
            {
                //if the player is the 1st to stand on the card
                case 0:
                    return GameBoard[positionNumber].Rect;
                //if the player is 2nd on the card
                case 1:
                    return CalculateSecondaryPosition(GameBoard[positionNumber].Rect);
                //otherwise, which is if the player is 3rd on the card
                default:
                   return CalculateTertialPosition(GameBoard[positionNumber].Rect);
            }

        }
        public void Step(int positionNumber, int orderOnTheCard)
        {
            //we tell the card on which the player priviously stood that s/he is leaving
            if (Player.CurrentCard == positionNumber)
            {
                GameBoard[Player.CurrentCard].DepartFromposition(Player); 
            }
            //else
            //{
            //    Player.CurrentCard = positionNumber;
            //   
            //}

            Point goalPosition;


            goalPosition = GetPositionOfNextStep(positionNumber, orderOnTheCard);

            //RegisterName("player", p.Currentposition);

            //Stopwatch watch = new Stopwatch();
            //update the current position
            
            Vector v = new Vector(goalPosition.X - Player.Currentposition.X, goalPosition.Y - Player.Currentposition.Y);
            //PointAnimation move = new PointAnimation(goalPosition, TimeSpan.FromSeconds(2));
            //re-render the scene
            //move.From = p.Currentposition;
            // for (int i = 0; i < 10000; i++)
            // {

            DispatcherTimer t = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };

            t.Start();

            int c = 0;
            int divide = 20;

            //watch.Start();
            t.Tick += (o, args) =>
            {


                //if (c > divide)
                //{
                //    t.Stop();
                //    //watch.Stop();
                //    //millis.Add(watch.ElapsedMilliseconds);
                //}

                if (c >= divide)
                {
                    t.Stop();
                    //watch.Stop();
                    //millis.Add(watch.ElapsedMilliseconds);
                }
                else
                {
                    Player.Currentposition = Point.Add(Player.Currentposition, v / divide);
                    Refresh(); //we just need a way to convey the invalidate visual from here
                }
                c++;

            };
            //t.Tick += (o, args) =>
            //  {
            //      decimal d = Math.Ceiling((decimal)Point.Subtract(goalPosition, p.Currentposition).Length);
            //      Debug.WriteLine(d);
            //      if (  d >5 )
            //      {
            //          p.Currentposition = Point.Add(p.Currentposition, v / duration);
            //          InvalidateVisual();
            //      }
            //      else
            //      {
            //          t.Stop();
            //      }
            //  };

            //t.Tick += new EventHandler((object obj, EventArgs args) => {
            //    c++;
            //    if (c > 15)
            //        t.Stop();
            //    p.Currentposition = Point.Add(p.Currentposition, v / 15);
            //    InvalidateVisual();
            //
            //});

            // }
            //Storyboard sb = new Storyboard();
            //sb.Children.Add(move);
            //Storyboard.SetTargetName(move, "player");
            //Storyboard.SetTargetProperty(move, new PropertyPath(EllipseGeometry.CenterProperty));
            //sb.Begin();
            //watch.Stop();
            //Debug.WriteLine(watch.ElapsedTicks);
            
        }

        public void NextRound()
        {
            //we check if there are players in the game
            if (IsTheGameStillOn())
            {
                //in the case of a re-roll, we have to inspect the current player, in all other cases, the next player matters
                if (Player.State == PlayerState.rollagain)
                {
                    Player.State = PlayerState.neutral;
                    //Turn++; //increment the number of turns
                }

                else if (Players.WhatIsNext().State == PlayerState.neutral)
                {
                    Player = Players.GetNextElement();
                    Turn++;
                }

                //if the next player has to miss the turn, we set back its state bc s/he is going to miss this round
                //then ask for the player coming after 
                else if (Players.WhatIsNext().State == PlayerState.missARound)
                {
                    Players.WhatIsNext().State = PlayerState.neutral; //set the state back
                    Player = Players.GetNextElement(2); //ask for the player after the one who misses a round
                    Turn += 2; //now one of the players missed a round, so it takes only 2 players to complete a round
                }
                else if (Players.WhatIsNext().State == PlayerState.lost)
                {
                    Player = Players.GetNextElement(2); //ask for the player after the one who misses a round
                    Turn += 2;
                }
                RollButtonEnabled = true;
                RandomGeneratedNumber = 0; //at the start of every new round, we set this back to 0

            }
            else
            {
                GeneralNotification?.Invoke(this, new TransferEventArgs("Sajnos egyikőtök sincs már játékban, így a játék véget ért"));
            }
        }

        public Point CalculatePrimaryPosition(Point cornerRect, int widthOfCurrentCard, int heightOfCUrrentCard)
        {
            //center
            //valid for square as well, since their width/height is the same as the normal height
            return Point.Add(cornerRect, new Vector(widthOfCurrentCard / 2, heightOfCUrrentCard / 2));
        }

        public Point CalculateSecondaryPosition(Point centerOfCard)
        {
            //up
            return Point.Subtract(centerOfCard, new Vector(0, 60));
        }

        public Point CalculateTertialPosition(Point centerOfCard)
        {
            //down
            return Point.Add(centerOfCard, new Vector(0, 60));
        }

        public void GenerateOrderOfCards()
        {
            //2* (const.vert-1) + 2*(const.hor -1) + 4
            int indexer = 0;
            Rand = new Random();
            int totalNumberOfCards = 2 * (Constants.numberOfElementsInAVerticalRow_start - 1) + 2 * (Constants.numberOfElementsInAHorizontalRow_start - 1) + 4;
            string[] temp = new string[totalNumberOfCards];

            GameBoard.Add(indexer++, new BoardField()
            {
                Rect = CalculatePrimaryPosition(new Point(StartPosition, LowerHorizontalAlign), SquareCard.widthHeight, SquareCard.widthHeight),
                ImageKey = "start"
            });

            BoardField b; //reference to a board item so that we can store elements in it
            //generating cards for lower horizontal row
            for (int i = 1; i <= Constants.numberOfElementsInAHorizontalRow_start; i++)
            {
                b = new BoardField();
                if (i % Constants.numberOfElementsInAHorizontalRow_start == 0)
                {
                    //temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = resourceNamesSquare[Rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[Rand.Next(0, resourceNamesNormal.Length)];
                }
                //add boardcard to the collection of boardscards
                GameBoard.Add(indexer++, b);
            }

            //generating cards for left vertical row
            for (int i = 1; i <= Constants.numberOfElementsInAVerticalRow_start; i++)
            {
                b = new BoardField();
                if (i % Constants.numberOfElementsInAVerticalRow_start == 0)
                {
                    //temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = resourceNamesSquare[Rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[Rand.Next(0, resourceNamesNormal.Length)];
                }
                GameBoard.Add(indexer++, b);
            }


            //while (i < temp.Length && i <= Constants.numberOfElementsInAVerticalRow)
            //{
            //    if (i % Constants.numberOfElementsInAVerticalRow == 0)
            //    {
            //        temp[i-1] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
            //    }
            //    else
            //    {
            //        temp[i - 1] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
            //    }
            //
            //    
            //    i++;
            //}

            //generating cards for lower horizontal row
            for (int i = 1; i <= Constants.numberOfElementsInAHorizontalRow_start; i++)
            {
                b = new BoardField();
                if (i % Constants.numberOfElementsInAHorizontalRow_start == 0)
                {
                    //temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = resourceNamesSquare[Rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[Rand.Next(0, resourceNamesNormal.Length)];
                }
                GameBoard.Add(indexer++, b);
            }
            //generating cards for upper horizontal row
            // while (i < temp.Length && i <= Constants.numberOfElementsInAHorizontalRow)
            // {
            //     if (i % Constants.numberOfElementsInAHorizontalRow == 0)
            //     {
            //         temp[i-1] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
            //     }
            //     else
            //     {
            //         temp[i - 1] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
            //     }
            //
            //    
            //     i++;
            // }


            //generating cards for right vertical row
            
            //we have the -1, bc we already have the start card, so we do not need to generate it here
            for (int i = 1; i <= Constants.numberOfElementsInAVerticalRow_start-1; i++)
            {
                b = new BoardField();
                if (i % Constants.numberOfElementsInAVerticalRow_start == 0)
                {
                    //temp[indexer++] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                    b.ImageKey = resourceNamesSquare[Rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[Rand.Next(0, resourceNamesNormal.Length)];
                }
                GameBoard.Add(indexer++, b);
            }
            //to make sure that the game can be won, 1 of the cards are switched to a win card randomly
            //we start form 1 bc the start cannot be switched
             GameBoard[Rand.Next(1, GameBoard.Count)].ImageKey = "enroll";
                
            
            GameBoard[2].ImageKey = "go"; //USED FOR TESTING!!!

            //while (i < temp.Length && i <= Constants.numberOfElementsInAVerticalRow)
            //{
            //    if (i % Constants.numberOfElementsInAVerticalRow == 0)
            //    {
            //        temp[i-1] = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
            //    }
            //    else
            //    {
            //        temp[i - 1] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
            //    }
            //
            //    
            //    i++;
            //}

        }
        /// <summary>
        /// Decides on which category the card with the given key belongs to
        /// </summary>
        /// <param name="cardKey">The key of the card</param>
        /// <returns></returns>
        public string EventCategroySelector(string cardKey)
        {
            if (greens.Contains(cardKey))
                return "green";
            else if (blues.Contains(cardKey))
                return "blue";
            else if (yellows.Contains(cardKey))
                return "yellow";
            else
                return "black";
        }

        /// <summary>
        /// Gets out the a pre-specified values from the dictionary that containts event texts and formats it to be displayed. I used Tuples instead of obj[] bc they are cool AF.
        /// </summary>
        /// <param name="cardKey">The key of the card that triggered the event</param>
        /// <returns></returns>
        public Tuple<string, int, int?> GetTextToDisplay(string cardKey)
        {

            
            StringBuilder builder = new StringBuilder();
            object temp = EventCardTexts[cardKey];
            int? indexHolder = null; //represents the index of the collections, but only if it is a collection
           
            int boundary = 0; //the number of characters allowed in 1 line  

            if(temp is string)
            {
                builder = new StringBuilder(temp as string);
            }
            else if(temp is string[])
            {
                string[] tempArray = temp as string[];
                 
                indexHolder = Rand.Next(0, tempArray.Length);
                builder = new StringBuilder(tempArray[(int)indexHolder]);
            }
            else if(temp is int[])
            {
                int[] tempArray = temp as int[];
                indexHolder = Rand.Next(0, tempArray.Length);
                builder = new StringBuilder($"Lépj előre: {tempArray[(int)indexHolder]}");
            }


            //based on the number of characters, we assign a fontsize as well
            int fontsize = 16;

            if (builder.Length < 50)
            {
                fontsize = 30;
            }
            else if (builder.Length >= 50 && builder.Length < 100)
            {
                fontsize = 20;
            }

            //based on the fontsize, we determine how many characters should be in a line (in a not so mathematical manner)
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

            //we append the string with \n in the proper places so they appear to be formatted
            int buffer = 0; //this is used to track the length of the line
            for (int i = 0; i < builder.Length; i++)
            {
                buffer++; 
                if(builder[i] == '\n')
                {
                    buffer = 0; //if we arrived at a new line, we reset the buffer
                }
                if (buffer % boundary == 0 && buffer != 0)
                {
                    //we look for the first space character and append the text there
                    //this will be found before the last character to be displayed, so we will count backwards
                    //this way the words will appear to be correct
                    int j = i;
                    while(j > 0 && builder[j] != ' ')
                    {
                        j--;
                    }
                    if (builder[j] != '\n')
                    {
                        builder.Insert(j, "\n");
                        builder.Remove(j + 1, 1); //we delete the space after the word, becuase we no longer need it due to the new line 
                    }
                }
            }
            //else it remains 12
            return Tuple.Create<string, int, int?>(builder.ToString(), fontsize, indexHolder);

        }

        public void AddSubjectToPlayer(Player p, Subject subject, bool free)
        {
            p.Subjects.Add(subject); //adding to the subject of the player
            Subjects[p.PuppetKey].Remove(subject); //remove the item from the list of availables
            if (!free)
            {
                RemoveMoney(p, subject.Price);
            }

            //we check if the player could win now
            if (CheckWinningConditions(p))
            {
                Win();
            }
        }

        void RemoveMoney(Player p, int sum)
        {
            if ((p.Money - sum) > 0)
            {
                p.Money -= sum;
            }
            else
            {
                p.State = PlayerState.lost;
                NotifyPlayer?.Invoke(this, new TransferEventArgs("Sajnos a Te számodra véget ért a játék :("));

            }

        }

        public void AddMoney(Player p, int sum)
        {
            p.Money += sum;
        }

        /// <summary>
        /// Refreshes the UI whatever the implementation might be
        /// </summary>
        public void Refresh()
        {
            Invalidate?.Invoke(null, null);
        }

        /// <summary>
        /// The player will appear on the designated position
        /// </summary>
        /// <param name="pos"></param>
        public void ArriveAtPosition(int pos)
        {
            GameBoard[Player.CurrentCard].DepartFromposition(Player); //leave the current card
            Player.Currentposition = GetPositionOfNextStep(pos, GameBoard[pos].GetNextFreePosition()); //set current position
            Player.CurrentCard = pos; //set current card
            GameBoard[Player.CurrentCard].ArriveAtPosition(Player); //arrive at position

        }

        #region MenuMenu

        [XmlIgnore]
        public ObservableCollection<string> PlayerTokens { get; set; }
        public Player NewGameSelectedPlayer { get; set; }
        string selectedItem;
        public string SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                
            }
        }

        public int Turn
        {
            get
            {
                return turn;
            }

            set
            {
                turn = value;
                OnPropertyChanged();
            }
        }

        public void AddPlayer()
        {
            if (Players.Count < 3)
            {
                Players.Add(new Player("nik", Players.Count + 1, $"Player {Players.Count + 1}")); 
            }
        }

        public void DeletePlayer(Player p)
        {
            if (Players.Count > 1)
            {
                Players.Remove(p); 
            }
        }

        /// <summary>
        /// This must run before any gameplay either new or loaded
        /// </summary>
        public void InitializeGame()
        {
            if (GameBoard.Count == 0)
            {
                GenerateOrderOfCards();
            }

            //if it is a new game, we will start it from 1 instead of 0
            //if it is a load, the loading has already loaded data, so it will not be 0
            if (RoundCounter == 0)
            {
                RoundCounter++;
            }

        }

        /// <summary>
        /// This must run before any NEW game, but not before loaded ones
        /// </summary>
        public void InitializeStartOfNewGame()
        {


            foreach (Player p in Players)
            {

                //we only load the subjects we need

                
                if (!Subjects.ContainsKey(p.PuppetKey))
                {
                    Subjects.Add(p.PuppetKey, new ObservableCollection<Subject>()); 
                }
                if (Subjects[p.PuppetKey].Count < 3) //only works because we habe 3 subjects
                {
                    switch (p.PuppetKey)
                    {
                        case "nik":
                            for (int i = 0; i < Constants.nik_targyak.Length; i++)
                            {
                                //adding the subjects with  prices
                                Subjects["nik"].Add(new Subject(Constants.nik_targyak[i], (i + 1) * 1000));
                            }
                            break;

                        case "rejto":
                            for (int i = 0; i < Constants.rejto_targyak.Length; i++)
                            {
                                //adding the subjects with  prices
                                Subjects["rejto"].Add(new Subject(Constants.rejto_targyak[i], (i + 1) * 1000));
                            }
                            break;
                        case "kando":
                            for (int i = 0; i < Constants.kando_targyak.Length; i++)
                            {
                                //adding the subjects with  prices
                                Subjects["kando"].Add(new Subject(Constants.kando_targyak[i], (i + 1) * 1000));
                            }
                            break;


                    }

                }
            }

            //adding the players to the first card
            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].CurrentCard = 0;
                BoardField start = GameBoard[Players[i].CurrentCard];
                Players[i].Currentposition = GetPositionOfNextStep(0, start.GetNextFreePosition());
                GameBoard[Players[i].CurrentCard].ArriveAtPosition(Players[i]);
               

            }
            //set the first player
            Player = Players[0];


            //Subjects.Add("nik", new ObservableCollection<Subject>());
            //for (int i = 0; i < Constants.nik_targyak.Length; i++)
            //{
            //    //adding the subjects with  prices
            //    Subjects["nik"].Add(new Subject(Constants.nik_targyak[i], (i + 1) * 1000));
            //}
            //
            //Subjects.Add("rejto", new ObservableCollection<Subject>());
            //for (int i = 0; i < Constants.rejto_targyak.Length; i++)
            //{
            //    //adding the subjects with  prices
            //    Subjects["rejto"].Add(new Subject(Constants.rejto_targyak[i], (i + 1) * 1000));
            //}
            //
            //Subjects.Add("kando", new ObservableCollection<Subject>());
            //for (int i = 0; i < Constants.kando_targyak.Length; i++)
            //{
            //    //adding the subjects with  prices
            //    Subjects["kando"].Add(new Subject(Constants.kando_targyak[i], (i + 1) * 1000));
            //}

        }

        public void ChangePuppetKey(object p, object selected, object unselected)
        {
            //we have to do casting as well as remapping the user-fruendly names to the keyes we used
            //of course we could just changed the keyes themselves, but then we needed to change it everywhere
            string temp = "";
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


            }
            
          
                (p as Player).PuppetKey = temp;

        }

        public bool CheckIfApplicable(string name)
        {
            string temp = "";
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


            }

            return Players.FirstOrDefault(pl => pl.PuppetKey == temp) != null;
        }
        public void CloseWindows()
        {
            //we signal that the game now begins
            CloseOpenWindows?.Invoke(null, null);
        }
        #endregion

        public void DismissRandomSubject()
        {
            
            string message = "";
            if (Player.Subjects.Count > 0)
            {
                Subject sub = Player.Subjects[Rand.Next(0, Player.Subjects.Count)];
                DismissSubject(sub, Player);
                message = $"Eltávolítottuk {sub.Name} nevű tárgyadat!";
            }
            else
            {
                message = "Még nincsenek tárgyaid, ezért ez rád nem érvényes!";
            }
            NotifyPlayer?.Invoke(null, new TransferEventArgs(message));
        }

        public void DismissSubject(Subject sub, Player player)
        {
            player.Subjects.Remove(sub);
            Subjects[player.PuppetKey].Add(sub);
        }

        public void TakeStep(int step)
        {
            GoToPosition(Player.CurrentCard + step);
        }

        public void Quit()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Checks if any of the players are in game; returns true if the game can go on
        /// </summary>
        /// <returns></returns>
        public bool IsTheGameStillOn()
        {
            int i = 0;
            while (i < Players.Count && Players[i].State == PlayerState.lost)
            {
                i++;
            }
            return i < Players.Count;
        }
        /// <summary>
        /// Checks if the player satisfies the conditions for winning; returns true if the player does
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool CheckWinningConditions(Player p)
        {
            return p.Subjects.Count == 3 && p.Money >= 0;
        }



    }
}
