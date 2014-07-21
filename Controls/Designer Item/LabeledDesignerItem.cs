using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DecisionTree.Model;
using System.Windows.Data;
using DecisionTree.Extensions;
using System.Windows.Media;
using System;
using System.Linq;

namespace DecisionTree.Controls
{
    public class LabeledDesignerItem : DesignerItem
    {
        private TextBlock LabelTextBlock { get; set; }
        private TextBlock CalculatedPayoutTextBlock { get; set; }
        private TextBlock EndPayoutTextBlock { get; set; }
        private TextBlock EndProbabilityTextBlock { get; set; }
        
        #region LabelOffset Property
        public Point LabelOffset
        {
            get { return (Point)GetValue(LabelOffsetProperty); }
            set { SetValue(LabelOffsetProperty, value); }
        }

        public static readonly DependencyProperty LabelOffsetProperty =
            DependencyProperty.Register("LabelOffset", typeof(Point), typeof(LabeledDesignerItem), new PropertyMetadata(new Point(0, 0), OnLabelOffsetPropertyChanged));

        private static void OnLabelOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            if (item != null)
            {
                item.UpdateLabelPosition();
            }
        }
        #endregion

        #region CalculatedPayoutOffset Property
        public Point CalculatedPayoutOffset
        {
            get { return (Point)GetValue(CalculatedPayoutOffsetProperty); }
            set { SetValue(CalculatedPayoutOffsetProperty, value); }
        }

        public static readonly DependencyProperty CalculatedPayoutOffsetProperty =
            DependencyProperty.Register("CalculatedPayoutOffset", typeof(Point), typeof(LabeledDesignerItem), new PropertyMetadata(new Point(0, 0), OnCalculatedPayoutOffsetPropertyChanged));

