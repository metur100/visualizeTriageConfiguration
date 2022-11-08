namespace TriageConfiguration.TriageElements
{
    public class Rules
    {
        public CriteriaEnum? Criteria { get; set; }
        public RuleTypeEnum? Type { get; set; }
        public double? RangeMinValue { get; set; }
        public double? RangeMaxValue { get; set; }
        public bool? BooleanStateValue { get; set; }
        public string? RegEx { get; set; }
    }
}
