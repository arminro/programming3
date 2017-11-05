// <copyright file="SubjectEventArgs.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using Beadando.Model;

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
