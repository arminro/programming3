using Beadando.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Beadando.View
{
    /// <summary>
    /// Interaction logic for SubjectWindow.xaml 
    /// </summary>
    public partial class SubjectWindow : Window
    {

        BL bl;
        bool free;
        public SubjectWindow(BL bl, bool free = false)
        {
            InitializeComponent();
            this.bl = bl;
            this.DataContext = bl;
            this.free = free;
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            bl.AddSubjectToPlayer(bl.Player, bl.SelectedSubject, free);
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Lb_available_subjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //we could cast selecteditem as a subject here, but since SW is in View, it has no notion of the class Model.Subject
            object selected = (e.OriginalSource as ListBox).SelectedItem;
            if (selected != null)
            {
                bl.CanPlayerBuySubject(selected); 
            }
        }
    }
}
