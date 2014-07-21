using DecisionTree.Extensions;
using DecisionTree.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public class LabeledConnectionLine : ConnectionLine, ISerializable
    {
        public delegate void ConnectionPropertyChangedHandler(Connection connection, DependencyPropertyChangedEventArgs e);
        public event ConnectionPropertyChangedHandler ConnectionPropertyChanged;

        protected TextBlock Label;
        protected TextBlock Payout;
        protected TextBlock Probability;

        public Connection Connection { get; set; }

        #region LabelOffset Property
        public Point LabelOffset
        {
            get { return (Point)GetValue(LabelOffsetProperty); }
            set { SetValue(LabelOffsetProperty, value); }
        }
        

        public static readonly DependencyProperty LabelOffsetProperty =
            DependencyProperty.Register("LabelOffset", typeof(Point), typeof(LabeledConnectionLine), new PropertyMetadata(new Point(0,0), LabelOffsetPropertyChanged));

        private static void LabelOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;

            if (line != null)
            {
                line.UpdateLabelPosition();
            }
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

            if (line != null)
            {
                line.UpdatePayoutPosition();
            }
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

            if (line != null)
            {
                line.UpdateProbabilityPosition();
            }
        }

        #endregion

        #region LabelForeground Property
        public Brush LabelForeground
        {
            get { return (Brush)GetValue(LabelForegroundProperty); }
            set { SetValue(LabelForegroundProperty, value); }
        }

        public static readonly DependencyProperty LabelForegroundProperty =
            DependencyProperty.Register("LabelForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnLabelForegroundProperyChanged));

        private static void OnLabelForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            Brush newBrush = e.NewValue as Brush;
            if (line != null)
            {
                line.Label.Foreground = newBrush;
            }
        }
        #endregion

        #region PositivePayoutForeground Property
        public Brush PositivePayoutForeground
        {
            get { return (Brush)GetValue(PositivePayoutForegroundProperty); }
            set { SetValue(PositivePayoutForegroundProperty, value); }
        }

        public static readonly DependencyProperty PositivePayoutForegroundProperty =
            DependencyProperty.Register("PositivePayoutForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnPositivePayoutForegroundProperyChanged));

        private static void OnPositivePayoutForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            if (line != null)
            {
                line.UpdatePayoutForeground();
            }
        }

        #endregion

        #region NegativePayoutForeground Property
        public Brush NegativePayoutForeground
        {
            get { return (Brush)GetValue(NegativePayoutForegroundProperty); }
            set { SetValue(NegativePayoutForegroundProperty, value); }
        }

        public static readonly DependencyProperty NegativePayoutForegroundProperty =
            DependencyProperty.Register("NegativePayoutForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnNegativePayoutForegroundProperyChanged));

        private static void OnNegativePayoutForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            if (line != null)
            {
                line.UpdatePayoutForeground();
            }
        }

        #endregion

        #region ProbabilityForeground Property
        public Brush ProbabilityForeground
        {
            get { return (Brush)GetValue(ProbabilityForegroundProperty); }
            set { SetValue(ProbabilityForegroundProperty, value); }
        }

        public static readonly DependencyProperty ProbabilityForegroundProperty =
            DependencyProperty.Register("ProbabilityForeground", typeof(Brush), typeof(LabeledConnectionLine), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnProbabilityForegroundProperyChanged));

        private static void OnProbabilityForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            Brush newBrush = e.NewValue as Brush;
            if (line != null)
            {
                line.Probability.Foreground = newBrush;
            }
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

            if (line != null)
            {
                line.Probability.Visibility = (newValue) ? Visibility.Visible : Visibility.Collapsed;
            }
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
            DependencyProperty.Register("IsFocused", typeof(bool), typeof(LabeledConnectionLine), new PropertyMetadata(false, OnIsFocusedPropertyChanged));

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            if (line != null)
            {
                line.UpdateStroke();
            }
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

        #region HighlightedStrokeThickness Property
        public double HighlightedStrokeThickness
        {
            get { return (double)GetValue(HighlightedStrokeThicknessProperty); }
            set { SetValue(HighlightedStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty HighlightedStrokeThicknessProperty =
            DependencyProperty.Register("HighlightedStrokeThickness", typeof(double), typeof(LabeledConnectionLine), new PropertyMetadata((double)1, OnHighlightedStrokeThicknessPropertyChanged));

        private static void OnHighlightedStrokeThicknessPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;

            if (line != null)
            {
                line.UpdateStrokeThickness();
            }
        }
        #endregion

        #region IsHighlighted Property
        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(LabeledConnectionLine), new PropertyMetadata(false, OnIsHighlightedPropertyChanged));

        private static void OnIsHighlightedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledConnectionLine line = d as LabeledConnectionLine;
            if (line != null)
            {
                line.UpdateStrokeThickness();
            }
        }
        #endregion

        public Size OptimumSize
        {
            get
            {
                double width = this.FirstTransition + this.SecondTransition;
                width += Math.Max(this.Payout.ActualWidth, this.Label.ActualWidth) + 30 + this.Probability.ActualWidth;
                width = MathExtensions.RoundUp(width, this.FindAncestor<DesignerGrid>().GridSize);

                return new Size(width, 0);
            }
        }

        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }

        private static List<string> propertiesToSerialize;
        public List<string> PropertiesToSerialize
        {
            get
            {
                return LabeledConnectionLine.propertiesToSerialize;
            }
        }

        static LabeledConnectionLine()
        {
            propertiesToSerialize = new List<string>(new string[]
            {             
                "Connection",
                "Label",
                "Payout",
                "Probability",
                "ParentId",
                "ChildId",
                "IsHighlighted"
            });
        }

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

            BindingHelper.RegisterForNotification<LabeledConnectionLine>("StartPoint", this, OnPointChanged);
            BindingHelper.RegisterForNotification<LabeledConnectionLine>("EndPoint", this, OnPointChanged);        
        }

        public void InitializeBindings()
        {
            if (Connection != null)
            {
                BindingHelper.RegisterBinding("Label", Connection, Label, TextBlock.TextProperty);
                BindingHelper.RegisterBinding("Payout", Connection, Payout, TextBlock.TextProperty, BindingMode.OneWay, "" + PayoutSign + " {0:#,0.##;(#,0.##)}");
                BindingHelper.RegisterBinding("Probability", Connection, Probability, TextBlock.TextProperty, BindingMode.OneWay, "{0:N2} %");

                BindingHelper.RegisterForNotification<LabeledConnectionLine>("Label", Connection, OnLabelChanged);
                BindingHelper.RegisterForNotification<LabeledConnectionLine>("Payout", Connection, OnPayoutChanged);
                BindingHelper.RegisterForNotification<LabeledConnectionLine>("Probability", Connection, OnProbabilityChanged);

                Connection.ConnectionPropertyChanged += Connection_ConnectionPropertyChanged;
            }
        }

        public override void Connect(DesignerItem parent, DesignerItem child)
        {
            base.Connect(parent, child);

            if (Connection == null)
            {
                this.Connection = new Connection();

                if (parent.ModelItem.Type == ModelItemType.Chance)
                {
                    double value = parent.ModelItem.ChildrenConnections.Sum(conn => conn.Probability);

                    if (value <= 100)
                    {
                        this.Connection.Probability = 100 - value;
                    }
                }
            }

            this.Connection.ConnectionPresenter = this;
            this.Connection.ParentItem = parent.ModelItem;
            this.Connection.ChildItem = child.ModelItem;
            
            this.IsProbabilityVisible = (parent.ModelItem.Type == ModelItemType.Chance);
            this.ParentId = parent.Id;
            this.ChildId = child.Id;

            parent.ModelItem.ChildrenConnections.Add(this.Connection);
            child.ModelItem.SetParentConnection(this.Connection);

            this.InitializeBindings();
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

        private void Connection_ConnectionPropertyChanged(Connection connection, DependencyPropertyChangedEventArgs e)
        {
            if (this.ConnectionPropertyChanged != null)
            {
                this.ConnectionPropertyChanged(connection, e);
            }
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

        private void UpdateStrokeThickness()
        {
            this.StrokeThickness = (this.IsHighlighted) ? this.HighlightedStrokeThickness : 1 ;
        }

        public void Delete()
        {
            DesignerGrid designer = this.FindAncestor<DesignerGrid>();

            if (designer != null)
            {
                if (Connection != null)
                {
                    this.Connection.ConnectionPropertyChanged -= this.Connection_ConnectionPropertyChanged;
                }

                this.Connection.ParentItem.ChildrenConnections.Remove(this.Connection);
                this.Connection.ChildItem.SetParentConnection(null);

                this.Connection = null;

                designer.Children.Remove(this);
                this.ConnectionPropertyChanged -= designer.Line_ConnectionPropertyChanged;
            }
        }
    }
}
