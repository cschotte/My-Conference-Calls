using System.Windows;
using Microsoft.Phone.Controls;
using System;
using System.Threading;
using System.Collections.Generic;
using MyConferenceCalls.Resources;
using System.IO.IsolatedStorage;

namespace MyConferenceCalls
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        bool loaded = false;

        public SettingsPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: dit kan beter
            switch (Thread.CurrentThread.CurrentCulture.Name)
            {
                case "en-US":
                    LanguagePicker.SelectedIndex = 0;
                    break;

                case "nl-NL":
                    LanguagePicker.SelectedIndex = 1;
                    break;

                case "es-ES":
                    LanguagePicker.SelectedIndex = 2;
                    break;

                case "en-GB":
                    LanguagePicker.SelectedIndex = 3;
                    break;
            }

            loaded = true;
        }

        private void LanguagePicker_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LanguagePicker == null || LanguagePicker.SelectedItem == null || loaded == false) return;

            var selectedLanguage = (KeyValuePair<string, string>)LanguagePicker.SelectedItem;
            if (selectedLanguage.Key == Thread.CurrentThread.CurrentCulture.Name) return;

            App.ChangeCulture(selectedLanguage.Key);

            MessageBox.Show(AppResources.RestartForLanguage, AppResources.ApplicationTitleNormal, MessageBoxButton.OK);
        }
    }
}