// <copyright file="SubjectEventArgs.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using Beadando.Model;

    /// <summary>
    /// Descendant from eventargs, responsible for passing inter-window data about subjects
    /// </summary>
    public class SubjectEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectEventArgs"/> class.
        /// </summary>
        /// <param name="player">reference to the player</param>
        /// <param name="availableSubjects">reference to the collection holding the subjects available to the player</param>
        /// <param name="isSubjectForFree">checks if the player is about to get the subjects for free</param>
        public SubjectEventArgs(Player player, ObservableCollection<Subject> availableSubjects, bool isSubjectForFree)
        {
            this.Player = player;
            this.AvailableSubjects = availableSubjects;
            this.IsSubjectFree = isSubjectForFree;
        }

        /// <summary>
        /// Gets or sets the player instance
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the subjects are for free
        /// </summary>
        public bool IsSubjectFree { get; set; }

        /// <summary>
        /// Gets or sets the collection holding the subjects the player can acquire
        /// </summary>
        public ObservableCollection<Subject> AvailableSubjects { get; set; }
    }
}
