namespace TriageConfiguration.TriageElements
{
    public class Results
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Action? Action { get; set; }
        public bool? RepairCostsVisible { get; set; }
        public bool? ResidualValueVisible { get; set; }
        public bool? ReplacementValueVisible { get; set; }
    }
}
