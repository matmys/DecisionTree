using DecisionTree.Model;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using DecisionTree.Extensions;
using System.Windows.Data;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace DecisionTree.Controls
{
    public partial class Toolbar
    {
        #region SelectedModel Property
        public IModelable SelectedModel
        {
            get { return (IModelable)GetValue(SelectedModelProperty); }
            set { SetValue(SelectedModelProperty, value); }
        }

        public static readonly DependencyProperty SelectedModelProperty =
            DependencyProperty.Register("SelectedModel", typeof(IModelable), typeof(Toolbar), new PropertyMetadata(null));
        #endregion

        public DesignerGrid DesignerGrid { get; set; }

        public Toolbar()
        {
            InitializeComponent();
        }

        public void InitializeBindings()
        {
            if (this.DesignerGrid != null)
            {
                Settings settings = Settings.Instance;

                BindingHelper.RegisterBinding("IsGridEnabled", settings, this.GridToggleButton, ImageToggleButton.IsCheckedProperty, BindingMode.TwoWay);
            }
        }

        private void ModelsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
	        {
                IModelable model = e.AddedItems[0] as IModelable;

                if (model != null)
                {
                    this.SelectedModel = model;
                }
	        }
        }

        public void SelectFirstModel()
        {
            if (this.ModelsComboBox.Items.Any())
            {
                this.ModelsComboBox.SelectedIndex = 0;
            }
        }

        private void GridToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ImageToggleButton button = sender as ImageToggleButton;

            if (button != null && this.DesignerGrid != null)
            {
                bool isChecked = false;

                if (button.IsChecked == true)
                {
                    isChecked = true;
                }

                Settings.Add<bool>("IsGridEnabled", isChecked);
            }
        }

        private void ExportImageButton_Click(object sender, RoutedEventArgs e)
        {
            this.DesignerGrid.SaveContentAsPNG();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DesignerGrid.SaveContent();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            this.DesignerGrid.LoadContent();
        }

        private void NewDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            this.DesignerGrid.Clear();
        }

        private void CalculateModelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedModel != null && this.SelectedModel.IsValid)
            {
                ModelItem item = this.DesignerGrid.Children.OfType<ModelItem>().FirstOrDefault();
                item = item.Root;
                this.SelectedModel.Calculate(item);
            }
        }

        private void SymetricAutoLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.DesignerGrid.SymetricAutoLayout();
        }

        private void CondensedAutoLayoutButton_Click(object sender, RoutedEventArgs e)
        {
            this.DesignerGrid.CondensedAutoLayout();
        }
    }
}
