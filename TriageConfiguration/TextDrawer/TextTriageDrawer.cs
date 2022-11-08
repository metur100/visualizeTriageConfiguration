using System.Text;
using TriageConfiguration.Drawer;
using TriageConfiguration.TriageElements;

namespace TriageConfiguration.TextDrawer
{
    public class TextTriageDrawer : ITriageDrawer
    {
        private readonly StringBuilder LogString = new();
        private StringBuilder SavedString = new();

        public void StartSet(string? customerName, string? description, OutputTypeEnum? outputType)
        {
            string result = $"Customer name: {customerName} {"\n" + "Description: "} {description} {"\n"}";
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void StartRuleSet(string? description)
        {
            string result = $"Description: {description}";
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddBoolRule(CriteriaEnum? criteria, bool? stateValue)
        {
            string result = $" Criteria: {criteria} = {stateValue}";
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddRegExRule(CriteriaEnum? criteria, string? regEx)
        {
            string result = $" Criteria: {criteria} = {regEx}";
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddRangeRule(CriteriaEnum? criteria, double? maxRange, double? minRange)
        {
            string result = $" Criteria: {criteria} is inbetween {minRange} und {maxRange}";
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddResult(string? name, string? description, bool? repairCostsVisible, bool? residualValueVisible, bool? replacementValueVisible, ActionTypeEnum? actionType)
        {
            string result = $"Result: {name} {"\n" + "  Description: "} {description} " +
                $"{"\n" + "  RepairCostsVisible: "} {repairCostsVisible}" +
                $"{"\n" + "  ResidualValueVisible: "} {residualValueVisible}" +
                $"{"\n" + "  ReplacementValueVisible: "} {replacementValueVisible}" +
                $"\n  Action: {actionType}" + "\n";
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void EndSetResult(string? name, string? description, bool? repairCostsVisible, bool? residualValueVisible, bool? replacementValueVisible)
        {
            string result = $"Result: {name} {"\n" + "  Description: "} {description} " +
                $"{"\n" + "  RepairCostsVisible: "} {repairCostsVisible}" +
                $"{"\n" + "  ResidualValueVisible: "} {residualValueVisible}" +
                $"{"\n" + "  ReplacementValueVisible: "} {replacementValueVisible}" + "\n";
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public string GetVisualization()
        {
            if (SavedString.Length > 0)
            {
                return SavedString.ToString();
            }
            return string.Empty;
        }
    }
}
