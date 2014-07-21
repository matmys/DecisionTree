using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System;
using DecisionTree.Extensions;
using DecisionTree.Model;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public partial class DesignerGrid
    {
        public delegate void ModelItemsChangedHandler(ModelItem item, ModelChange change);
        public event ModelItemsChangedHandler ModelItemsChanged;

        public delegate void ConnectionPropertyChangedHandler(Connection connection, DependencyPropertyChangedEventArgs e);
        public event ConnectionPropertyChangedHandler ConnectionPropertyChanged;

        public bool IsLeftMouseButtonClicked;

        public IEnumerable<DesignerItem> SelectedItems
        {
            get
            {
                return this.Children.OfType<DesignerItem>().Where(item => item.IsSelected);
            }
        }
        public UIElementCollection Children
        {
            get { return this.Designer.Children; }
        }

        public int GridSize
        {
            get
            {
                return (IsGridEnabled) ? this.GridSizeValue : 1;
            }
        }

        #region GridSizeValue Property
        public int GridSizeValue
        {
            get { return (int)GetValue(GridSizeValueProperty); }
            set { SetValue(GridSizeValueProperty, value); }
        }

        public static readonly DependencyProperty GridSizeValueProperty =
            DependencyProperty.Register("GridSizeValue", typeof(int), typeof(DesignerGrid), new PropertyMetadata(10));
        #endregion

        #region IsGridEnabled Property
        public bool IsGridEnabled
        {
            get { return (bool)GetValue(IsGridEnabledProperty); }
            set { SetValue(IsGridEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsGridEnabledProperty =
            DependencyProperty.Register("IsGridEnabled", typeof(bool), typeof(DesignerGrid), new PropertyMetadata(false, OnIsGridEnabledPropertyChanged));

        private static void OnIsGridEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerGrid designer = d as DesignerGrid;
            bool newValue = (bool)e.NewValue;

            if (designer != null && newValue == true)
            {
                foreach (DesignerItem child in designer.Children.OfType<DesignerItem>())
                {
                    child.SnapToGridLines();
                }
            }           
        }
        #endregion

        public void InitializeBindings()
        {
            Settings settings = Settings.Instance;

            BindingHelper.RegisterBinding("IsGridEnabled", settings, this, DesignerGrid.IsGridEnabledProperty);
        }

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

        public void DesignerGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

            bool isFirstNode = !this.Children.OfType<DesignerItem>().Any();

            if (item != null)
            {
                if (connectionDecorator.IsConnected || isFirstNode)
                {
                    LabeledDesignerItem newDesignerItem = new LabeledDesignerItem(ModelItem.GetModelItem(item.StyleString))
                    {
                        Height = MathExtensions.RoundUp(item.ActualHeight*item.ItemScale, GridSize),
                        Width = MathExtensions.RoundUp(item.ActualWidth*item.ItemScale, GridSize),
                    };

                    newDesignerItem.Position = this.CalculateDroppedItemPosition(pos, item.DragPoint, item.ItemScale);

                    this.AddDesignerItem(newDesignerItem);
                    newDesignerItem.SelectItem();

                    if (!isFirstNode)
                    {
                        LabeledConnectionLine newLine = new LabeledConnectionLine();
                        newLine.Connect(connectionDecorator.ConnectedTo, newDesignerItem);

                        this.AddConnectionLine(newLine);
                    }

                    this.OnItemAdded(newDesignerItem.ModelItem);
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

            if (eventArgs != null)
            {
                SelectionCollection collection = eventArgs.Data as SelectionCollection;
                ToolboxItem item = collection.Select(t => t.Item).OfType<ToolboxItem>().FirstOrDefault();

                return item;
            }
            return null;
        }

        private Point CalculateDroppedItemPosition(Point dropPosition, Point? dragPoint, double itemScale)
        {
            double deltaVertical = 0, deltaHorizontal = 0;

            if (dragPoint.HasValue)
            {
                deltaVertical = dragPoint.Value.Y * itemScale;
                deltaHorizontal = dragPoint.Value.X * itemScale;
            }

            double x = Math.Max(0, dropPosition.X - deltaHorizontal);
            double y = Math.Max(0, dropPosition.Y - deltaVertical);

            x = MathExtensions.RoundDown(x, this.GridSize);
            y = MathExtensions.RoundDown(y, this.GridSize);

            return new Point(x, y);
        }
        #endregion

        public void AddDesignerItem(LabeledDesignerItem item)
        {
            item.Designer = this;
            item.InitializeLabels();
            item.InitializeBindings();

            item.ContentControlTemplate = this.CreateDesignerItemControlTemplate(item.ModelItem.Type);

            //Attaching DragThumbControl style to fit DesignerItem content
            item.DragControl.ThumbControl.Style = Application.Current.Resources[item.ModelItem.Type.ToString() + "_DragThumb"] as Style;

            this.Children.Add(item);
        }

        private Path CreateDesignerItemControlTemplate(ModelItemType type)
        {
            StringToPathGeometryConverter converter = new StringToPathGeometryConverter();

            Path path = new Path();
            string pathData = Application.Current.Resources[type.ToString() + "Data"] as string;
            path.Data = converter.Convert(pathData);
            path.IsHitTestVisible = false;
            path.Style = Application.Current.Resources[type.ToString()] as Style;

            return path;
        }

        public void AddConnectionLine(LabeledConnectionLine connection)
        {
            connection.ConnectionPropertyChanged += this.Line_ConnectionPropertyChanged;

            this.Children.Add(connection);
        }

        public void Line_ConnectionPropertyChanged(Connection connection, DependencyPropertyChangedEventArgs e)
        {
            if (ConnectionPropertyChanged != null)
            {
                ConnectionPropertyChanged(connection, e);
            }
        }

        public void OnItemDeletion(ModelItem item)
        {
            if (this.ModelItemsChanged != null)
            {
                this.ModelItemsChanged(item, ModelChange.Deleted);
            }
        }

        public void OnItemAdded(ModelItem item)
        {
            if (this.ModelItemsChanged != null)
            {
                this.ModelItemsChanged(item, ModelChange.Added);
            }
        }

        public void ModelResults_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    ModelItem affectedItem = item as ModelItem;

                    if (affectedItem != null)
                    {
                        this.HighlightPathToNode(affectedItem);
                    }
                }
            }
            else
            {
                this.ClearHighlights();

                if (e.NewItems != null)
                {
                    foreach (ModelItem item in e.NewItems)
                    {
                        this.HighlightPathToNode(item);
                    }
                }
            }
        }

        private void HighlightPathToNode(ModelItem item)
        {
            ModelItem currentItem = item;

            while (item.Parent != null)
            {
                item.ParentConnection.ConnectionPresenter.IsHighlighted = true;
                item = item.Parent;
            }
        }

        private void ClearHighlights()
        {
            IEnumerable<LabeledConnectionLine> linesToClear = this.Children.OfType<LabeledConnectionLine>().Where(line => line.IsHighlighted == true);

            foreach (LabeledConnectionLine line in linesToClear)
            {
                line.IsHighlighted = false;
            }
        }

        public DesignerItem GetDeisgnerItemById(Guid id)
        {
            return this.Children.OfType<DesignerItem>().FirstOrDefault(item => item.Id == id);
        }
    }
}
