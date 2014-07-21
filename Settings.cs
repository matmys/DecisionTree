using DecisionTree.Extensions;
using System;
using System.IO.IsolatedStorage;
using System.Windows;

namespace DecisionTree
{
    public class Settings : DependencyObject
    {
        //DesignerGrid Settings

        #region IsGridEnabled Property
        public bool IsGridEnabled
        {
            get { return (bool)GetValue(IsGridEnabledProperty); }
            set { SetValue(IsGridEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsGridEnabledProperty =
            DependencyProperty.Register("IsGridEnabled", typeof(bool), typeof(Settings), new PropertyMetadata(false, OnIsGridEnabledPropertyChanged));

        private static void OnIsGridEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newValue = (bool)e.NewValue;
            Add<bool>("IsGridEnabled", newValue);
        }
        #endregion

        private static Settings instance;

        private Settings() 
        {
            this.IsGridEnabled = Get<bool>("IsGridEnabled");
        }

        public static Settings Instance
        {
            get
            {
                return instance;
            }
        }

        static Settings()
        {
            instance = new Settings();
        }

        public static bool Contains(string key)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings != null)
	        {
		        return settings.Contains(key);
	        }
            else
            {
                return false;
            }
        }

        public static T Get<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        public static T Get<T>(string key, T defaultValue)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            T value;

            if (settings == null || !settings.TryGetValue<T>(key, out value))
            {
                return defaultValue;
            }
            else
            {
                return value;
            }
        }

        public static void Add<T>(string key, T value)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings == null)
            {
                return;
            }

            if (settings.Contains(key))
            {
                settings[key] = value;

            }
            else
            {
                settings.Add(key, value);
            }
            settings.Save();
        }
    }
}
