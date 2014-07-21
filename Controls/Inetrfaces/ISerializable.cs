using System.Collections.Generic;

namespace DecisionTree.Controls
{
    public interface ISerializable
    {
        List<string> PropertiesToSerialize { get; }
    }
}
