using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DecisionTree.Controls
{
    public partial class RubberbandSelection : UserControl
    {
        private Point startPosition;
        private DesignerGrid designer;

        public int ZIndex
        {
            get
            {
                return Canvas.GetZIndex(this);
            }
            set
            {
                Canvas.SetZIndex(this, value);
            }
        }

        #region AllowRender Property
        public bool AllowRender
        {
            get { return (bool)GetValue(AllowRenderProperty); }
            set { SetValue(AllowRenderProperty, value); }
        }

        public static readonly DependencyProperty AllowRenderProperty =
            DependencyProperty.Register("AllowRender", typeof(bool), typeof(RubberbandSelection), new PropertyMetadata(false, new PropertyChangedCallback(OnAllowRenderChanged)));

        private static void OnAllowRenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RubberbandSelection rubberband = d as RubberbandSelection;
            bool newValue = (bool)e.NewValue;

            if (newValue)
            {
                rubberband.Visibility = Visibility.Visible;
            }
            else
            {
                rubberband.Visibility = Visibility.Collapsed;
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
            Rect rect = new Rect(this.Margin.Left, this.Margin.Top, this.Width, this.Height);

            foreach (DesignerItem item in designer.Designer.Children.OfType<DesignerItem>())
            {
                Point upperLeft = new Point(item.Margin.Left, item.Margin.Top);
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
            designer.Designer_MouseLeftButtonUp(sender, e);
        }

        public void Show(Point startPosition)
        {
            this.startPosition = startPosition;
            this.Update(startPosition);
            this.AllowRender = true;
        }

        public void Hide()
        {
            this.AllowRender = false;
        }
    }
}
