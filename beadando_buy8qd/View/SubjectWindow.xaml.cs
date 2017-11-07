// <copyright file="SubjectWindow.xaml.cs" company="OE-NIK">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Beadando.View
{
    using System.Windows;
    using System.Windows.Controls;
    using Beadando.ViewModel;

    /// <summary>
    /// Interaction logic for SubjectWindow.xaml
    /// </summary>
    public partial class SubjectWindow : Window
    {
        private BL bl;
        private bool free;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectWindow"/> class.
        /// </summary>
        /// <param name="bl">reference to the business logic instance</param>
        /// <param name="free">indicates wether the subjects will be for free</param>
        public SubjectWindow(BL bl, bool free = false)
        {
            this.InitializeComponent();
            this.bl = bl;
            this.DataContext = bl;
            this.free = free;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.bl.AddSubjectToPlayer(this.bl.Player, this.bl.SelectedSubject, this.free);
            this.DialogResult = true;
            e.Handled = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            e.Handled = true;
        }

        private void Lb_available_subjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // we could cast selecteditem as a subject here, but since SW is in View, it has no notion of the class Model.Subject
            object selected = (e.OriginalSource as ListBox).SelectedItem;
            if (selected != null)
            {
                this.bl.CanPlayerBuySubject(selected);
            }
        }
    }
}
