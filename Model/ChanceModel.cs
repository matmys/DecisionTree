
using System.Windows;
namespace DecisionTree.Model
{
    public class ChanceModel : ModelItem
    {
        #region CalculatedPayout Property
        public double CalculatedPayout
        {
            get { return (double)GetValue(CalculatedPayoutProperty); }
            set { SetValue(CalculatedPayoutProperty, value); }
        }

        public static readonly DependencyProperty CalculatedPayoutProperty =
            DependencyProperty.Register("CalculatedPayout", typeof(double), typeof(ChanceModel), new PropertyMetadata((double)0));
        #endregion

        public ChanceModel()
        {

        }
    }
}
