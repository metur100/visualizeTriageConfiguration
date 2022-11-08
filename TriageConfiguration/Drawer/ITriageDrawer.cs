using TriageConfiguration.TriageElements;

namespace TriageConfiguration.Drawer
{
    public interface ITriageDrawer
    {
        void StartSet(string? customerName, string? description, OutputTypeEnum? outputType);
        void StartRuleSet(string? description);
        void AddBoolRule(CriteriaEnum? criteria, bool? stateValue);
        void AddRegExRule(CriteriaEnum? criteria, string? regEx);
        void AddRangeRule(CriteriaEnum? criteria, double? maxRange, double? minRange);
        void AddResult(string? name, string? description, bool? repairCostsVisible, bool? residualValueVisible, bool? replacementValueVisible, ActionTypeEnum? actionType);
        void EndSetResult(string? name, string? description, bool? repairCostsVisible, bool? residualValueVisible, bool? replacementValueVisible);
        string GetVisualization();
    }
}
