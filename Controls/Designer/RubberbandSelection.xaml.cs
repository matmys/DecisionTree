using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DecisionTree.Controls
{
    public partial class RubberbandSelection
    {
        private Point startPosition;
        private DesignerGrid designer;

        private int ZIndex
        {
            set
            {
                Canvas.SetZIndex(this, value);
            }
        }

        public Point Position
        {
            get
            {
                return new Point(this.Margin.Left, this.Margin.Top);
            }

            set
            {
                this.Margin = new Thickness(value.X, value.Y, 0, 0);
            }
        }

        #region AllowRender Property
        public bool AllowRender
        {
            get { return (bool)GetValue(AllowRenderProperty); }
            set { SetValue(AllowRenderProperty, value); }
        }

        public static readonly DependencyProperty AllowRenderProperty =
            DependencyProperty.Register("AllowRender", typeof(bool), typeof(RubberbandSelection), new PropertyMetadata(false, OnAllowRenderChanged));

        private static void OnAllowRenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RubberbandSelection rubberband = d as RubberbandSelection;
            bool newValue = (bool)e.NewValue;

            if (rubberband != null)
            {
                rubberband.Visibility = newValue ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        public RubberbandSelection()
        {
            InitializeComponent();
            ZIndex = 100;
        }

        public void InitializeConnections(DesignerGrid designerGrid)
        {
            this.designer = designerGrid;
            this.designer.MouseMove += designer_MouseMove;
            this.designer.MouseLeftButtonUp += designer_MouseLeftButtonUp;
        }

        void designer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.AllowRender)
            {
                this.Finialize();
            }
        }

        public void designer_MouseMove(object sender, MouseEventArgs e)
        {
            if (designer.IsLeftMouseButtonClicked)
            {
                if (this.AllowRender)
                {
                    this.Update(e.GetPosition(designer.Designer));
                }
                else
                {
                    this.Show(e.GetPosition(designer.Designer));
                }
            }       
        }

        public void Update(Point currentPosition)
        {
            double x = Math.Min(startPosition.X, currentPosition.X);
            double y = Math.Min(startPosition.Y, currentPosition.Y);

            x = Math.Max(0, x);
            y = Math.Max(0, y);

            Point limitedPosition = new Point(Math.Max(0, currentPosition.X), Math.Max(0, currentPosition.Y));

            this.Margin = new Thickness(x, y, 0, 0);
            this.Width = Math.Abs(startPosition.X - limitedPosition.X);
            this.Height = Math.Abs(startPosition.Y - limitedPosition.Y);
        }

        public void Finialize()
        {
            Rect rect = new Rect(this.Position.X, this.Position.Y, this.Width, this.Height);

            foreach (DesignerItem item in designer.Designer.Children.OfType<DesignerItem>())
            {
                Point upperLeft = new Point(item.Position.X, item.Position.Y);
                Point bottomRight = new Point(upperLeft.X + item.Width, upperLeft.Y + item.Height);

                if (rect.Contains(upperLeft) && rect.Contains(bottomRight))
                {
                    item.IsSelected = true;
                }
            }

            this.Hide();
        }

        private void RubberbandSelection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            designer.DesignerGrid_MouseLeftButtonUp(sender, e);
        }

        private void Show(Point newStartPosition)
        {
            this.startPosition = newStartPosition;
            this.Update(newStartPosition);
            this.AllowRender = true;
        }

        private void Hide()
        {
            this.AllowRender = false;
        }
    }
}
