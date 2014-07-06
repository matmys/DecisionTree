using System.Windows;

namespace DecisionTree.Model
{
    public class ConnectionProperties : DependencyObject
    {

        #region HasProbability
        public bool HasProbability
        {
            get { return (bool)GetValue(HasProbabilityProperty); }
            set { SetValue(HasProbabilityProperty, value); }
        }

        public static readonly DependencyProperty HasProbabilityProperty =
            DependencyProperty.Register("HasProbability", typeof(bool), typeof(ConnectionProperties), new PropertyMetadata(false));
        #endregion

        #region Label Property
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ConnectionProperties), new PropertyMetadata(""));
        #endregion

        #region Payout Property
        public double Payout
        {
            get { return (double)GetValue(PayoutProperty); }
            set { SetValue(PayoutProperty, value); }
        }

        public static readonly DependencyProperty PayoutProperty =
            DependencyProperty.Register("Payout", typeof(double), typeof(ConnectionProperties), new PropertyMetadata((double)0));
        #endregion

        #region Probability Property
        public double Probability
        {
            get { return (double)GetValue(ProbabilityProperty); }
            set { SetValue(ProbabilityProperty, value); }
        }

        public static readonly DependencyProperty ProbabilityProperty =
            DependencyProperty.Register("Probability", typeof(double), typeof(ConnectionProperties), new PropertyMetadata((double)0));
        #endregion

    }
}
