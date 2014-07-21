using System;

namespace DecisionTree.Extensions
{
    public class SettingsChangedEventArgs : EventArgs
    {
        public string Key { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }

        public SettingsChangedEventArgs(string key, object oldValue, object newValue)
        {
            this.Key = key;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
}
