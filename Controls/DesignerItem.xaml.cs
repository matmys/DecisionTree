using DecisionTree.Extensions;
using DecisionTree.Model;

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace DecisionTree.Controls
{
    public partial class DesignerItem : UserControl
    {
        #region IsSelected
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(DesignerItem),
            new PropertyMetadata(new PropertyChangedCallback(OnSelectionChanged)));

        private static void OnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerItem control = d as DesignerItem; 
            bool isSelected = (bool)e.NewValue;
            if (isSelected)
            {
                control.SelectionDecorator.Visibility = Visibility.Visible;
            }
            else
            {
                control.SelectionDecorator.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region AdditionalContent
        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AdditionalContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register("AdditionalContent", typeof(object), typeof(DesignerItem), new PropertyMetadata(null));
        #endregion

        public Point AbsolutePosition { get; private set; }

        public Point InputPoint
        {
            get
            {
                return new Point(this.Margin.Left, this.Margin.Top + this.Height / 2);
            }
        }
        public Point OutputPoint
        {
            get
            {
                return new Point(this.Margin.Left + this.Width, this.Margin.Top + this.Height / 2);
            }
        }

        public LabeledConnectionLine ParentConnection { get; private set; }
        public List<LabeledConnectionLine> ChildrenConnections { get; private set; }

        public ModelItem ModelItem { get; private set; }

        public DesignerItem()
        {
            InitializeComponent();
            ChildrenConnections = new List<LabeledConnectionLine>();
            ModelItem = new ModelItem();
        }

        public DesignerItem(ModelItem modelItem)
        {
            InitializeComponent();
            ChildrenConnections = new List<LabeledConnectionLine>();
            ModelItem = modelItem;
        }

        public void SelectItem()
        {
            DesignerGrid designer = this.FindAncestor(typeof(DesignerGrid)) as DesignerGrid;

            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        this.IsSelected = !this.IsSelected;
                    }
                    else
                    {
                        this.SelectAllChildren();
                    }
                }
                else
                {
                    if (!this.IsSelected)
                    {
                        designer.DeselectAll();

                        this.IsSelected = true;
                        this.Focus();

                        this.FindAndUpdatePropertiesBox();
                    }
                }
            }
        }

        public void SelectAllChildren()
        {
            this.IsSelected = true;

            foreach (DesignerItem item in this.GetChildren())
            {
                item.IsSelected = true;
                item.SelectAllChildren();
            }
        }

        private void DesignerItem_MouseMove(object sender, MouseEventArgs e)
        {
            DesignerGrid designer = VisualTreeHelper.GetParent(this) as DesignerGrid;

            if (designer != null)
            {
                designer.rubberbandSelection.designer_MouseMove(sender, e);
            }
        }

        public void SetPosition(Point newPosition, int gridRounding = 1)
        {
            AbsolutePosition = newPosition;

            double X = MathExtensions.RoundDown(AbsolutePosition.X, gridRounding);
            double Y = MathExtensions.RoundDown(AbsolutePosition.Y, gridRounding);
            this.Margin = new Thickness(X, Y, 0, 0);
            this.UpdateConnectionsPositions();
        }

        public DesignerItem GetParent()
        {
            return ParentConnection.ParentItem;
        }

        public DesignerItem GetChild(int index = 0)
        {
            if (ChildrenConnections.Count > index)
            {
                return ChildrenConnections[index].ChildItem;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<DesignerItem> GetChildren()
        {
            return ChildrenConnections.Select(childConnection => childConnection.ChildItem);
        }

        public void SetParent(DesignerItem newParent, LabeledConnectionLine line)
        {
            ConnectionProperties connection = new ConnectionProperties();
            line.Connection = connection;
            line.IsProbabilityVisible = (newParent.ModelItem.Type == ModelItemType.Chance) ? true : false;
            line.InitializeBindings();
            newParent.ModelItem.AddConnection(connection);

            line.ChildItem = this;
            line.ParentItem = newParent;
            newParent.ChildrenConnections.Add(line);
            this.ParentConnection = line;
        }

        public void SetChild(DesignerItem newChild, LabeledConnectionLine line)
        {
            line.ChildItem = newChild;
            line.ParentItem = this;
            this.ChildrenConnections.Add(line);
            newChild.ParentConnection = line;
        }

        public void UpdateConnectionsPositions()
        {
            if (this.ParentConnection != null)
            {
                this.ParentConnection.EndPoint = this.InputPoint;
            }

            foreach (ConnectionLine line in this.ChildrenConnections)
            {
                line.StartPoint = this.OutputPoint;
            }
        }

        public void Delete()
        {
            DesignerGrid designerGrid = this.FindAncestor<DesignerGrid>();

            if (ParentConnection != null)
            {
                this.GetParent().ModelItem.Connections.Remove(ParentConnection.Connection);
                this.GetParent().ChildrenConnections.Remove(ParentConnection);
                
                designerGrid.Designer.Children.Remove(ParentConnection);

            }

            for (int i = 0; i < ChildrenConnections.Count; i++)
            {
                this.GetChild(i).ParentConnection = null;
                designerGrid.Designer.Children.Remove(ChildrenConnections[i]);
            }

            this.ParentConnection = null;
            this.ChildrenConnections.Clear();
            designerGrid.Designer.Children.Remove(this);
        }

        private void FindAndUpdatePropertiesBox()
        {
            Grid grid = this.FindAncestor<Expander>().FindAncestor<Grid>().FindChild<Grid>();
            Expander expander = VisualTreeHelper.GetChild(grid, 1) as Expander;
            PropertiesBox box = expander.Content as PropertiesBox;

            if (box != null)
            {
                box.Load(this);
            }
        }
    }
}
