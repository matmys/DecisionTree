using DecisionTree.Extensions;
using DecisionTree.Model;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DecisionTree.Controls
{
    //<summary>
    //Handles the ConnectionLine during the drag&drop operations on the DesignerGrid.
    //</summary>

    public class ConnectionDecorator : ConnectionLine
    {
        DesignerGrid DesignerGrid;

        public bool IsConnected { get; private set; }

        public ConnectionDecorator()
        {
            this.AllowRender = false;
        }

        public void InitializeConnections(DesignerGrid designerGrid)
        {
            DesignerGrid = designerGrid;

            if (DesignerGrid != null)
            {
                DesignerGrid.panelDragDropTarget.DragOver += panelDragDropTarget_DragOver;
                DesignerGrid.panelDragDropTarget.Drop += panelDragDropTarget_Drop;
                DesignerGrid.panelDragDropTarget.DragEnter += panelDragDropTarget_DragEnter;
                DesignerGrid.panelDragDropTarget.DragLeave += panelDragDropTarget_DragLeave;
            }
        }

        public LabeledConnectionLine CopyAsConnectionLine(Point inputAdjustment)
        {
            LabeledConnectionLine line = new LabeledConnectionLine();
            line.StartPoint = this.StartPoint;
            line.EndPoint = inputAdjustment;
            line.ParentItem = this.ParentItem;
            return line;
        }

        protected void panelDragDropTarget_DragEnter(object sender, Microsoft.Windows.DragEventArgs e)
        {
            ToolboxItem item = DesignerGrid.GetToolboxItemFromDropData(e);
            if (item != null)
            {
                this.updateTemporaryConnection(item, e);
                this.AllowRender = true;
            }
        }

        protected void panelDragDropTarget_DragOver(object sender, Microsoft.Windows.DragEventArgs e)
        {
            if (this.AllowRender)
            {
                ToolboxItem item = DesignerGrid.GetToolboxItemFromDropData(e);

                if (item != null)
                {
                    this.updateTemporaryConnection(item, e);
                }
            }
        }

        protected void panelDragDropTarget_DragLeave(object sender, Microsoft.Windows.DragEventArgs e)
        {
            this.AllowRender = false;
        }

        void panelDragDropTarget_Drop(object sender, Microsoft.Windows.DragEventArgs e)
        {
            this.AllowRender = false;
        }

        private void updateTemporaryConnection(ToolboxItem item, Microsoft.Windows.DragEventArgs e)
        {
            Point pos = e.GetPosition(DesignerGrid.Designer);
            double deltaVertical = 0, deltaHorizontal = 0;

            if (item.DragPoint.HasValue)
            {
                deltaVertical = item.DragPoint.Value.Y;
                deltaHorizontal = item.DragPoint.Value.X;
            }

            Point endPoint = new Point(pos.X - deltaHorizontal, pos.Y - deltaVertical + item.Height / 2);
            DesignerItem closestItem = FindClosestDesignerItem(endPoint);

            if (closestItem != null)
            {
                this.EndPoint = endPoint;
                this.Show();
                this.StartPoint = closestItem.OutputPoint;
                this.ParentItem = closestItem;
            }
            else
            {
                this.Hide();
                this.ParentItem = null;
            }
        }

        private DesignerItem FindClosestDesignerItem(Point relativeTo)
        {
            DesignerItem closestItem = null;
            double minDistance = double.MaxValue;

            IEnumerable<DesignerItem> collection = DesignerGrid.Designer.Children.OfType<DesignerItem>().Where(designerItem => designerItem.ModelItem.Type != ModelItemType.End);

            foreach (DesignerItem item in collection)
            {
                if (item.OutputPoint.X + this.FirstTransition + this.SecondTransition < relativeTo.X)
                {
                    double distance = Math.Sqrt(Math.Pow(relativeTo.X - item.OutputPoint.X, 2) + Math.Pow(relativeTo.Y - item.OutputPoint.Y, 2));

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestItem = item;
                    }
                }
            }

            return closestItem;
        }

        new public void Show()
        {
            base.Show();
            this.IsConnected = true;
        }

        new public void Hide()
        {
            base.Hide();
            this.IsConnected = false;
        }
    }
}
