using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Phone.UserData;
using Microsoft.Phone.Shell;
using System.Threading;

namespace MyConferenceCalls
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Today = new ObservableCollection<Appointment>();
            this.Tomorrow = new ObservableCollection<Appointment>();
            this.Language = new Dictionary<string, string>();
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            LoadLanguages();
            LoadAppointments();

            LeaderName = Settings.Read<string>("leadername", string.Empty);
            LeaderPin = Settings.Read<string>("leaderpin", string.Empty);
            ConferenceNumber = Settings.Read<string>("conferencenumber", string.Empty);
            UseLync = Settings.Read<bool>("lync", false);

            this.IsDataLoaded = true;
        }

        public void SaveData()
        {
            int startcount = Settings.Read<int>("startcount", 1);

            Settings.Write<string>("leadername", LeaderName);
            Settings.Write<string>("leaderpin", LeaderPin);
            Settings.Write<string>("conferencenumber", ConferenceNumber);
            Settings.Write<bool>("lync", UseLync);

            Settings.Write<string>("Language", Thread.CurrentThread.CurrentCulture.Name);
            Settings.Write<int>("startcount", startcount);
        }

        #region LeaderName
        private string _LeaderName = string.Empty;
        public string LeaderName
        {
            get { return _LeaderName; }
            set
            {
                if (value != _LeaderName)
                {
                    _LeaderName = value;
                    NotifyPropertyChanged("LeaderName");
                }
            }
        }
        #endregion

        #region LeaderPin
        private string _LeaderPin = string.Empty;
        public string LeaderPin
        {
            get { return _LeaderPin; }
            set
            {
                if (value != _LeaderPin)
                {
                    _LeaderPin = value;
                    NotifyPropertyChanged("LeaderPin");
                }
            }
        }
        #endregion

        #region ConferenceNumber
        private string _ConferenceNumber = string.Empty;
        public string ConferenceNumber
        {
            get { return _ConferenceNumber; }
            set
            {
                if (value != _ConferenceNumber)
                {
                    _ConferenceNumber = value;
                    NotifyPropertyChanged("ConferenceNumber");
                }
            }
        }
        #endregion

        #region UseLync
        private bool _UseLync = false;
        public bool UseLync
        {
            get { return _UseLync; }
            set
            {
                if (value != _UseLync)
                {
                    _UseLync = value;
                    NotifyPropertyChanged("UseLync");
                }
            }
        }
        #endregion

        #region Appointment
        public ObservableCollection<Appointment> Today { get; private set; }
        public ObservableCollection<Appointment> Tomorrow { get; private set; }

        private void LoadAppointments()
        {
            Appointments appts = new Appointments();

            //Identify the method that runs after the asynchronous search completes.
            appts.SearchCompleted += new EventHandler<AppointmentsSearchEventArgs>(Appointments_SearchCompleted);

            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddHours(12);
            int max = 40;

            //Start the asynchronous search.
            appts.SearchAsync(start, end, max, null);
        }

        private void Appointments_SearchCompleted(object sender, AppointmentsSearchEventArgs e)
        {
            if (e.Results != null)
            {
                foreach (Appointment appt in e.Results)
                {
                    string ConferenceId = Conference.GetId(appt);

                    if (!string.IsNullOrEmpty(ConferenceId))
                    {
                        if (appt.StartTime > DateTime.Now.Date.AddDays(1))
                        {
                            Tomorrow.Add(appt);
                        }
                        else
                        {
                            Today.Add(appt);
                        }
                    }
                }
            }

            // update the Tile
            ShellTile PrimaryTile = ShellTile.ActiveTiles.First();
            if (PrimaryTile != null)
            {
                IconicTileData tile = new IconicTileData();
                tile.Count = Today.Count;
                tile.Title = "My Conference Calls";
                tile.SmallIconImage = new Uri("/ApplicationIcon.png", UriKind.Relative);
                tile.IconImage = new Uri("/AppHub/icon200.png", UriKind.Relative);
                PrimaryTile.Update(tile);
            }
        }
        #endregion

        #region Language
        public Dictionary<string, string> Language { get; private set; }

        private void LoadLanguages()
        {
            Language.Add("en-US", new CultureInfo("en-US").NativeName);
            Language.Add("nl-NL", new CultureInfo("nl-NL").NativeName);
            Language.Add("es-ES", new CultureInfo("es-ES").NativeName);
            Language.Add("en-GB", new CultureInfo("en-GB").NativeName);
        }
        #endregion

        #region Meeting
        private Appointment _Meeting;
        public Appointment Meeting
        {
            get { return _Meeting; }
            set
            {
                if (value != _Meeting)
                {
                    _Meeting = value;
                    NotifyPropertyChanged("Meeting");
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}