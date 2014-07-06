using DecisionTree.Model;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public class LabeledConnectionLine : ConnectionLine
    {
        protected TextBlock Label;
        protected TextBlock Payout;
        protected TextBlock Probability;

        public ConnectionProperties Connection { get; set; }

        #region LabelOffset Property
        public Point LabelOffset
        {
            get { return (Point)GetValue(LabelOffsetProperty); }
            set { SetValue(LabelOffsetProperty, value); }
        }
        

        public static readonly DependencyProperty LabelOffsetProperty =
            DependencyProperty.Register("LabelOffset", typeof(Point), typeof(LabeledConnectionLine), new PropertyMetadata(new Point(0,0), new PropertyChangedCallback(LabelOffsetPropertyChanged)));

        private static void LabelOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;

            line.UpdateLabelPosition();
        }
        #endregion

        #region PayoutOffset Property
        public Point PayoutOffset
        {
            get { return (Point)GetValue(PayoutOffsetProperty); }
            set { SetValue(PayoutOffsetProperty, value); }
        }

        public static readonly DependencyProperty PayoutOffsetProperty =
            DependencyProperty.Register("PayoutOffset", typeof(Point), typeof(LabeledConnectionLine), new PropertyMetadata(new Point(0, 0), PayoutOffsetPropertyChanged));

        private static void PayoutOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;

            line.UpdatePayoutPosition();
        }
        #endregion

        #region ProbabilityOffset Property
        public Point ProbabilityOffset
        {
            get { return (Point)GetValue(ProbabilityOffsetProperty); }
            set { SetValue(ProbabilityOffsetProperty, value); }
        }

        public static readonly DependencyProperty ProbabilityOffsetProperty =
            DependencyProperty.Register("ProbabilityOffset", typeof(Point), typeof(LabeledConnectionLine), new PropertyMetadata(new Point(0, 0), ProbabilityOffsetPropertyChanged));

        private static void ProbabilityOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;

            line.UpdateProbabilityPosition();
        }
        #endregion

        #region LabelForeground Property
        public Brush LabelForeground
        {
            get { return (Brush)GetValue(LabelForegroundProperty); }
            set { SetValue(LabelForegroundProperty, value); }
        }

        public static readonly DependencyProperty LabelForegroundProperty =
            DependencyProperty.Register("LabelForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(OnLabelForegroundProperyChanged)));

        private static void OnLabelForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            Brush newBrush = e.NewValue as Brush;
            line.Label.Foreground = newBrush;
        }
        #endregion

        #region PositivePayoutForeground Property
        public Brush PositivePayoutForeground
        {
            get { return (Brush)GetValue(PositivePayoutForegroundProperty); }
            set { SetValue(PositivePayoutForegroundProperty, value); }
        }

        public static readonly DependencyProperty PositivePayoutForegroundProperty =
            DependencyProperty.Register("PositivePayoutForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(OnPositivePayoutForegroundProperyChanged)));

        private static void OnPositivePayoutForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            line.UpdatePayoutForeground();
        }
        #endregion

        #region NegativePayoutForeground Property
        public Brush NegativePayoutForeground
        {
            get { return (Brush)GetValue(NegativePayoutForegroundProperty); }
            set { SetValue(NegativePayoutForegroundProperty, value); }
        }

        public static readonly DependencyProperty NegativePayoutForegroundProperty =
            DependencyProperty.Register("NegativePayoutForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(OnNegativePayoutForegroundProperyChanged)));

        private static void OnNegativePayoutForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            line.UpdatePayoutForeground();
        }
        #endregion

        #region ProbabilityForeground Property
        public Brush ProbabilityForeground
        {
            get { return (Brush)GetValue(ProbabilityForegroundProperty); }
            set { SetValue(ProbabilityForegroundProperty, value); }
        }

        public static readonly DependencyProperty ProbabilityForegroundProperty =
            DependencyProperty.Register("ProbabilityForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(OnProbabilityForegroundProperyChanged)));

        private static void OnProbabilityForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            Brush newBrush = e.NewValue as Brush;
            line.Probability.Foreground = newBrush;
        }
        #endregion

        #region IsProbabilityVisible Property
        public bool IsProbabilityVisible
        {
            get { return (bool)GetValue(IsProbabilityVisibleProperty); }
            set { SetValue(IsProbabilityVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsProbabilityVisibleProperty =
            DependencyProperty.Register("IsProbabilityVisible", typeof(bool), typeof(LabeledConnectionLine), new PropertyMetadata(false, OnIsProbabilityVisibleChanged));

        private static void OnIsProbabilityVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            bool newValue = (bool)e.NewValue;

            line.Probability.Visibility = (newValue) ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region PayoutSign Property
        public char PayoutSign
        {
            get { return (char)GetValue(PayoutSignProperty); }
            set { SetValue(PayoutSignProperty, value); }
        }

        public static readonly DependencyProperty PayoutSignProperty =
            DependencyProperty.Register("PayoutSign", typeof(char), typeof(LabeledConnectionLine), new PropertyMetadata('$'));
        #endregion

        #region IsFocused Property
        public bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.Register("IsFocused", typeof(bool), typeof(LabeledConnectionLine), new PropertyMetadata(false, new PropertyChangedCallback(OnIsFocusedPropertyChanged)));

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            line.UpdateStroke();
        }
        #endregion

        #region UnfocusedStroke Property
        public Brush UnfocusedStroke
        {
            get { return (Brush)GetValue(UnfocusedStrokeProperty); }
            set { SetValue(UnfocusedStrokeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UnfocusedStroke.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnfocusedStrokeProperty =
            DependencyProperty.Register("UnfocusedStroke", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnIsFocusedPropertyChanged));
        #endregion

        #region FocusedStroke Property
        public Brush FocusedStroke
        {
            get { return (Brush)GetValue(FocusedStrokeProperty); }
            set { SetValue(FocusedStrokeProperty, value); }
        }

        public static readonly DependencyProperty FocusedStrokeProperty =
            DependencyProperty.Register("FocusedStroke", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnIsFocusedPropertyChanged));
        #endregion

        public LabeledConnectionLine()
        {
            InitializeLabels();
        }

        public void InitializeLabels()
        {
            Label = new TextBlock();
            Payout = new TextBlock();
            Probability = new TextBlock();

            Probability.Visibility = Visibility.Collapsed;

            this.LayoutRoot.Children.Add(Label);
            this.LayoutRoot.Children.Add(Payout);
            this.LayoutRoot.Children.Add(Probability);

            this.RegisterForNotification("StartPoint", this, OnPointChanged);
            this.RegisterForNotification("EndPoint", this, OnPointChanged);
            
        }

        public void InitializeBindings()
        {
            if (Connection != null)
            {
                Binding labelBinding = new Binding("Label");
                Binding payoutBinding = new Binding("Payout");
                Binding probabilityBinding = new Binding("Probability");

                probabilityBinding.StringFormat = "{0:N2} %";
                payoutBinding.StringFormat = "" + PayoutSign + " {0:#,0.##;(#,0.##)}";

                labelBinding.Source = Connection;
                payoutBinding.Source = Connection;
                probabilityBinding.Source = Connection;

                Label.SetBinding(TextBlock.TextProperty, labelBinding);
                Payout.SetBinding(TextBlock.TextProperty, payoutBinding);
                Probability.SetBinding(TextBlock.TextProperty, probabilityBinding);

                this.RegisterForNotification("Label", Connection, OnLabelChanged);
                this.RegisterForNotification("Payout", Connection, OnPayoutChanged);
                this.RegisterForNotification("Probability", Connection, OnProbabilityChanged);
            }
        }

        private void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == Connection)
            {
                UpdateLabelPosition();
            }
        }

        private void OnPayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == Connection)
            {
                UpdatePayoutPosition();
                UpdatePayoutForeground();
            }
        }

        private void OnProbabilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == Connection)
            {
                UpdateProbabilityPosition();
            }
        }

        private void OnPointChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d == this)
            {
                UpdateLabelPosition();
                UpdatePayoutPosition();
                UpdateProbabilityPosition();
            }
        }

        public void RegisterForNotification(string propertyName, DependencyObject element, PropertyChangedCallback callback)
        {
            //Bind to a depedency property
            Binding b = new Binding(propertyName) { Source = element };
            var prop = System.Windows.DependencyProperty.RegisterAttached(
                "ListenAttached" + propertyName,
                typeof(object),
                typeof(ConnectionLine),
                new System.Windows.PropertyMetadata(callback));

            BindingOperations.SetBinding(element, prop, b);
        }

        private void UpdateLabelPosition()
        {
            this.Label.Margin = new Thickness(this.P3.X + this.LabelOffset.X, this.P3.Y - this.Label.ActualHeight + this.LabelOffset.Y, 0, 0);
        }

        private void UpdatePayoutPosition()
        {
            this.Payout.Margin = new Thickness(this.P3.X + this.PayoutOffset.X, this.P3.Y + this.PayoutOffset.Y, 0, 0);
        }

        private void UpdateProbabilityPosition()
        {
            this.Probability.Margin = new Thickness(this.P4.X - this.Probability.ActualWidth + this.ProbabilityOffset.X, this.P4.Y + this.ProbabilityOffset.Y, 0, 0);
        }

        private void UpdatePayoutForeground()
        {
            if (Connection != null && Connection.Payout >= 0)
            {
                Payout.Foreground = this.PositivePayoutForeground;
            }
            else
            {
                Payout.Foreground = this.NegativePayoutForeground;
            }
        }

        private void UpdateStroke()
        {
            this.Stroke = (this.IsFocused) ? this.FocusedStroke : this.UnfocusedStroke;
        }
    }
}
