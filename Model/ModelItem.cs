using System.Windows;
using System.Linq;
using System.Collections.Generic;
using DecisionTree.Controls;
using System;

namespace DecisionTree.Model
{
    public abstract class ModelItem : DependencyObject
    {
        public Connection ParentConnection { get; private set; }
        public List<Connection> ChildrenConnections { get; private set; }
        public ModelItemType Type { get; set; }

        public DesignerItem ItemPresenter { get; set; }
        public ModelItem Parent
        {
            get
            {
                return (this.ParentConnection != null) ? ParentConnection.ParentItem : null;
            }
        }
        public IEnumerable<ModelItem> Children
        {
            get
            {
                return this.ChildrenConnections.Select(connection => connection.ChildItem);
            }     
        }
        public ModelItem Root
        {
            get
            {
                ModelItem current = this;

                while (current.Parent != null)
                {
                    current = current.Parent;
                }

                return current;
            }
        }

        public double PotentialPayout
        {
            get
            {
                switch (this.Type)
                {
                    case ModelItemType.Decision:
                    {
                        return (this as DecisionModel).CalculatedPayout;
                    }
                    case ModelItemType.Chance:
                    {
                        return (this as ChanceModel).CalculatedPayout;
                    }
                    default:
                    {
                        return 0;
                    }
                }
            }

            set
            {
                switch (this.Type)
                {
                    case ModelItemType.Decision:
                    {
                        (this as DecisionModel).CalculatedPayout = value;
                        break;
                    }  
                    case ModelItemType.Chance:
                    {
                        (this as ChanceModel).CalculatedPayout = value;
                        break;
                    }
                    default:
                    {
                        break;
                    }     
                }
            }
        }

        #region ItemLabel Property
        public string ItemLabel
        {
            get { return (string)GetValue(ItemLabelProperty); }
            set { SetValue(ItemLabelProperty, value); }
        }

        public static readonly DependencyProperty ItemLabelProperty =
            DependencyProperty.Register("ItemItemLabel", typeof(string), typeof(ModelItem), new PropertyMetadata(""));
        #endregion

        protected ModelItem()
        {
            ChildrenConnections = new List<Connection>();
        }

        public void AddChildConnection(Connection connection)
        {
            if (this.Type == ModelItemType.Chance)
            {
                double value = ChildrenConnections.Sum(conn => conn.Probability);

                if (value <= 100)
                {
                    connection.Probability = 100 - value;
                }
            }

            ChildrenConnections.Add(connection);
        }

        public void SetParentConnection(Connection connection)
        {
            ParentConnection = connection;
        }

        public static ModelItem GetModelItem(string modelType)
        {
            ModelItem newModelItem;

            switch (modelType)
            {
                case "Decision":
                {
                    newModelItem = new DecisionModel
                    {
                        Type = ModelItemType.Decision
                    };
                    return newModelItem;
                }
                case "Chance":
                {
                    newModelItem = new ChanceModel
                    {
                        Type = ModelItemType.Chance
                    };
                    return newModelItem;
                }
                case "End":
                {
                    newModelItem = new EndModel
                    {
                        Type = ModelItemType.End
                    };
                    return newModelItem;
                }
                default:
                {
                    return null;
                }
            }
        }

        public IEnumerable<T> Descendants<T>() where T : ModelItem
        {
            List<T> result = new List<T>();
            T item = this as T;

            if (item != null)
            {
                result.Add(item);
            }

            foreach (ModelItem child in this.Children)
            {
                result.AddRange(child.Descendants<T>());
            }

            return result;
        }
    }

    [Flags]
    public enum ModelItemType
    {
        None = 0,
        Decision = 1,
        Chance = 2,
        End = 4
    };

    [Flags]
    public enum ModelChange
    {
        None = 0,
        Added = 1,
        Deleted = 2
    };
}
