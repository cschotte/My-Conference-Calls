using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.UserData;

namespace MyConferenceCallsTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private static volatile bool _classInitialized;

        private int AppointmentsCount = 0;

        public ScheduledAgent()
        {
            if (!_classInitialized)
            {
                _classInitialized = true;

                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    Application.Current.UnhandledException += ScheduledAgent_UnhandledException;
                });
            }
        }

        private void ScheduledAgent_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void OnInvoke(ScheduledTask task)
        {
            // Get Appointments
            Appointments appts = new Appointments();

            //Identify the method that runs after the asynchronous search completes.
            appts.SearchCompleted += new System.EventHandler<AppointmentsSearchEventArgs>(appts_SearchCompleted);

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddHours(12);

            int max = 40;

            //Start the asynchronous search.
            appts.SearchAsync(start, end, max, null);
        }

        void appts_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            AppointmentsCount = 0;

            if (e.Results != null)
            {
                foreach (Appointment appt in e.Results)
                {
                    if (IsConfCall(appt))
                    {
                        AppointmentsCount++;
                    }
                }
            }

            // update the Tile
            ShellTile PrimaryTile = ShellTile.ActiveTiles.First();
            if (PrimaryTile != null)
            {
                IconicTileData tile = new IconicTileData();
                tile.Count = AppointmentsCount;
                tile.Title = "My Conference Calls";
                tile.SmallIconImage = new Uri("/ApplicationIcon.png", UriKind.Relative);
                tile.IconImage = new Uri("/icon200.png", UriKind.Relative);
                PrimaryTile.Update(tile);
            }

            NotifyComplete();
        }

        bool IsConfCall(Appointment appt)
        {
            if (appt == null) return false;
            if (string.IsNullOrWhiteSpace(appt.Details)) return false;

            string[] patterns = { @"Conference ID: *(\d+)", @"gotomeeting.com/join/(\d+)", @"Call part. code *(\d+)", @"Participant code: *(\d+)", @"Meeting number: *(\d+)" };

            foreach (string pattern in patterns)
            {
                Match match = Regex.Match(appt.Details, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (match.Success) return true;
            }

            return false;
        }
    }
}