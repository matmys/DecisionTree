using System.Windows.Controls.Primitives;
using DecisionTree.Extensions;
using DecisionTree.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System;

namespace DecisionTree.Controls
{
    public partial class DesignerItem : ISerializable
    {
        #region IsSelected Property
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(DesignerItem),
            new PropertyMetadata(OnSelectionChanged));

        private static void OnSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerItem control = d as DesignerItem; 
            bool isSelected = (bool)e.NewValue;
            if (control != null)
            {
                control.SelectionDecorator.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        #region ContentControlTemplate Property
        public object ContentControlTemplate
        {
            get { return GetValue(ContentControlTemplateProperty); }
            set { SetValue(ContentControlTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentControlTemplateProperty =
            DependencyProperty.Register("ContentControlTemplate", typeof(object), typeof(DesignerItem), new PropertyMetadata(null));
        #endregion

        public IEnumerable<LabeledConnectionLine> ChildrenConnections
        {
            get
            {
                return ModelItem.ChildrenConnections.Select(connection => connection.ConnectionPresenter);
            }
        }
        public LabeledConnectionLine ParentConnection
        {
            get
            {
                return ModelItem.ParentConnection.ConnectionPresenter;
            }
        }

        public IEnumerable<DesignerItem> Children
        {
            get
            {
                return this.ModelItem.Children.Select(child => child.ItemPresenter);
            }
        }

        public Point InputPoint
        {
            get
            {
                return new Point(this.Position.X, this.Position.Y + this.Height / 2);
            }
        }
        public Point OutputPoint
        {
            get
            {
                return new Point(this.Position.X + this.Width, this.Position.Y + this.Height / 2);
            }
        }

        public DesignerGrid Designer;
        public Guid Id { get; set; }
        public ModelItem ModelItem { get; private set; }
        public Point AbsolutePosition { get; private set; }

        public Point Position
        {
            get
            {
                return new Point(this.Margin.Left, this.Margin.Top);
            }

            set
            {
                this.SetPosition(value);
            }
        }

        private static List<string> propertiesToSerialize;
        public List<string> PropertiesToSerialize
        {
            get
            {
                return DesignerItem.propertiesToSerialize;
            }
        }

        static DesignerItem()
        {
            propertiesToSerialize = new List<string>(new string[]
            {
                "Id",
                "ItemLabel",
                "IsSelected",              
                "Position",
                "X",
                "Y",
                "ModelItem",
                "Type",
                "EndProbability",
                "EndPayout",
                "CalculatedPayout",
                "Height",
                "Width"
            });
        }

        public DesignerItem(ModelItem modelItem)
        {
            InitializeComponent();
            this.Id = Guid.NewGuid();
            this.ModelItem = modelItem;
            this.ModelItem.ItemPresenter = this;
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
                        IsSelected = !IsSelected;
                    }
                    else
                    {
                        SelectAllChildren();
                    }
                }
                else
                {
                    if (!IsSelected)
                    {
                        designer.DeselectAll();

                        IsSelected = true;
                        Focus();

                        FindAndUpdatePropertiesBox();
                    }
                }
            }
        }

        private void SelectAllChildren()
        {
            IsSelected = true;

            foreach (DesignerItem item in this.Children)
            {
                item.IsSelected = true;
                item.SelectAllChildren();
            }
        }

        protected virtual void SetPosition(Point newPosition)
        {
            AbsolutePosition = newPosition;
            this.Margin = new Thickness(AbsolutePosition.X, AbsolutePosition.Y, 0, 0);

            if (this.Designer != null)
            {
                this.SnapToGridLines();
            }
        }

        protected void SetPosition(double x, double y)
        {
            this.SetPosition(new Point(x, y));
        }

        public virtual void Delete()
        {
            this.Designer.OnItemDeletion(this.ModelItem);

            if (ModelItem.ParentConnection != null)
            {
                this.ParentConnection.Delete();
            }

            foreach (LabeledConnectionLine connection in this.ChildrenConnections.ToList())
            {
                connection.Delete();
            }

            this.Designer.Children.Remove(this);
        }

        public virtual void SnapToGridLines()
        {
            double x = MathExtensions.RoundDown(this.Position.X, this.Designer.GridSize);
            double y = MathExtensions.RoundDown(this.Position.Y, this.Designer.GridSize);

            this.Margin = new Thickness(x, y, 0, 0);

            this.UpdateConnectionsPositions();
        }

        private void UpdateConnectionsPositions()
        {
            if (ModelItem.ParentConnection != null)
            {
                ModelItem.ParentConnection.ConnectionPresenter.EndPoint = InputPoint;
            }

            foreach (ConnectionLine line in ModelItem.ChildrenConnections.Select(con => con.ConnectionPresenter))
            {
                line.StartPoint = OutputPoint;
            }
        }

        private void FindAndUpdatePropertiesBox()
        {
            Grid grid = this.FindAncestor<Expander>().FindAncestor<Grid>().FindChild<Grid>();
            Expander expander = VisualTreeHelper.GetChild(grid, 1) as Expander;

            if (expander != null)
            {
                PropertiesBox box = expander.Content as PropertiesBox;

                if (box != null)
                {
                    box.Load(this);
                }
            }
        }
    }
}
