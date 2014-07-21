using DecisionTree.Model;
using System.Collections.ObjectModel;

namespace DecisionTree.Models
{
    public static class ModelsFactory
    {
        public static ObservableCollection<IModelable> GetModels()
        {
            ObservableCollection<IModelable> models = new ObservableCollection<IModelable>();

            models.Add(new BestCaseScenarioModel());

            return models;
        }
    }
}
