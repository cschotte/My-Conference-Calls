using System.IO.IsolatedStorage;
using System;

namespace MyConferenceCalls
{
    public static class Settings
    {
        public static TT Read<TT>(string name)
        {
            return Read<TT>(name, default(TT));
        }

        public static TT Read<TT>(string name, TT defaultValue)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            TT value;

            if (settings == null || !settings.TryGetValue<TT>(name, out value))
                return defaultValue;

            return value;
        }

        public static void Write<TT>(string name, TT value)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings == null || value == null)
                return;

            if (settings.Contains(name))
                settings[name] = value;
            else
                settings.Add(name, value);
        }

        public static void Save()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            settings.Save();
        }

        public static void Clear()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            settings.Clear();
        }
    }
}