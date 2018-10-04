using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using MyConferenceCalls.Resources;

namespace MyConferenceCalls
{
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            var CurrentAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            string VersionNumber = CurrentAssembly.Split('=')[1].Split(',')[0];

            versionTextBlock.Text = string.Format(versionTextBlock.Text, VersionNumber);
        }

        private void reviewButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask review = new MarketplaceReviewTask();
            review.Show();
        }

        private void supportButton_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask mail = new EmailComposeTask();
            mail.To = AppResources.EMail;
            mail.Subject = AppResources.ApplicationTitleLong;
            mail.Show();
        }
    }
}