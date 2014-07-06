using DecisionTree.Extensions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public partial class DragThumbControl : UserControl
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

                IEnumerable<DesignerItem> selctedItems = designer.SelectedItems;

                foreach (DesignerItem item in selctedItems)
                {
                    minLeft = Math.Min(item.AbsolutePosition.X, minLeft);
                    minTop = Math.Min(item.AbsolutePosition.Y, minTop);
                }

                // prevent draging over left and top border of the designer
                double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                foreach (DesignerItem item in selctedItems)
                {
                    Point newPos = new Point(item.AbsolutePosition.X + deltaHorizontal, item.AbsolutePosition.Y + deltaVertical);

                    item.SetPosition(newPos, designer.GridSize);
                }

                this.designer.InvalidateMeasure();
            }
        }

        private void DragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as DesignerItem;
            this.designerItem.SelectItem();

            if (designerItem != null)
            {
                designer = this.FindAncestor(typeof(DesignerGrid)) as DesignerGrid;
            }
        }

        private void DragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            foreach (DesignerItem item in designer.SelectedItems)
            {
                double x = item.Margin.Left;
                double y = item.Margin.Top;

                item.SetPosition(new Point(x, y), this.designer.GridSize);
            }
        }
    }
}
