// <copyright file="TransferEventArgs.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A class inheriting from EventArgs in charge of tranfering strings from one namespace to another through events
    /// </summary>
    public class TransferEventArgs : EventArgs
    {
        public TransferEventArgs(string load)
        {
            this.Load = load;
        }

        public string Load { get; set; }
    }
}
