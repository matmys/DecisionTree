using System.Windows;

namespace DecisionTree.Model
{
    public class EndModel : ModelItem
    {


        #region EndProbability Property
        public double EndProbability
        {
            get { return (double)GetValue(EndProbabilityProperty); }
            set { SetValue(EndProbabilityProperty, value); }
        }

        public static readonly DependencyProperty EndProbabilityProperty =
            DependencyProperty.Register("EndProbability", typeof(double), typeof(EndModel), new PropertyMetadata((double)0));
        #endregion

        #region EndPayout Property
        public double EndPayout
        {
            get { return (double)GetValue(EndPayoutProperty); }
            set { SetValue(EndPayoutProperty, value); }
        }

        public static readonly DependencyProperty EndPayoutProperty =
            DependencyProperty.Register("EndPayout", typeof(double), typeof(EndModel), new PropertyMetadata((double)0));
        #endregion

    }
}
