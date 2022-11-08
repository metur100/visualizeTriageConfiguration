using TriageConfiguration.TriageElements;

namespace TriageConfiguration.Drawer
{
    public class TriageDrawer
    {
        public static string Draw(TriageConfig? triageConfig, ITriageDrawer triageDrawer, OutputTypeEnum outputType)
        {
            triageDrawer.StartSet(triageConfig?.CustomerName, triageConfig?.Description, outputType);
            for (int i = 0; i < triageConfig?.Rulesets.Count; i++)
            {
                triageDrawer.StartRuleSet(triageConfig?.Rulesets[i].Description);
                for (int j = 0; j < triageConfig?.Rulesets[i].Rules?.Count; j++)
                {
                    var ruleCriteria = triageConfig.Rulesets[i].Rules[j].Criteria;
                    var minValue = triageConfig.Rulesets[i].Rules[j].RangeMinValue;
                    var maxValue = triageConfig.Rulesets[i].Rules[j].RangeMaxValue;

                    switch (triageConfig.Rulesets[i].Rules[j].Type)
                    {
                        case RuleTypeEnum.RegEx:
                            triageDrawer.AddRegExRule(ruleCriteria, triageConfig.Rulesets[i].Rules[j].RegEx);
                            break;
                        case RuleTypeEnum.Boolean:
                            triageDrawer.AddBoolRule(ruleCriteria, triageConfig.Rulesets[i].Rules[j].BooleanStateValue);
                            break;
                        case RuleTypeEnum.Range:
                            triageDrawer.AddRangeRule(ruleCriteria, maxValue, minValue);
                            break;
                        default:
                            break;
                    }
                }
                for (int k = 0; k < triageConfig?.Rulesets[i].Results.Count; k++)
                {
                    triageDrawer.AddResult(triageConfig?.Rulesets[i].Results[k].Name, triageConfig?.Rulesets[i].Results[k].Description,
                         triageConfig?.Rulesets[i].Results[k].RepairCostsVisible, triageConfig?.Rulesets[i].Results[k].ResidualValueVisible,
                         triageConfig?.Rulesets[i].Results[k].ReplacementValueVisible, triageConfig?.Rulesets[i].Results[k].Action?.Type.GetValueOrDefault());
                }
            }
            triageDrawer.EndSetResult(triageConfig?.DefaultResult?.Name, triageConfig?.DefaultResult?.Description,
                triageConfig?.DefaultResult?.RepairCostsVisible, triageConfig?.DefaultResult?.ResidualValueVisible,
                triageConfig?.DefaultResult?.ReplacementValueVisible);

            return triageDrawer.GetVisualization();
        }
    }
}
