using Microsoft.Windows;
using System.Windows;
using System.Windows.Input;

namespace DecisionTree.Controls
{
    public partial class ToolboxItem
    {
        public string StyleString { get; set; }
        public Point? DragPoint { get; set; }
        public Style ItemStyle { get; set; }

        #region ItemScale Property
        public double ItemScale
        {
            get { return ( double)GetValue(ItemScaleProperty); }
            set { SetValue(ItemScaleProperty, value); }
        }

        public static readonly DependencyProperty ItemScaleProperty =
            DependencyProperty.Register("ItemScale", typeof( double), typeof(ToolboxItem), null);
        #endregion

        public ToolboxItem()
        {
            InitializeComponent();
            this.MouseEnter += ToolboxItem_MouseEnter;
            this.MouseLeave += ToolboxItem_MouseLeave;
            this.MouseLeftButtonDown += ToolboxItem_MouseLeftButtonDown;
            DragDrop.DragDropCompleted += DragDrop_DragDropCompleted;
        }

        void DragDrop_DragDropCompleted(object sender, DragDropCompletedEventArgs e)
        {
            DragPoint = null;
        }

        void ToolboxItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragPoint = new Point(e.GetPosition(this).X, e.GetPosition(this).Y - 8);
        }

        void ToolboxItem_MouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState((ToolboxItem)sender, "Normal", true);
        }

        protected void ToolboxItem_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState((ToolboxItem)sender, "MouseOver", true);
        }
    }
}
