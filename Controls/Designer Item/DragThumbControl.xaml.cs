using DecisionTree.Extensions;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DecisionTree.Controls
{
    public partial class DragThumbControl
    {
        DesignerGrid designer;
        DesignerItem designerItem;

        public DragThumbControl()
        {
            InitializeComponent();
        }

        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (designerItem != null && designer != null && designerItem.IsSelected)
            {
                double minLeft = double.MaxValue;
                double minTop = double.MaxValue;

                foreach (DesignerItem item in designer.SelectedItems)
                {
                    minLeft = Math.Min(item.AbsolutePosition.X, minLeft);
                    minTop = Math.Min(item.AbsolutePosition.Y, minTop);
                }

                // prevent draging over left and top border of the designer
                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                foreach (DesignerItem item in designer.SelectedItems)
                {
                    item.Position = new Point(item.AbsolutePosition.X + deltaHorizontal, item.AbsolutePosition.Y + deltaVertical);
                }

                this.designer.InvalidateMeasure();
            }
        }

        private void DragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as DesignerItem;

            if (this.designerItem != null)
            {
                this.designerItem.SelectItem();
                this.designer = this.FindAncestor(typeof(DesignerGrid)) as DesignerGrid;
            }
        }

        private void DragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            foreach (DesignerItem item in designer.SelectedItems)
            {
                double x = item.Position.X;
                double y = item.Position.Y;

                item.Position = new Point(x, y);
            }
        }
    }
}
