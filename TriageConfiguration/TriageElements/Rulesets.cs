namespace TriageConfiguration.TriageElements
{
    public class Rulesets
    {
        public string? Description { get; set; }
        public List<Rules> Rules { get; set; } = new List<Rules>();
        public List<Results> Results { get; set; } = new List<Results>();
    }
}
