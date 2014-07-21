using System.Linq;
using DecisionTree.Extensions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace DecisionTree.Controls
{
    public partial class ResizeThumbControl
    {
        DesignerItem designerItem;
        DesignerGrid designer;

        public ResizeThumbControl()
        {
            InitializeComponent();
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            designerItem = this.DataContext as DesignerItem;

            if (designerItem != null)
            {
                designer = this.FindAncestor(typeof(DesignerGrid)) as DesignerGrid;
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;

            if (designerItem != null && designer != null && designerItem.IsSelected && thumb != null)
            {
                double minLeft, minTop, minDeltaHorizontal, minDeltaVertical;
                double dragDeltaVertical, dragDeltaHorizontal, scale;

                List<DesignerItem> selectedItems = designer.SelectedItems.ToList();

                CalculateDragLimits(selectedItems, out minLeft, out minTop, out minDeltaHorizontal, out minDeltaVertical);
                
                foreach (DesignerItem item in selectedItems)
                {
                    switch (thumb.VerticalAlignment)
                    {
                        case VerticalAlignment.Bottom:
                            dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                            scale = (item.Height - dragDeltaVertical) / item.Height;
                            item.Height = item.Height * scale;
                            break;
                        case VerticalAlignment.Top:
                            dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                            scale = (item.Height - dragDeltaVertical) / item.Height;
                            item.Margin = new Thickness(item.Margin.Left, item.Position.Y + dragDeltaVertical, 0, 0);
                            item.Height = item.Height * scale;
                            break;
                    }

                    switch (thumb.HorizontalAlignment)
                    {
                        case (HorizontalAlignment.Left):
                            dragDeltaHorizontal = Math.Min(Math.Max(-minLeft, e.HorizontalChange), minDeltaHorizontal);
                            scale = (item.Width - dragDeltaHorizontal) / item.Width;
                            item.Margin = new Thickness(item.Margin.Left + dragDeltaHorizontal, item.Position.Y, 0, 0);
                            item.Width = item.Width * scale;
                            break;
                        case (HorizontalAlignment.Right):
                            dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                            scale = (item.Width - dragDeltaHorizontal) / item.Width;
                            item.Width = item.Width * scale;
                            break;
                    }
                }
            }
        }

        private void CalculateDragLimits(IEnumerable<DesignerItem> selectedItems, out double minLeft, out double minTop, out double minDeltaHorizontal, out double minDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;

            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            foreach (DesignerItem item in selectedItems)
            {
                double left = item.Margin.Left;
                double top = item.Margin.Top;

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.Width - item.MinWidth);
                minDeltaVertical = Math.Min(minDeltaVertical, item.Height - item.MinHeight);
            }
        }
    }
}
