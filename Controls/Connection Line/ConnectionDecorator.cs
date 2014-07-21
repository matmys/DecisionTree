using DecisionTree.Model;
using System.Windows;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DecisionTree.Controls
{
    //<summary>
    //Handles the ConnectionLine during the drag&drop operations on the designerGrid.
    //</summary>

    public class ConnectionDecorator : ConnectionLine
    {
        private DesignerGrid designerGrid;

        public DesignerItem ConnectedTo { get; private set; }
        public bool IsConnected { get; private set; }

        public ConnectionDecorator()
        {
            this.AllowRender = false;
        }

        public void InitializeConnections(DesignerGrid designer)
        {
            this.designerGrid = designer;

            if (this.designerGrid != null)
            {
                this.designerGrid.panelDragDropTarget.DragOver += panelDragDropTarget_DragOver;
                this.designerGrid.panelDragDropTarget.Drop += panelDragDropTarget_Drop;
                this.designerGrid.panelDragDropTarget.DragEnter += panelDragDropTarget_DragEnter;
                this.designerGrid.panelDragDropTarget.DragLeave += panelDragDropTarget_DragLeave;
            }
        }

        public LabeledConnectionLine CopyAsConnectionLine(Point inputAdjustment)
        {
            LabeledConnectionLine line = new LabeledConnectionLine
            {
                StartPoint = this.StartPoint,
                EndPoint = inputAdjustment
            };
            return line;
        }

        private void panelDragDropTarget_DragEnter(object sender, Microsoft.Windows.DragEventArgs e)
        {
            ToolboxItem item = this.designerGrid.GetToolboxItemFromDropData(e);
            if (item != null)
            {
                this.UpdateTemporaryConnection(item, e);
                this.AllowRender = true;
            }
        }

        private void panelDragDropTarget_DragOver(object sender, Microsoft.Windows.DragEventArgs e)
        {
            if (this.AllowRender)
            {
                ToolboxItem item = this.designerGrid.GetToolboxItemFromDropData(e);

                if (item != null)
                {
                    this.UpdateTemporaryConnection(item, e);
                }
            }
        }

        private void panelDragDropTarget_DragLeave(object sender, Microsoft.Windows.DragEventArgs e)
        {
            this.AllowRender = false;
        }

        void panelDragDropTarget_Drop(object sender, Microsoft.Windows.DragEventArgs e)
        {
            this.AllowRender = false;
        }

        private void UpdateTemporaryConnection(ToolboxItem item, Microsoft.Windows.DragEventArgs e)
        {
            Point pos = e.GetPosition(this.designerGrid.Designer);
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
                this.Connect(closestItem.OutputPoint, endPoint);
                this.ConnectedTo = closestItem;

                this.Show();
            }
            else
            {
                this.Hide();
                this.ConnectedTo = null;
            }
        }

        private DesignerItem FindClosestDesignerItem(Point relativeTo)
        {
            DesignerItem closestItem = null;
            double minDistance = double.MaxValue;

            IEnumerable<DesignerItem> collection = this.designerGrid.Designer.Children.OfType<DesignerItem>().Where(designerItem => designerItem.ModelItem.Type != ModelItemType.End);

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

        private new void Show()
        {
            base.Show();
            this.IsConnected = true;
        }

        private new void Hide()
        {
            base.Hide();
            this.IsConnected = false;
        }
    }
}
