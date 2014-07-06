using System.Windows;
using System.Windows.Controls;

namespace DecisionTree.Controls
{
    public partial class Toolbox : UserControl
    {
        #region ItemSize Property
        public Size ItemSize
        {
            get { return (Size)GetValue(ItemSizeProperty); }
            set { SetValue(ItemSizeProperty, value); }
        }

        public static readonly DependencyProperty ItemSizeProperty =
            DependencyProperty.Register("ItemSize", typeof(Size), typeof(Toolbox), new PropertyMetadata(new Size(50,50)));
        #endregion

        #region ItemScale Property
        public double ItemScale
        {
            get { return (double)GetValue(ItemScaleProperty); }
            set { SetValue(ItemScaleProperty, value); }
        }

        public static readonly DependencyProperty ItemScaleProperty =
            DependencyProperty.Register("ItemScale", typeof(double), typeof(Toolbox), new PropertyMetadata(0.7));
        #endregion

        public Toolbox()
        {
            InitializeComponent();
        }
    }
}
