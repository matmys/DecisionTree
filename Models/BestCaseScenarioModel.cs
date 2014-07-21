using DecisionTree.Extensions;
using DecisionTree.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace DecisionTree.Models
{
    public class BestCaseScenarioModel : IModelable, INotifiable, IHighlightable, IChangeListner
    {
        public string Name { get; private set; }
        public virtual bool IsValid
        {
            get
            {
                return true;
            }
        }

        public ObservableCollection<ModelItem> NodesWithPathsToHighlight { get; private set; }
        public ExtendedObservableCollection<Notification> Notifications { get; private set; }

        public BestCaseScenarioModel()
        {
            this.Name = "Best Case Scenario";
            NodesWithPathsToHighlight = new ObservableCollection<ModelItem>();
            Notifications = new ExtendedObservableCollection<Notification>();
        }

        public virtual void Calculate(ModelItem rootNode)
        {
            
        }

        public virtual void OnConnectionPropertyChanged(Connection connection, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == Connection.PayoutProperty)
            {
                this.CalculateEndPayoutsOnConnectionChanged(connection, e);
            }

            this.CalculatePotentialPayoutsOnConnectionChanged(connection, e);
            this.FindResultNodesOnConnectionChanged(connection, e);
            this.ClearNotifications();
            this.AddNotifications();
        }

        public virtual void OnModelItemChanged(ModelItem item, ModelChange change)
        {
            this.CalculateEndPayoutsOnModelItemChanged(item, change);
            this.CalculatePotentialPayoutsOnItemChanged(item, change);
            this.FindResultNodesOnItemChanged(item, change);
        }

        protected virtual void CalculateEndPayoutsOnModelItemChanged(ModelItem item, ModelChange change)
        {
            //After the ModelItem has been added
            EndModel endModelItem = item as EndModel;

            if (endModelItem != null && change == ModelChange.Added)
            {
                double value = 0;
                ModelItem current = item;

                while (current.Parent != null)
                {
                    value += current.ParentConnection.Payout;
                    current = current.Parent;
                }

                endModelItem.EndPayout = value;
            }

            //Just before the ModelItem deletion
            if (change == ModelChange.Deleted)
            {
                foreach (ModelItem child in item.Children)
                {
                    this.CalculateEndPayoutOnModelItemDeletion(child, 0);
                }
            }
        }

        protected virtual void CalculateEndPayoutOnModelItemDeletion(ModelItem item, double currentValue)
        {
            if (item.Type != ModelItemType.End)
            {
                foreach (ModelItem child in item.Children)
                {
                    double newValue = currentValue + child.ParentConnection.Payout;
                    this.CalculateEndPayoutOnModelItemDeletion(child, newValue);
                }
            }
            else
            {
                EndModel endModel = item as EndModel;
                endModel.EndPayout = currentValue;
            }
        }

        protected virtual void CalculatePotentialPayoutsOnItemChanged(ModelItem item, ModelChange change)
        {
            if (change == ModelChange.Deleted && item.Parent != null)
            {
                item.ParentConnection.Payout = 0;
                item.PotentialPayout = 0;

                ModelItem current = item.Parent;

                while (current != null)
                {
                    this.CalculateModelItemPotentialPayout(current);
                    current = current.Parent;
                }
            }
        }

        protected virtual void FindResultNodesOnItemChanged(ModelItem item, ModelChange change)
        {
            if (item.Type == ModelItemType.End)
            {
                this.ClearResultItems();
                this.FindResultNodes(item.Root, 100);
            }
        }

        protected virtual void CalculateEndPayoutsOnConnectionChanged(Connection connection, DependencyPropertyChangedEventArgs e)
        {
                double deltaValue = (double)e.NewValue - (double)e.OldValue;

                IEnumerable<EndModel> descendants = connection.ChildItem.Descendants<EndModel>();

                foreach (EndModel descendant in descendants)
                {
                    descendant.EndPayout += deltaValue;
                }
        }

        protected virtual void CalculatePotentialPayoutsOnConnectionChanged(Connection connection, DependencyPropertyChangedEventArgs e)
        {
            ModelItem item = connection.ParentItem;

            while (item != null)
            {
                this.CalculateModelItemPotentialPayout(item);
                item = item.Parent;
            }
        }

        protected virtual void CalculateModelItemPotentialPayout(ModelItem item)
        {
            if (item.Type == ModelItemType.Chance)
            {
                double value = 0;

                foreach (Connection connection in item.ChildrenConnections)
                {

                    value += (connection.Payout + connection.ChildItem.PotentialPayout) * (connection.Probability / 100);
                }

                (item as ChanceModel).CalculatedPayout = value;
            }
            else if (item.Type == ModelItemType.Decision)
            {
                double value = Double.MinValue;

                foreach (Connection connection in item.ChildrenConnections)
                {
                    value = Math.Max(value, (connection.Payout + connection.ChildItem.PotentialPayout));
                }

                (item as DecisionModel).CalculatedPayout = value;
            }
        }

        protected virtual void FindResultNodesOnConnectionChanged(Connection connection, DependencyPropertyChangedEventArgs e)
        {
            this.ClearResultItems();
            this.FindResultNodes(connection.ParentItem.Root, 100);
        }

        protected virtual void FindResultNodes(ModelItem item, double probability)
        {
            switch (item.Type)
            {
                case ModelItemType.Decision:
                {
                    if (item.Children.Any())
                    {
                        double max = item.Children.Max(child => (child.PotentialPayout + child.ParentConnection.Payout));

                        foreach (ModelItem child in item.Children.Where(child => (child.PotentialPayout + child.ParentConnection.Payout) == max))
                        {
                            this.FindResultNodes(child, probability);
                        }
                    }
                    break;
                }
                case ModelItemType.Chance:
                {
                    foreach (ModelItem child in item.Children)
                    {
                        double newProbability = probability * (child.ParentConnection.Probability / 100);
                        this.FindResultNodes(child, newProbability);
                    }
                    break;
                }
                case ModelItemType.End:
                {
                    EndModel endModel = item as EndModel;
                    endModel.EndProbability = probability;
                    this.NodesWithPathsToHighlight.Add(item);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        private void ClearResultItems()
        {
            foreach (EndModel item in this.NodesWithPathsToHighlight)
            {
                item.EndProbability = 0;
            }

            this.NodesWithPathsToHighlight.Clear();
        }

        private void ClearNotifications()
        {
            this.Notifications.Clear();
        }

        private void AddNotifications()
        {
            IEnumerable<EndModel> collection = NodesWithPathsToHighlight.OfType<EndModel>();

            if (collection.Any())
            {
                double max = collection.Max(item => item.EndPayout);
                double avg = collection.Average(item => item.EndPayout);
                double min = collection.Min(item => item.EndPayout);

                string maxPayout = "Max Payout $ " + String.Format("{0:#,0.##;(#,0.##)}", max);
                string avgPayout = "Avg Payout $ " + String.Format("{0:#,0.##;(#,0.##)}", avg);
                string minPayout = "Min Payout $ " + String.Format("{0:#,0.##;(#,0.##)}", min);

                Notifications.Add(new Notification(maxPayout, new SolidColorBrush(Colors.Green)));
                Notifications.Add(new Notification(avgPayout));
                Notifications.Add(new Notification(minPayout, new SolidColorBrush(Colors.Red)));
            }
        }
    }
}
