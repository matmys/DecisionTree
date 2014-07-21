using System.ComponentModel;
using System.Windows.Media;

namespace DecisionTree.Extensions
{
    public class Notification : INotifyPropertyChanged
    {
        private string text;
        private Brush foreground;

        public string Text
        {
            get 
            { return text; }

            set
            {
                this.text = value;
                this.OnPropertyChanged("Text");
            }
        }
        public Brush Foreground 
        { 
            get { return foreground; }
            set
            {
                this.foreground = value;
                this.OnPropertyChanged("Foreground");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Notification()
        {
            this.Text = "";
            this.Foreground = new SolidColorBrush(Colors.Black);
        }

        public Notification(string notification)
        {
            this.Text = notification;
            this.Foreground = new SolidColorBrush(Colors.Black);
        }

        public Notification(string notification, Brush foreground)
        {
            this.Text = notification;
            this.Foreground = foreground;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
