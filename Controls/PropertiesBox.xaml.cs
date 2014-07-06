using DecisionTree.Model;
using DecisionTree.Extensions;

using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace DecisionTree.Controls
{
    public partial class PropertiesBox : UserControl
    {
        public DesignerItem LoadedItem;

        public PropertiesBox()
        {
            InitializeComponent();
        }

        public void Load(DesignerItem item)
        {
            LoadedItem = item;
            this.loadImage();
            this.loadModelItemData();
            this.dumpConnectionBoxes();
            this.createConnectionBoxes();
            this.InitializeBindings();
        }

        public void InitializeBindings()
        {
            if (LoadedItem != null)
            {
                foreach (ConnectionPropertiesBox box in this.ConnectionsStackPanel.Children.OfType<ConnectionPropertiesBox>())
                {
                    box.InitializeBindings();
                }
            }
        }

        private void loadModelItemData()
        {
            this.ModelTypeTextBlock.Text = LoadedItem.ModelItem.Type.ToString() + " Node";
        }

        private void loadImage()
        {
            if (LoadedItem != null)
            {
                string modelType = LoadedItem.ModelItem.Type.ToString();

                string pathData = Application.Current.Resources[modelType + "Data"] as string;
                Brush fill = Application.Current.Resources[modelType + "Brush"] as Brush;
                Brush stroke = Application.Current.Resources[modelType + "Stroke"] as Brush;

                StringToPathGeometryConverter converter = new StringToPathGeometryConverter();

                PropertiesImage.Data = converter.Convert(pathData);
                PropertiesImage.Fill = fill;
                PropertiesImage.Stroke = stroke;
            }
        }

        private void dumpConnectionBoxes()
        {
            foreach (ConnectionPropertiesBox box in this.ConnectionsStackPanel.Children.OfType<ConnectionPropertiesBox>().ToList())
            {
                this.ConnectionsStackPanel.Children.Remove(box);
            }
        }

        private void createConnectionBoxes()
        {
            if (this.LoadedItem != null)
            {
                for (int i = 0; i < this.LoadedItem.ChildrenConnections.Count; i++)
                {
                    bool hasItemProbability = (this.LoadedItem.ModelItem.Type == ModelItemType.Chance) ? true : false;
                    ConnectionPropertiesBox box = new ConnectionPropertiesBox(i+1, hasItemProbability);
                    box.BindedConnection = LoadedItem.ChildrenConnections[i];

                    this.ConnectionsStackPanel.Children.Add(box);
                }
            }
        }
    }
}
