// <copyright file="Bindable.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A class responsible for handling the notification of property changes
    /// </summary>
    public abstract class Bindable : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the event with the CMN Roslyn attribute
        /// </summary>
        /// <param name="prop">The peroperty which was updated, the CMN takes care of adding value to it</param>
        protected void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
