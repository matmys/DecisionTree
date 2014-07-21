using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DecisionTree.Controls 
{
    public class ImageToggleButton : ToggleButton
    {
        #region Image Property
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageToggleButton), new PropertyMetadata(default(ImageSource)));
        #endregion
    }
}
