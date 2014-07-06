using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public partial class ConnectionLine : UserControl
    {
        protected Point P1 = new Point(0, 0);
        protected Point P2 = new Point(0, 0);
        protected Point P3 = new Point(0, 0);
        protected Point P4 = new Point(0, 0);

        public DesignerItem ParentItem { get; set; }
        public DesignerItem ChildItem { get; set; }

        #region Stroke Property
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(ConnectionLine), new PropertyMetadata(null));
        #endregion

        #region StrokeThickness Property
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(ConnectionLine), new PropertyMetadata((double)1));
        #endregion

        #region StrokeDashArray Property
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(ConnectionLine), new PropertyMetadata(new DoubleCollection()));
        #endregion

        #region StartPoint Property
        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(ConnectionLine), new PropertyMetadata(new Point(0,0), new PropertyChangedCallback(OnStartPointChanged)));

        private static void OnStartPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Point newPoint = (Point)e.NewValue;
            ConnectionLine line = d as ConnectionLine;

            line.P1 = new Point(newPoint.X + line.Margin.Left, newPoint.Y);

            line.P2.X = newPoint.X + line.FirstTransition;
            line.P3.X = newPoint.X + line.FirstTransition + line.SecondTransition;
            line.P2.Y = newPoint.Y;

            line.updatePointCollection();
        }
        #endregion

        #region EndPoint Property
        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(ConnectionLine), new PropertyMetadata(new Point(0, 0), new PropertyChangedCallback(OnEndPointChanged)));

        private static void OnEndPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Point newPoint = (Point)e.NewValue;
            ConnectionLine line = d as ConnectionLine;

            line.P4 = new Point(newPoint.X - line.Margin.Right, newPoint.Y);
            line.P3.Y = newPoint.Y;

            line.updatePointCollection();
            line.PathMargin = new Thickness(line.P4.X, line.P4.Y, 0, 0);
        }
        #endregion

        #region Margin Property
        new public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        new public static readonly DependencyProperty MarginProperty =
            DependencyProperty.Register("Margin", typeof(Thickness), typeof(ConnectionLine), new PropertyMetadata(new Thickness(0), new PropertyChangedCallback(OnMarginChanged)));

        private static void OnMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionLine line = d as ConnectionLine;
            Thickness oldMargin = (Thickness)e.OldValue;
            Thickness newMargin = (Thickness)e.NewValue;

            double leftDelta = newMargin.Left - oldMargin.Left;
            double rightDelta = newMargin.Right - oldMargin.Right;

            line.P1.X += leftDelta;
            line.P4.X -= rightDelta;

            line.updatePointCollection();
            line.PathMargin = new Thickness(line.P4.X, line.P4.Y, 0, 0);
        }
        #endregion

        #region Points Property
        private PointCollection Points
        {
            get { return (PointCollection)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        private static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(PointCollection), typeof(ConnectionLine), new PropertyMetadata(new PointCollection()));
        #endregion

        #region FirstTransition Property
        public double FirstTransition
        {
            get { return (double)GetValue(FirstTransitionProperty); }
            set { SetValue(FirstTransitionProperty, value); }
        }

        public static readonly DependencyProperty FirstTransitionProperty =
            DependencyProperty.Register("FirstTransition", typeof(double), typeof(ConnectionLine), new PropertyMetadata((double)20, new PropertyChangedCallback(OnFirstTransitionChanged)));

        private static void OnFirstTransitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newTransition = (double)e.NewValue;
            ConnectionLine line = d as ConnectionLine;

            line.P2.X = line.StartPoint.X + newTransition;
            line.P3.X = line.StartPoint.X + newTransition + line.SecondTransition;
            line.updatePointCollection();
        }
        #endregion

        #region SecondTransition Property
        public double SecondTransition
        {
            get { return (double)GetValue(SecondTransitionProperty); }
            set { SetValue(SecondTransitionProperty, value); }
        }

        public static readonly DependencyProperty SecondTransitionProperty =
            DependencyProperty.Register("SecondTransition", typeof(double), typeof(ConnectionLine), new PropertyMetadata((double)10, OnSecondTransitionChanged));

        private static void OnSecondTransitionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newTransition = (double)e.NewValue;
            ConnectionLine line = d as ConnectionLine;

            line.P3.X = line.StartPoint.X + line.FirstTransition + newTransition;
            line.updatePointCollection();

        }
        #endregion

        #region AllowRender Property
        public bool AllowRender
        {
            get { return (bool)GetValue(AllowRenderProperty); }
            set { SetValue(AllowRenderProperty, value); }
        }

        public static readonly DependencyProperty AllowRenderProperty =
            DependencyProperty.Register("AllowRender", typeof(bool), typeof(ConnectionLine), new PropertyMetadata(true, new PropertyChangedCallback(OnAllowRenderChanged)));

        private static void OnAllowRenderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionLine line = d as ConnectionLine;
            bool allowRender = (bool)e.NewValue;

            if (allowRender)
            {
                line.Show();
            }
            else
            {
                line.Hide();
            }
        }
        #endregion

        #region PathMargin Property
        private Thickness PathMargin
        {
            get { return (Thickness)GetValue(PathMarginProperty); }
            set { SetValue(PathMarginProperty, value); }
        }

        public static readonly DependencyProperty PathMarginProperty =
            DependencyProperty.Register("PathMargin", typeof(Thickness), typeof(ConnectionLine), new PropertyMetadata(new Thickness(0,0,0,0)));
        #endregion

        private void updatePointCollection()
        {
            PointCollection newPointCollection = new PointCollection() { P1, P2, P3, P4 };
            Points = newPointCollection;
        }

        public ConnectionLine()
        {
            InitializeComponent();
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }
    }
}
