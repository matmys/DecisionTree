
using System.Windows;
using System.Windows.Data;
namespace DecisionTree.Extensions
{
    public static class BindingHelper
    {
        public static void RegisterBinding(string sourcePropertyName, DependencyObject source, DependencyObject target, DependencyProperty targetProperty, BindingMode mode = BindingMode.OneWay, string stringFormat="")
        {
            Binding binding = new Binding(sourcePropertyName)
            {
                Source = source,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = mode,
                StringFormat = stringFormat
            };

            BindingOperations.SetBinding(target, targetProperty, binding);
        }

        public static void RegisterBinding(string sourcePropertyName, DependencyObject source, DependencyObject target, DependencyProperty targetProperty, string stringFormat)
        {
            BindingHelper.RegisterBinding(sourcePropertyName, source, target, targetProperty, BindingMode.OneWay, stringFormat);
        }

        public static void RegisterForNotification<T>(string sourcePropertyName, DependencyObject source, PropertyChangedCallback callback) where T : DependencyObject
        {
            var dp = DependencyProperty.RegisterAttached(
                "ListenAttached" + sourcePropertyName,
                typeof(object),
                typeof(T),
                new PropertyMetadata(callback));

            BindingHelper.RegisterBinding(sourcePropertyName, source, source, dp);
        }
    }
}
