using ImageTools;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DecisionTree.Extensions
{
    public static class ImageSaver
    {
        public static void SaveAsPNG(UIElement element)
        {
            ExtendedImage myImage = element.ToImage();

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Image Files (*.png)|*.png";

            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    myImage.WriteToStream(stream);
                }
            }
        }
        
    }
}
