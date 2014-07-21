using DecisionTree.Extensions;
using System.Collections.ObjectModel;

namespace DecisionTree.Model
{
    public interface INotifiable
    {
        ExtendedObservableCollection<Notification> Notifications { get; }
    }
}