        private static void OnCalculatedPayoutOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            if (item != null)
            {
                item.UpdateCalculatedPayoutPosition();
            }
        }
        #endregion

        #region EndPayoutOffset Property
        public Point EndPayoutOffset
        {
            get { return (Point)GetValue(EndPayoutOffsetProperty); }
            set { SetValue(EndPayoutOffsetProperty, value); }
        }

        public static readonly DependencyProperty EndPayoutOffsetProperty =
            DependencyProperty.Register("EndPayoutOffset", typeof(Point), typeof(LabeledDesignerItem), new PropertyMetadata(new Point(0, 0), OnEndPayoutOffsetPropertyChanged));

        private static void OnEndPayoutOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            if (item != null)
            {
                item.UpdateEndPayoutPosition();
            }
        }
        #endregion

        #region EndProbabilityOffset Property
        public Point EndProbabilityOffset
        {
            get { return (Point)GetValue(EndProbabilityOffsetProperty); }
            set { SetValue(EndProbabilityOffsetProperty, value); }
        }

        public static readonly DependencyProperty EndProbabilityOffsetProperty =
            DependencyProperty.Register("EndProbabilityOffset", typeof(Point), typeof(LabeledDesignerItem), new PropertyMetadata(new Point(0, 0), OnEndProbabilityOffsetPropertyChanged));

        private static void OnEndProbabilityOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            if (item != null)
            {
                item.UpdateEndProbabilityPosition();
            }
        }
        #endregion

        #region CalculatedPayoutForeground Property
        public Brush CalculatedPayoutForeground
        {
            get { return (Brush)GetValue(CalculatedPayoutForegroundProperty); }
            set { SetValue(CalculatedPayoutForegroundProperty, value); }
        }

        public static readonly DependencyProperty CalculatedPayoutForegroundProperty =
            DependencyProperty.Register("CalculatedPayoutForeground", typeof(Brush), typeof(LabeledDesignerItem), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnCalculatedPayoutForegroundProperyChanged));

        private static void OnCalculatedPayoutForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            Brush newBrush = e.NewValue as Brush;
            if (item != null && item.ModelItem.Type != ModelItemType.End)
            {
                item.CalculatedPayoutTextBlock.Foreground = newBrush;
            }
        }
        #endregion

        #region CalculatedPayoutVisibility Property
        public Visibility CalculatedPayoutVisibility
        {
            get { return (Visibility)GetValue(CalculatedPayoutVisibilityProperty); }
            set { SetValue(CalculatedPayoutVisibilityProperty, value); }
        }

        public static readonly DependencyProperty CalculatedPayoutVisibilityProperty =
            DependencyProperty.Register("CalculatedPayoutVisibility", typeof(Visibility), typeof(LabeledDesignerItem), new PropertyMetadata(Visibility.Collapsed));
        #endregion

        #region PayoutSign Property
        public char PayoutSign
        {
            get { return (char)GetValue(PayoutSignProperty); }
            set { SetValue(PayoutSignProperty, value); }
        }

        public static readonly DependencyProperty PayoutSignProperty =
            DependencyProperty.Register("PayoutSign", typeof(char), typeof(LabeledDesignerItem), new PropertyMetadata('$'));
        #endregion

        #region PositivePayoutForeground Property
        public Brush PositivePayoutForeground
        {
            get { return (Brush)GetValue(PositivePayoutForegroundProperty); }
            set { SetValue(PositivePayoutForegroundProperty, value); }
        }

        public static readonly DependencyProperty PositivePayoutForegroundProperty =
            DependencyProperty.Register("PositivePayoutForeground", typeof(Brush), typeof(LabeledDesignerItem), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnPositivePayoutForegroundProperyChanged));

        private static void OnPositivePayoutForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            if (item != null)
            {
                item.UpdateEndPayoutForeground();
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
            DependencyProperty.Register("NegativePayoutForeground", typeof(Brush), typeof(LabeledDesignerItem), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnNegativePayoutForegroundProperyChanged));

        private static void OnNegativePayoutForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            if (item != null)
            {
                item.UpdateEndPayoutForeground();
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
            DependencyProperty.Register("ProbabilityForeground", typeof(Brush), typeof(LabeledDesignerItem), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnProbabilityForegroundProperyChanged));

        private static void OnProbabilityForegroundProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LabeledDesignerItem item = d as LabeledDesignerItem;
            Brush newBrush = e.NewValue as Brush;
            if (item != null && item.ModelItem.Type == ModelItemType.End)
            {
                item.EndProbabilityTextBlock.Foreground = newBrush;
            }
        }
        #endregion

        public new IEnumerable<LabeledDesignerItem> Children
        {
            get
            {
                return this.ModelItem.Children.Select(child => (child.ItemPresenter as LabeledDesignerItem));
            }
        }

        private double TextBlockDesiredHeigth
        {
            get
            {
                TextBlock testBlock = new TextBlock();
                testBlock.Text = "Test String";
                testBlock.FontSize = this.FontSize;
                return testBlock.ActualHeight;
            }
        }

        public Size OptimumSize
        {
            get
            {
                double textWidth = Math.Max(this.LabelTextBlock.ActualWidth + Math.Abs(this.LabelOffset.X), this.Width);
                double textHeight = this.TextBlockDesiredHeigth - this.LabelOffset.Y + this.Height;

                if (CalculatedPayoutTextBlock != null)
                {
                    textWidth = Math.Max(textWidth, this.CalculatedPayoutTextBlock.ActualWidth + Math.Abs(CalculatedPayoutOffset.X));
                    textHeight += this.TextBlockDesiredHeigth + this.CalculatedPayoutOffset.Y;
                }

                if (EndPayoutTextBlock != null && EndProbabilityTextBlock != null)
                {
                    double width = Math.Max(this.EndProbabilityTextBlock.ActualWidth + this.EndProbabilityOffset.X, this.EndPayoutTextBlock.ActualWidth + this.EndPayoutOffset.X);
                    width += this.Width;

                    textWidth = Math.Max(textWidth, width);
                }

                textWidth = MathExtensions.RoundUp(textWidth, this.Designer.GridSize);
                textHeight = MathExtensions.RoundUp(textHeight, this.Designer.GridSize);

                return new Size(textWidth, textHeight);
            }
        }

        public Size FamilySize { get; private set; }

        public LabeledDesignerItem(ModelItem modelItem) : base(modelItem)
        {
            
        }

        public void InitializeLabels()
        {
            this.LabelTextBlock = this.InitializeTextBlock();
            this.Designer.Children.Add(this.LabelTextBlock);

            if (((ModelItemType.Chance | ModelItemType.Decision) & this.ModelItem.Type) != ModelItemType.None)
            {
                this.CalculatedPayoutTextBlock = this.InitializeTextBlock();
                this.Designer.Children.Add(this.CalculatedPayoutTextBlock);
            }

            if (this.ModelItem.Type == ModelItemType.End)
            {
                this.EndPayoutTextBlock = this.InitializeTextBlock();
                this.EndProbabilityTextBlock = this.InitializeTextBlock();
                this.Designer.Children.Add(this.EndPayoutTextBlock);
                this.Designer.Children.Add(this.EndProbabilityTextBlock);
            }
        }

        public void InitializeBindings()
        {
            BindingHelper.RegisterBinding("ItemLabel", ModelItem, LabelTextBlock, TextBlock.TextProperty);

            BindingHelper.RegisterForNotification<LabeledDesignerItem>("Text", this.LabelTextBlock, OnLabelTextPropertyChanged);

            if (((ModelItemType.Chance | ModelItemType.Decision) & this.ModelItem.Type) != ModelItemType.None)
            {
                BindingHelper.RegisterBinding("CalculatedPayout", ModelItem, CalculatedPayoutTextBlock, TextBlock.TextProperty, "" + this.PayoutSign + " {0:#,0.##;(#,0.##)}");
                BindingHelper.RegisterBinding("CalculatedPayoutVisibility", this, CalculatedPayoutTextBlock, TextBlock.VisibilityProperty);

                BindingHelper.RegisterForNotification<LabeledDesignerItem>("Text", this.CalculatedPayoutTextBlock, OnCalculatedPayoutTextPropertyChanged);
            }

            if (this.ModelItem.Type == ModelItemType.End)
            {
                BindingHelper.RegisterBinding("EndProbability", ModelItem, EndProbabilityTextBlock, TextBlock.TextProperty, "{0:N2} %");
                BindingHelper.RegisterBinding("EndPayout", ModelItem, EndPayoutTextBlock, TextBlock.TextProperty, "" + this.PayoutSign + " {0:#,0.##;(#,0.##)}");

                BindingHelper.RegisterForNotification<LabeledDesignerItem>("Text", this.EndProbabilityTextBlock, OnEndProbabilityTextPropertyChanged);
                BindingHelper.RegisterForNotification<LabeledDesignerItem>("Text", this.EndPayoutTextBlock, OnEndPayoutTextPropertyChanged);
            }

        }

        public override void Delete()
        {
            this.Designer.Children.Remove(LabelTextBlock);

            if (((ModelItemType.Chance | ModelItemType.Decision) & this.ModelItem.Type) != ModelItemType.None)
            {
                this.Designer.Children.Remove(CalculatedPayoutTextBlock);
            }

            if (this.ModelItem.Type == ModelItemType.End)
            {
                this.Designer.Children.Remove(EndPayoutTextBlock);
                this.Designer.Children.Remove(EndProbabilityTextBlock);
            }

            base.Delete();
        }

        private void OnLabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateLabelPosition();
        }

        private void OnCalculatedPayoutTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateCalculatedPayoutPosition();
        }

        private void OnEndPayoutTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateEndPayoutPosition();
            this.UpdateEndPayoutForeground();
        }

        private void OnEndProbabilityTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateEndProbabilityPosition();
        }

        public override void SnapToGridLines()
        {
            base.SnapToGridLines();
            this.UpdateLabelsPositions();
        }

        private void UpdateLabelsPositions()
        {
            this.UpdateLabelPosition();
            this.UpdateCalculatedPayoutPosition();
            this.UpdateEndPayoutPosition();
            this.UpdateEndProbabilityPosition();         
        }

        private void UpdateLabelPosition()
        {
            if (this.LabelTextBlock != null)
            {
                double x = this.GetHorizontalMiddle() - (this.LabelTextBlock.ActualWidth / 2) + this.LabelOffset.X;
                double y = this.Position.Y - this.LabelTextBlock.ActualHeight + this.LabelOffset.Y;
                this.LabelTextBlock.Margin = new Thickness(x, y, 0, 0);
            }
        }

        private void UpdateCalculatedPayoutPosition()
        {
            if (this.CalculatedPayoutTextBlock != null)
            {
                double x = this.GetHorizontalMiddle() - (this.CalculatedPayoutTextBlock.ActualWidth / 2) + this.CalculatedPayoutOffset.X;
                double y = this.Position.Y + this.Height + this.CalculatedPayoutOffset.Y;
                this.CalculatedPayoutTextBlock.Margin = new Thickness(x, y, 0, 0);
            }
        }

        private void UpdateEndPayoutPosition()
        {
            if (this.EndPayoutTextBlock != null)
            {
                double x = this.Margin.Left + this.Width + this.EndPayoutOffset.X;
                double y = this.GetVerticalMiddle() - (this.EndPayoutTextBlock.ActualHeight / 2) + this.EndPayoutOffset.Y;
                this.EndPayoutTextBlock.Margin = new Thickness(x, y, 0, 0);
            }
        }

        private void UpdateEndProbabilityPosition()
        {
            if (this.EndProbabilityTextBlock != null)
            {
                double x = this.Margin.Left + this.Width + this.EndProbabilityOffset.X;
                double y = this.GetVerticalMiddle() - (this.EndProbabilityTextBlock.ActualHeight / 2) + this.EndProbabilityOffset.Y;
                this.EndProbabilityTextBlock.Margin = new Thickness(x, y, 0, 0);
            } 
        }

        private void UpdateEndPayoutForeground()
        {
            if (this.EndPayoutTextBlock != null)
            {
                EndModel endItem = this.ModelItem as EndModel;

                if (endItem != null)
                {
                    if (endItem.EndPayout >= 0)
                    {
                        this.EndPayoutTextBlock.Foreground = this.PositivePayoutForeground;
                    }
                    else
                    {
                        this.EndPayoutTextBlock.Foreground = this.NegativePayoutForeground;
                    }
                } 
            }
        }

        private double GetHorizontalMiddle()
        {
            return this.Position.X + (this.Width / 2);
        }

        private double GetVerticalMiddle()
        {
            return this.Position.Y + (this.Height / 2);
        }

        private TextBlock InitializeTextBlock()
        {
            TextBlock newTextBlock = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                IsHitTestVisible = false
            };

            return newTextBlock;
        }

        public void SymetricMeasure(int currentDepth, ref double[] output)
        {
            Size optSize = this.OptimumSize;
            double width = optSize.Width;
            double height = optSize.Height;

            if (this.ChildrenConnections.Any())
            {
                double maxConnection = this.ChildrenConnections.Max(connection => connection.OptimumSize.Width);
                width += maxConnection;

                if (output.Count() < currentDepth)
                {
                    Array.Resize(ref output, currentDepth);
                }

                output[currentDepth - 1] = Math.Max(output[currentDepth - 1], maxConnection);

                foreach (LabeledDesignerItem item in this.Children)
                {
                    item.SymetricMeasure(currentDepth + 1, ref output);
                }

                width += this.Children.Max(item => item.FamilySize.Width);
                height = Math.Max(this.Children.Count() * this.Children.Max(item => item.FamilySize.Height), height);
            }

            this.FamilySize = new Size(width, height);
        }

        public void SymetricAlign(double horizontalOffset, double verticalOffset, double[] horizontalAligment, int currentDepth)
        {
            this.SetPosition(horizontalOffset, verticalOffset - this.Height/2);

            if (this.Children.Any())
	        {
                List<LabeledDesignerItem> children = this.Children.ToList();

                double childOptimalSize = this.FamilySize.Height / children.Count;
                double top = verticalOffset - (children.Count/2 * childOptimalSize);
                top += (children.Count % 2 != 0) ? 0 : childOptimalSize / 2;

                for (int i = 0; i < children.Count; i++)
			    {
                    children[i].SymetricAlign(this.Width + horizontalOffset + horizontalAligment[currentDepth-1], top + (i * childOptimalSize), horizontalAligment, currentDepth + 1);
			    }

	        }

            
        }

        public void CondensedMeasure(int currentDepth, ref double[] output)
        {
            Size optSize = this.OptimumSize;
            double width = optSize.Width;
            double height = optSize.Height;

            if (this.ChildrenConnections.Any())
            {
                double maxConnection = this.ChildrenConnections.Max(connection => connection.OptimumSize.Width);
                width += maxConnection;

                if (output.Count() < currentDepth)
                {
                    Array.Resize(ref output, currentDepth);
                }

                output[currentDepth - 1] = Math.Max(output[currentDepth - 1], maxConnection);

                foreach (LabeledDesignerItem item in this.Children)
                {
                    item.CondensedMeasure(currentDepth + 1, ref output);
                }

                width += this.Children.Max(item => item.FamilySize.Width);
                height = Math.Max(this.Children.Sum(item => item.FamilySize.Height), height);
            }

            this.FamilySize = new Size(width, height);
        }

        public void CondensedAlign(double horizontalOffset, double verticalOffset, double[] horizontalAligment, int currentDepth)
        {
            if (this.Children.Any())
            {
                List<LabeledDesignerItem> children = this.Children.ToList();

                double top = verticalOffset - this.FamilySize.Height / 2;
                double sumaricTransition = 0;

                for (int i = 0; i < children.Count; i++)
                {
                    children[i].CondensedAlign(this.Width + horizontalOffset + horizontalAligment[currentDepth - 1], top + sumaricTransition + children[i].FamilySize.Height/2, horizontalAligment, currentDepth + 1);

                    sumaricTransition += children[i].FamilySize.Height;
                }

                double firstChildVerticalMiddle = children[0].Position.Y + children[0].Height / 2;
                double lastChildVerticalMiddle = children[children.Count - 1].Position.Y + children[children.Count - 1].Height / 2;

                double verticalMiddle = firstChildVerticalMiddle + (lastChildVerticalMiddle - firstChildVerticalMiddle) / 2;

                this.SetPosition(horizontalOffset, verticalMiddle - this.Height / 2);
            }
            else
            {
                this.SetPosition(horizontalOffset, verticalOffset - this.Height / 2);
            }
        }
    }
}
