using DecisionTree.Extensions;
using DecisionTree.Model;
using DecisionTree.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DecisionTree
{
    public partial class MainPage : UserControl
    {

        #region Models Property
        public ObservableCollection<IModelable> Models
        {
            get { return (ObservableCollection<IModelable>)GetValue(ModelsProperty); }
            set { SetValue(ModelsProperty, value); }
        }

        public static readonly DependencyProperty ModelsProperty =
            DependencyProperty.Register("Models", typeof(ObservableCollection<IModelable>), typeof(MainPage), new PropertyMetadata(new ObservableCollection<IModelable>()));
        #endregion

        #region SelectedModel Property
        public IModelable SelectedModel
        {
            get { return (IModelable)GetValue(SelectedModelProperty); }
            set { SetValue(SelectedModelProperty, value); }
        }

        public static readonly DependencyProperty SelectedModelProperty =
            DependencyProperty.Register("SelectedModel", typeof(IModelable), typeof(MainPage), new PropertyMetadata(null, OnSelectedModelPropertyChanged));

        private static void OnSelectedModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IHighlightable model = e.OldValue as IHighlightable;
            MainPage page = d as MainPage;

            if (model != null && page != null)
            {
                model.NodesWithPathsToHighlight.CollectionChanged -= page.DesignerGridControl.ModelResults_CollectionChanged;
            }

            page.InitializeSelectedModel();
        }
        #endregion

        public MainPage()
        {
            InitializeComponent();
            this.DesignerGridControl.InitializeBindings();
            this.InitializeToolbar();
            this.AttachEvents();
            this.InitializeModels();
        }

        private void AttachEvents()
        {
            this.KeyDown += this.DesignerGridControl.KeyGestureManager.RiseEvent;
            this.DesignerGridControl.ConnectionPropertyChanged += designerGrid_ConnectionPropertyChanged;
            this.DesignerGridControl.ModelItemsChanged += designerGrid_ModelItemsChanged;
        }

        private void InitializeToolbar()
        {
            this.ToolbarControl.DesignerGrid = DesignerGridControl;
            this.ToolbarControl.InitializeBindings();
        }

        private void InitializeModels()
        {
            this.Models = ModelsFactory.GetModels();

            BindingHelper.RegisterBinding("Models", this, this.ToolbarControl.ModelsComboBox, ComboBox.ItemsSourceProperty);
            BindingHelper.RegisterBinding("SelectedModel", this.ToolbarControl, this, MainPage.SelectedModelProperty);

            this.ToolbarControl.SelectFirstModel();    
        }

        private void InitializeSelectedModel()
        {
            IHighlightable model = this.SelectedModel as IHighlightable;

            if (model != null)
            {
                model.NodesWithPathsToHighlight.CollectionChanged += this.DesignerGridControl.ModelResults_CollectionChanged;
            }

            INotifiable notifiableModel = this.SelectedModel as INotifiable;

            if (notifiableModel != null)
            {
                this.NotificationBar.ItemsSource = notifiableModel.Notifications;
            }
        }

        private void designerGrid_ModelItemsChanged(ModelItem item, ModelChange change)
        {
            IChangeListner listner = this.SelectedModel as IChangeListner;

            if (listner != null)
            {
                listner.OnModelItemChanged(item, change);
            }
        }

        private void designerGrid_ConnectionPropertyChanged(Connection connection, DependencyPropertyChangedEventArgs e)
        {
            IChangeListner listner = this.SelectedModel as IChangeListner;

            if (listner != null)
            {
                listner.OnConnectionPropertyChanged(connection, e);
            }
        }
    }
}
