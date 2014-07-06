using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DecisionTree.Controls
{
    public partial class DesignerGrid :UserControl
    {
        public KeyGestureManager KeyGestureManager = new KeyGestureManager();

        public DesignerGrid()
        {
            InitializeComponent();
            connectionDecorator.InitializeConnections(this);
            rubberbandSelection.InitializeConnections(this);

            KeyGestureManager.Del_KeyGesture += delete_KeyGesture;
            KeyGestureManager.Crtl_A_KeyGesture += selectAll_KeyGesture;
        }

        #region Delete Command
        private void delete_KeyGesture(object sender, KeyEventArgs e)
        {
            foreach (DesignerItem item in this.SelectedItems.ToList())
            {
                item.Delete();
            }
        }
        #endregion

        #region SelectAll Command
        private void selectAll_KeyGesture(object sender, KeyEventArgs e)
        {
            foreach (DesignerItem item in this.Designer.Children.OfType<DesignerItem>())
            {
                item.IsSelected = true;
            }
        }
        #endregion
    }
}
