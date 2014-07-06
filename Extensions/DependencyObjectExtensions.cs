using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DecisionTree.Extensions
{
    public static class DependencyObjectExtensions
    {

        public static T FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
        {
            return obj.FindAncestor(typeof(T)) as T;
        }

        public static DependencyObject FindAncestor(this DependencyObject obj, Type ancestorType)
        {
            var tmp = VisualTreeHelper.GetParent(obj);
            while (tmp != null && !ancestorType.IsAssignableFrom(tmp.GetType()))
            {
                tmp = VisualTreeHelper.GetParent(tmp);
            }
            return tmp;
        }

        public static T FindChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            if (obj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                var result = (child as T) ?? FindChild<T>(child);
                if (result != null) return result;
            }
            return null;
        }
    }
}
