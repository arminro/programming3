using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Beadando.Model;

namespace Beadando.ViewModel
{
    class BL
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
            resourceNamesNormal = new string[] { "go", "event", "roll", "enroll", "uv", "randi", "neptun" };
            resourceNamesSquare = new string[] { "megajanlott", "lead", "einstein" };

            greens = new string[] { "go", "start", "roll", "einstein" };
            blacks = new string[] { "uv", "randi", };
            yellows = new string[] { "event" };
            blues = new string[] { "enroll", "megajanlott", "neptun", "lead" };

            GameBoard = new CircularDictionary<BoardField>();

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

            rand = new Random();
           
            //millis = new List<long>();
            
            SetMetrics();
            GenerateOrderOfCards();
            //P.Currentposition = GameBoard[0].Rect;
            Players = new CircularList<Player>(); //the max number of players is 3
            Players.Add(new Player("nik", GameBoard[0].Rect, 0, "Nikes kocka"));
            GameBoard[0].ArriveAtPosition(Players[0]);

            Players.Add(new Player("kando", CalculateSecondaryPosition(GameBoard[0].Rect), 1, "Részeg Kandós"));
            GameBoard[0].ArriveAtPosition(Players[1]);

            Players.Add(new Player("rejto", CalculateTertialPosition(GameBoard[0].Rect), 2, "Cuki rejtős lány <3 <3"));
            GameBoard[0].ArriveAtPosition(Players[2]);

            NextRound();

        }


        CircularDictionary<BoardField> gameBoard; //dict to hold the cards of the board
        Dictionary<string, object> eventCardTexts;
        Random rand;
        CircularList<Player> players;
        public event EventHandler Invalidate; //provides a way to invalidate the visual from hte view
        public event EventHandler<CardEventArgs> EventCard;

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

        Player p; //reference to the actual player
        int leftVerticalAlign;
        int incrementAtMovement;
        int upperVerticalAlign;
        int rightVerticalAlign;
        int startPosition;
        int lowerHorizontalAlign;

        float puppetDiameter; //the diameter of the player puppet
        float puppetDiameterChangeConstant; //the constant to which the diameter of the puppet changes
        int roundIncrementor; //this var is incremented to change players



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

        #endregion

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

        public  CircularList<Player> Players
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
            int boundary = position > Player.CurrentCard ? position - Player.CurrentCard : (GameBoard.Count - Player.CurrentCard) + position;
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
                    EventCard?.Invoke(this, new CardEventArgs(GameBoard[Player.CurrentCard].ImageKey));
                    NextRound();

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

            

            switch (orderOnTheCard)
            {
                //if the player is the 1st to stand on the card
                case 0:
                    goalPosition = GameBoard[positionNumber].Rect;
                    break;
                    //if the player is 2nd on the card
                case 1:
                    goalPosition = CalculateSecondaryPosition(GameBoard[positionNumber].Rect);
                    break;
                    //otherwise, which is if the player is 3rd on the card
                default:
                    goalPosition = CalculateTertialPosition(GameBoard[positionNumber].Rect);
                    break;
            }

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
                    Invalidate?.Invoke(null, null); //we just need a way to convey the invalidate visual from here
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
            Player = Players.GetNextElement();
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
            rand = new Random();
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
                    b.ImageKey = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
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
                    b.ImageKey = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
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
                    b.ImageKey = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
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
                    b.ImageKey = resourceNamesSquare[rand.Next(0, resourceNamesSquare.Length)];
                }
                else
                {
                    //temp[indexer++] = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                    b.ImageKey = resourceNamesNormal[rand.Next(0, resourceNamesNormal.Length)];
                }
                GameBoard.Add(indexer++, b);
            }
            //to make sure that the game can be won, 3 of the cards are switched to a win card randomly
            for (int i = 0; i < 3; i++)
            {
                //we start form 1 bc the start cannot be switched
                GameBoard[rand.Next(1, GameBoard.Count)].ImageKey = "enroll";
                
            }
            //GameBoard[2].ImageKey = "megajanlott"; //USED FOR TESTING!!!

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
        public Tuple<string, int> GetTextToDisplay(string cardKey)
        {

            
            StringBuilder builder = new StringBuilder();
            object temp = EventCardTexts[cardKey];
            int boundary = 0; //the number of characters allowed in 1 line  

            if(temp is string)
            {
                builder = new StringBuilder(temp as string);
            }
            else if(temp is string[])
            {
                string[] tempArray = temp as string[];
                builder = new StringBuilder(tempArray[rand.Next(0, tempArray.Length)]);
            }
            else if(temp is int[])
            {
                int[] tempArray = temp as int[];
                builder = new StringBuilder($"Lépj előre: {tempArray[rand.Next(0, tempArray.Length)]}");
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
            return Tuple.Create<string, int>(builder.ToString(), fontsize);

        }



    }
}
