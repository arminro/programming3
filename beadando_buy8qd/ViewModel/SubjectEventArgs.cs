using Beadando.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando.ViewModel
{
    public class SubjectEventArgs : EventArgs
    {
        public SubjectEventArgs(Player player, ObservableCollection<Subject> availableSubjects, bool isSubjectForFree)
        {
            this.Player = player;
            this.AvailableSubjects = availableSubjects;
            this.IsSubjectFree = isSubjectForFree;
        }

        public Player Player { get; set; }
        public bool IsSubjectFree { get; set; }

        public ObservableCollection<Subject> AvailableSubjects { get; set; }
    }
}
