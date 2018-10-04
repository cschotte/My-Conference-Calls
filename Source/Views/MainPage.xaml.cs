using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Scheduler;
using MyConferenceCalls.Resources;
using Microsoft.Phone.Shell;

namespace MyConferenceCalls
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;

            // ApplicationBar is not bindable, so for multi language we need code :-(
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = AppResources.Settings;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).Text = AppResources.Help;

            ((ApplicationBarMenuItem)ApplicationBar.MenuItems[0]).Text = AppResources.About;
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            this.GoToPage(ApplicationPages.Settings);
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            this.GoToPage(ApplicationPages.Help);
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            this.GoToPage(ApplicationPages.About);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            // If selected index is -1 (no selection) do nothing
            if (listBox == null) return;
            if (listBox.SelectedIndex == -1) return;

            // Call the Conference
            App.ViewModel.Meeting = listBox.Items[listBox.SelectedIndex] as Appointment;
            Conference.Call(App.ViewModel.Meeting);

            // Reset selected index to -1 (no selection)
            listBox.SelectedIndex = -1;
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.Meeting = (((MenuItem)sender).DataContext) as Appointment;

            this.GoToPage(ApplicationPages.Meeting);
        }
    }
}