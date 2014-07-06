using Microsoft.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Markup;
using System;
using DecisionTree.Extensions;
using DecisionTree.Model;

namespace DecisionTree.Controls
{
    public partial class DesignerGrid : UserControl
    {
        public bool IsLeftMouseButtonClicked = false;
        public IEnumerable<DesignerItem> SelectedItems
        {
            get
            {
                return this.Designer.Children.OfType<DesignerItem>().Where(item => item.IsSelected == true);
            }
        }

        #region GridSize Property
        public int GridSize
        {
            get { return (int)GetValue(GridSizeProperty); }
            set { SetValue(GridSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridSizeProperty =
            DependencyProperty.Register("GridSize", typeof(int), typeof(DesignerGrid), new PropertyMetadata(1));
        #endregion

        private void DesignerGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == Designer)
            {
                IsLeftMouseButtonClicked = true;
                Designer.CaptureMouse();

                //Deselect all DesignerItems
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) == ModifierKeys.None)
                {
                    DeselectAll();
                }
            }
        }

        public void Designer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsLeftMouseButtonClicked = false;
            Designer.ReleaseMouseCapture();
        }

        public void DeselectAll()
        {
            foreach (DesignerItem item in SelectedItems)
            {
                item.IsSelected = false;
            }
        }

        #region PanelDragDrop Handling

        private void PanelDragDropTarget_Drop(object sender, Microsoft.Windows.DragEventArgs e)
        {
            ToolboxItem item = GetToolboxItemFromDropData(e);

            //Get drop position relative to "Designer" Grid
            Point pos = e.GetPosition(Designer);

            bool isFirstNode = Designer.Children.OfType<DesignerItem>().Count() == 0;

            if (item != null)
            {
                if (connectionDecorator.IsConnected || isFirstNode)
                {
                    StringToPathGeometryConverter converter = new StringToPathGeometryConverter();

                    Path path = new Path();
                    string pathData = Application.Current.Resources[item.StyleString + "Data"] as string;
                    path.Data = converter.Convert(pathData);
                    path.IsHitTestVisible = false;
                    path.Style = item.ItemStyle;

                    DesignerItem newDesignerItem = new DesignerItem(ModelItem.GetModelItem(item.StyleString));
                    newDesignerItem.Height = MathExtensions.RoundUp(item.ActualHeight * item.ItemScale, GridSize);
                    newDesignerItem.Width = MathExtensions.RoundUp(item.ActualWidth * item.ItemScale, GridSize);
                    newDesignerItem.AdditionalContent = path;

                    //Attaching DragThumbControl style to fit DesignerItem content
                    newDesignerItem.DragControl.thumbControl.Style = item.DragThumbStyle;

                    double deltaVertical = 0, deltaHorizontal = 0;

                    if (item.DragPoint.HasValue)
                    {
                        deltaVertical = item.DragPoint.Value.Y * item.ItemScale;
                        deltaHorizontal = item.DragPoint.Value.X * item.ItemScale;
                    }

                    double x = Math.Max(0, pos.X - deltaHorizontal);
                    double y = Math.Max(0, pos.Y - deltaVertical);

                    x = MathExtensions.RoundDown(x, GridSize);
                    y = MathExtensions.RoundDown(y, GridSize);

                    newDesignerItem.SetPosition(new Point(x, y), GridSize);

                    Designer.Children.Add(newDesignerItem);

                    newDesignerItem.SelectItem();

                    if (!isFirstNode)
                    {
                        LabeledConnectionLine newLine = connectionDecorator.CopyAsConnectionLine(newDesignerItem.InputPoint);
                        newDesignerItem.SetParent(newLine.ParentItem, newLine);
                        Designer.Children.Add(newLine);
                    }
                }
            }
        }

        private void PanelDragDropTarget_ItemDragStarting(object sender, ItemDragEventArgs e)
        {
            e.Cancel = true;
            e.Handled = true;
        }

        public ToolboxItem GetToolboxItemFromDropData(Microsoft.Windows.DragEventArgs e)
        {
            //Get dropped ToolboxItem
            ItemDragEventArgs eventArgs = e.Data.GetData(e.Data.GetFormats()[0]) as ItemDragEventArgs;
            SelectionCollection collection = eventArgs.Data as SelectionCollection;
            ToolboxItem item = collection.Select(t => t.Item).OfType<ToolboxItem>().FirstOrDefault();

            return item;
        }

        #endregion

    }
}
