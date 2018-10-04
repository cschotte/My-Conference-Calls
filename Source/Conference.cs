using System.Windows;
using Microsoft.Phone.UserData;
using System.Text.RegularExpressions;
using Microsoft.Phone.Tasks;
using MyConferenceCalls.Resources;
using Microsoft.Phone.Marketplace;
using System.IO.IsolatedStorage;
using System;

namespace MyConferenceCalls
{
    public static class Conference
    {
        public static string GetId(Appointment appt)
        {
            if(appt == null)
                return string.Empty;

            if(string.IsNullOrWhiteSpace(appt.Details))
                return string.Empty;

            string[] patterns = { @"Conference ID: *(\d+)", @"gotomeeting.com/join/(\d+)", @"Call part. code *(\d+)", @"Participant code: *(\d+)", @"Meeting number: *(\d+)" };

            foreach (string pattern in patterns)
            {
                Match match = Regex.Match(appt.Details, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (match.Success)
                    return match.Groups[1].Value;
            }

            return string.Empty;
        }

        public static string GetLync(Appointment appt)
        {
            if (appt == null)
                return string.Empty;

            Match match = Regex.Match(appt.Details, "Join online meeting<([^>]+)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (match.Success)
                return match.Groups[1].Value;

            return string.Empty;
        }

        public static string GetNumber(Appointment appt)
        {
            if (appt == null) return string.Empty;

            return App.ViewModel.ConferenceNumber;
        }

        public static string GetPin(Appointment appt)
        {
            try
            {
                if (appt.Organizer.DisplayName.ToLower() == App.ViewModel.LeaderName.ToLower())
                {
                    return App.ViewModel.LeaderPin;
                }

                // your are not the leader or have no Pin
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static void Call(Appointment appt)
        {
            string Number = Conference.GetNumber(appt);
            string Id = Conference.GetId(appt);
            string Pin = Conference.GetPin(appt);
            string DailString = string.Empty;

            if (App.ViewModel.UseLync)
            {
                string LyncUrl = GetLync(appt);

                if (!string.IsNullOrWhiteSpace(LyncUrl))
                {
                    WebBrowserTask webbrowser = new WebBrowserTask();
                    webbrowser.Uri = new Uri(LyncUrl, UriKind.Absolute);
                    webbrowser.Show();
                }

                return;
            }

            if (!string.IsNullOrEmpty(Pin))
            {
                DailString = string.Format("{0}pp{1}#pp*{2}", Number, Id, Pin);
            }
            else
            {
                DailString = string.Format("{0}pp{1}#", Number, Id);
            }

            if (!string.IsNullOrEmpty(Number))
            {
                    PhoneCallTask phonecall = new PhoneCallTask();
                    phonecall.PhoneNumber = DailString;
                    phonecall.Show();
            }
            else
            {
                MessageBox.Show(AppResources.WarningNoDailNumber, AppResources.ApplicationTitleNormal, MessageBoxButton.OK);
            }
        }

        public static bool IsTryMode()
        {
            LicenseInformation license = new LicenseInformation();

            if (license.IsTrial())
            {
                int startcount = Settings.Read<int>("startcount", 1);

                if (startcount > 4) return true;

                Settings.Write<int>("startcount", startcount + 1);
            }

            return false;
        }
    }
}
