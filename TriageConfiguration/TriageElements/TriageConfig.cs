namespace TriageConfiguration.TriageElements
{
    public class TriageConfig
    {
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public List<Rulesets> Rulesets { get; set; } = new List<Rulesets>();
        public Results? DefaultResult { get; set; }
    }
}
