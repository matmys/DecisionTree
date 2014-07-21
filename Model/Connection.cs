using DecisionTree.Controls;
using System.Windows;

namespace DecisionTree.Model
{
    public class Connection : DependencyObject
    {
        public delegate void PropertyChangedHandler(Connection connection, DependencyPropertyChangedEventArgs e);
        public event PropertyChangedHandler ConnectionPropertyChanged;

        public LabeledConnectionLine ConnectionPresenter { get; set; }

        public ModelItem ParentItem { get; set; }
        public ModelItem ChildItem { get; set; }

        #region Label Property
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(Connection), new PropertyMetadata(""));
        #endregion

        #region Payout Property
        public double Payout
        {
            get { return (double)GetValue(PayoutProperty); }
            set { SetValue(PayoutProperty, value); }
        }

        public static readonly DependencyProperty PayoutProperty =
            DependencyProperty.Register("Payout", typeof(double), typeof(Connection), new PropertyMetadata((double)0, OnPropertyChanged));
        #endregion

        #region Probability Property
        public double Probability
        {
            get { return (double)GetValue(ProbabilityProperty); }
            set { SetValue(ProbabilityProperty, value); }
        }

        public static readonly DependencyProperty ProbabilityProperty =
            DependencyProperty.Register("Probability", typeof(double), typeof(Connection), new PropertyMetadata((double)0, OnPropertyChanged));
        #endregion

        protected static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Connection connection = d as Connection;

            if (connection != null && connection.ConnectionPropertyChanged != null)
            {
                connection.ConnectionPropertyChanged(connection, e);
            }
        }
    }
}
