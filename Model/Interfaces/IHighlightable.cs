using DecisionTree.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DecisionTree.Model
{
    public interface IHighlightable
    {
          ObservableCollection<ModelItem> NodesWithPathsToHighlight { get; }
    }
}
