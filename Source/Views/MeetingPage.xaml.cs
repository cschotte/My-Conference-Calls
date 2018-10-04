using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyConferenceCalls.Resources;

namespace MyConferenceCalls
{
    public partial class MeetingPage : PhoneApplicationPage
    {
        public MeetingPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            // ApplicationBar is not bindable, so for multi language we need code :-(
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = AppResources.Call;
        }

        private void CallButton_Click(object sender, EventArgs e)
        {
            Conference.Call(App.ViewModel.Meeting);
        }
    }
}