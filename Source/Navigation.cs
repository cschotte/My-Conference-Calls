using System;
using Microsoft.Phone.Controls;

namespace MyConferenceCalls
{
    public enum ApplicationPages
    {
        Settings,
        Help,
        About,
        Meeting
    }

    public static class Navigation
    {
        public static void GoToPage(this PhoneApplicationPage phoneApplicationPage, ApplicationPages applicationPage)
        {
            switch (applicationPage)
            {
                case ApplicationPages.Settings:
                    phoneApplicationPage.NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
                    break;

                case ApplicationPages.Help:
                    phoneApplicationPage.NavigationService.Navigate(new Uri("/Views/HelpPage.xaml", UriKind.Relative));
                    break;

                case ApplicationPages.About:
                    phoneApplicationPage.NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
                    break;

                case ApplicationPages.Meeting:
                    phoneApplicationPage.NavigationService.Navigate(new Uri("/Views/MeetingPage.xaml", UriKind.Relative));
                    break;
            }
        }
    }
}
