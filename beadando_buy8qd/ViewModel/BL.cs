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

            resourceNamesNormal = new string[] { "go", "event", "roll", "enroll", "uv", "randi", "neptun" };
            resourceNamesSquare = new string[] { "megajanlott", "lead", "einstein" };
            GameBoard = new CircularDictionary<BoardField>();
            rand = new Random();
            P = new Player("nik");
            //millis = new List<long>();

            SetMetrics();
            GenerateOrderOfCards();
            P.Currentposition = GameBoard[0].Rect;
            

        }


        CircularDictionary<BoardField> gameBoard; //dict to hold the cards of the board
        Random rand;

        public event EventHandler Invalidate; //provides a way to invalidate the visual from hte view
        public event EventHandler<CardEventArgs> GetCardKey;

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

        Player p;
        int leftVerticalAlign;
        int incrementAtMovement;
        int upperVerticalAlign;
        int rightVerticalAlign;
        int startPosition;
        int lowerHorizontalAlign;

        float puppetDiameter;
        float puppetDiameterChangeConstant;




        public string[] resourceNamesNormal;
        public string[] resourceNamesSquare;


        string[] resourceNamesNormalLeft;
        string[] resourceNamesNormalRight;

        int movementSpeed;
        //List<long> millis; //used to calc the average of the 

        internal Player P
        {
            get
            {
                return p;
            }

            private set
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
            LowerHorizontalAlign += offset;
            



            float temp = ((float)(NormalCard.width + offset) / (float)NormalCard.width);
            PuppetDiameter *= temp;
            SetMetrics();
        }

        public void MoveHorizontally(int offset)
        {
            StartPosition += offset;
            //megcsinálni az összes bábura
            P.Currentposition = Point.Add(P.Currentposition, new Vector(offset, 0));
            
            SetMetrics();
        }

        public void MoveVertically(int offset)
        {
            LowerHorizontalAlign += offset;
            //összes bábura!!!
            P.Currentposition = Point.Add(P.Currentposition, new Vector(0, offset));

            
            SetMetrics();
        }
        /// <summary>
        /// Goes to the specified position from the current one. 
        /// </summary>
        /// <param name="position">The key of the destination card</param>
        public void GoToPosition(int position)
        {
            /*We have to keep stepping from the current position, but the number of steps has to start from 0*/
            int positionHolder = p.CurrentCard; //we start off from the current position of the player
            int startingPosition = p.CurrentCard;
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Send);
            int incr = 0; 
            timer.Interval = TimeSpan.FromMilliseconds(422); //422 //kókány: 328.10526
            timer.Start();
            timer.Tick += (o, args) => {
           
                //we get the number of cards between the player and the destination card
                
                if (incr <= Math.Abs(position-startingPosition))
                {
                    Step(positionHolder++);
                    incr++;
                }
                else
                {
                    timer.Stop();
                    p.CurrentCard = GetPlayerCardNumber(p);
                    MessageBox.Show($"Now I stand on card{p.CurrentCard}");
                    //MessageBox.Show($"Average run stat: {millis.Average().ToString()}");
                }
            };
            //Step(31);

            


        }

        int GetPlayerCardNumber(Player p)
        {
            return GameBoard.Where(g => 
            IsThePlayerInTheVicinity(g.Value.Rect, p.Currentposition) == true)
            .FirstOrDefault().Key;
        }

        bool IsThePlayerInTheVicinity(Point cardRect, Point playerCurrentPosition)
        {
            //the player stands on the card if it is witin a diameter of 10 units
            int vicinity = 10;
            return Math.Abs(Point.Subtract(cardRect, playerCurrentPosition).Length) <= vicinity;
        }

        public void Step(int positionNumber)
        {

            //RegisterName("player", p.Currentposition);

            //Stopwatch watch = new Stopwatch();
            //update the current position
            Point goalPosition = new Point(GameBoard[positionNumber].Rect.X, GameBoard[positionNumber].Rect.Y);
            Vector v = new Vector(goalPosition.X - P.Currentposition.X, goalPosition.Y - P.Currentposition.Y);
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
                    P.Currentposition = Point.Add(P.Currentposition, v / divide);
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



    }
}
