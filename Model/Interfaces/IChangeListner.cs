using System.Windows;

namespace DecisionTree.Model
{
    public interface IChangeListner
    {
        void OnConnectionPropertyChanged(Connection connection, DependencyPropertyChangedEventArgs e);

        void OnModelItemChanged(ModelItem item, ModelChange change);
    }
}
