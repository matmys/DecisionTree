using DecisionTree.Model;
using DecisionTree.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Data;

namespace DecisionTree.Controls
{
    public partial class PropertiesBox
    {
        public DesignerItem LoadedItem;

        public PropertiesBox()
        {
            InitializeComponent();
        }

        public void Load(DesignerItem item)
        {
            LoadedItem = item;
            this.LoadImage();
            this.LoadModelItemData();
            this.DumpConnectionBoxes();
            this.CreateConnectionBoxes();
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

                BindingHelper.RegisterBinding("ItemLabel", this.LoadedItem.ModelItem, this.LabelTextBox, TextBox.TextProperty, BindingMode.TwoWay);
            }
        }

        private void LoadModelItemData()
        {
            this.ModelTypeTextBlock.Text = LoadedItem.ModelItem.Type.ToString() + " Node";
        }

        private void LoadImage()
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

        private void DumpConnectionBoxes()
        {
            foreach (ConnectionPropertiesBox box in this.ConnectionsStackPanel.Children.OfType<ConnectionPropertiesBox>().ToList())
            {
                this.ConnectionsStackPanel.Children.Remove(box);
            }
        }

        private void CreateConnectionBoxes()
        {
            if (this.LoadedItem != null)
            {
                for (int i = 0; i < this.LoadedItem.ModelItem.ChildrenConnections.Count; i++)
                {
                    bool hasItemProbability = (this.LoadedItem.ModelItem.Type == ModelItemType.Chance);
                    ConnectionPropertiesBox box = new ConnectionPropertiesBox(i+1, hasItemProbability);
                    box.BindedConnection = LoadedItem.ModelItem.ChildrenConnections[i].ConnectionPresenter;

                    this.ConnectionsStackPanel.Children.Add(box);
                }
            }
        }
    }
}
