using System.Windows;
using System.Linq;
using System.Collections.Generic;

namespace DecisionTree.Model
{
    public class ModelItem : DependencyObject
    {
        public List<ConnectionProperties> Connections;
        public ModelItemType Type;

        #region Name Property
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(ModelItem), new PropertyMetadata(""));
        #endregion

        #region Label Property
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ModelItem), new PropertyMetadata(""));
        #endregion
  
        public ModelItem()
        {
            Connections = new List<ConnectionProperties>();
        }

        public void AddConnection(ConnectionProperties connection)
        {
            if (this.Type == ModelItemType.Chance)
            {
                double value = Connections.Sum(conn => conn.Probability);
                connection.Probability = 100 - value;
            }

            Connections.Add(connection);
        }

        public static ModelItem GetModelItem(string modelType)
        {
            ModelItem newModelItem;

            switch (modelType)
            {
                case "Decision":
                {
                    newModelItem = new DecisionModel() as ModelItem;
                    newModelItem.Type = ModelItemType.Decision;
                    return newModelItem;
                }
                case "Chance":
                {
                    newModelItem = new ChanceModel() as ModelItem;
                    newModelItem.Type = ModelItemType.Chance;
                    return newModelItem;
                }
                case "End":
                {
                    newModelItem = new EndModel() as ModelItem;
                    newModelItem.Type = ModelItemType.End;
                    return newModelItem;
                }
                default:
                {
                    return null;
                }
            }
        }
    }

    public enum ModelItemType
    {
        None = 0,
        Decision = 2,
        Chance = 4,
        End = 8
    };

}
