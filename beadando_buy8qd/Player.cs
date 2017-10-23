using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Beadando.Model;

namespace Beadando
{
    class Player 
    {
        string puppetKey;
        Point currentposition;
        bool[] subjects; //storing the subjects of the player
        

        public Player(string puppetKey)
        {
            this.puppetKey = puppetKey;
            this.Subjects = new bool[5]; //there are 5 subjects
            for (int i = 0; i < Subjects.Length; i++)
            {
                subjects[i] = false;
            }
            
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
                
            }
        }


        public bool[] Subjects
        {
            get
            {
                return subjects;
            }

            private set
            {
                subjects = value;
            }
        }

        public void AddSubject(int subectIdx)
        {
            Subjects[subectIdx] = true;
        }

    }
}
