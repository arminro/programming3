using Beadando.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.ViewModel
{
    class SubjectEventArgs : EventArgs
    {
        public SubjectEventArgs(Player player, List<Subject> availableSubjects)
        {
            this.Player = player;
            this.AvailableSubjects = availableSubjects;
        }

        public Player Player { get; set; }
        public List<Subject> AvailableSubjects { get; set; }
    }
}
