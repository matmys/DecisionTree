using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public class ImageButton : Button
    {
        #region Image Property
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(default(ImageSource)));
        #endregion

    }
}
