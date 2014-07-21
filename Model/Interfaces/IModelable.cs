using DecisionTree.Extensions;
using System.ComponentModel;

namespace DecisionTree.Model
{
    public interface IModelable
    {
        string Name { get; }
        bool IsValid { get; }
        
        void Calculate(ModelItem rootNode);
    }
}
