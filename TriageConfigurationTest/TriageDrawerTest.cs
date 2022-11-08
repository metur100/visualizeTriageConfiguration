using System.Text.Json.Serialization;
using System.Text.Json;
using TriageConfiguration.TriageElements;
using Moq;
using NUnit.Framework;
using TriageConfiguration.Drawer;

namespace TriageConfigurationTest
{
    public class TriageDrawerTest
    {
        [Test]
        public void Draw_OneRuleSetWithOneRule_RulesSentToDrawer()
        {
            var mockTriageDrawer = new Mock<ITriageDrawer>();
            TriageDrawer.Draw(DeserializeJson("TestFiles\\RuleSetWithOneRule.json"), mockTriageDrawer.Object, OutputTypeEnum.HtmlImage);

            string resultDescription = "Based on the amount of damage, it can be assumed to be a total loss.";
            string defaultDescription = "This case can be processed as Default Result.";

            mockTriageDrawer.Verify(x => x.StartSet("Test Customer", "Test Customer Description", OutputTypeEnum.HtmlImage), Times.Once());
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 1"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(CriteriaEnum.TotalLoss, true), Times.Once);
            mockTriageDrawer.Verify(x => x.AddRegExRule(It.IsAny<CriteriaEnum?>(), It.IsAny<string?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddRangeRule(It.IsAny<CriteriaEnum?>(), It.IsAny<double?>(), It.IsAny<double?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddResult("TotalLossAssessment", resultDescription, true, true, true, null), Times.Once());
            mockTriageDrawer.Verify(x => x.EndSetResult("DefaultResult", defaultDescription, false, false, false), Times.Once());
            mockTriageDrawer.Verify(x => x.GetVisualization(), Times.Once());
        }

        [Test]
        public void Draw_NoRuleSets_RulesSentToDrawer()
        {
            var mockTriageDrawer = new Mock<ITriageDrawer>();
            TriageDrawer.Draw(DeserializeJson("TestFiles\\NoRuleSets.json"), mockTriageDrawer.Object, OutputTypeEnum.HtmlImage);

            string defaultDescription = "This case can be processed as Default Result.";

            mockTriageDrawer.Verify(x => x.StartSet("Test Customer", "Test Customer Description", OutputTypeEnum.HtmlImage), Times.Once());
            mockTriageDrawer.Verify(x => x.StartRuleSet(It.IsAny<string?>()), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(It.IsAny<CriteriaEnum?>(), It.IsAny<bool?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddRegExRule(It.IsAny<CriteriaEnum?>(), It.IsAny<string?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddRangeRule(It.IsAny<CriteriaEnum?>(), It.IsAny<double?>(), It.IsAny<double?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddResult(It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<bool?>(), It.IsAny<ActionTypeEnum>()), Times.Never());
            mockTriageDrawer.Verify(x => x.EndSetResult("DefaultResult", defaultDescription, false, false, false), Times.Once());
            mockTriageDrawer.Verify(x => x.GetVisualization(), Times.Once());
        }

        [Test]
        public void Draw_NoRulesOnlyResult_RulesSentToDrawer()
        {
            var mockTriageDrawer = new Mock<ITriageDrawer>();
            TriageDrawer.Draw(DeserializeJson("TestFiles\\NoRules.json"), mockTriageDrawer.Object, OutputTypeEnum.HtmlImage);

            string resultDescription = "Based on the amount of damage, it can be assumed to be a total loss.";
            string defaultDescription = "This case can be processed as Default Result.";

            mockTriageDrawer.Verify(x => x.StartSet("Test Customer", "Test Customer Description", OutputTypeEnum.HtmlImage), Times.Once());
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 1"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(It.IsAny<CriteriaEnum?>(), It.IsAny<bool?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddRegExRule(It.IsAny<CriteriaEnum?>(), It.IsAny<string?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddRangeRule(It.IsAny<CriteriaEnum?>(), It.IsAny<double?>(), It.IsAny<double?>()), Times.Never);
            mockTriageDrawer.Verify(x => x.AddResult("TotalLossAssessment", resultDescription, true, true, true, null), Times.Once());
            mockTriageDrawer.Verify(x => x.EndSetResult("DefaultResult", defaultDescription, false, false, false), Times.Once());
            mockTriageDrawer.Verify(x => x.GetVisualization(), Times.Once());
        }

        [Test]
        public void Draw_OneRuleSetWithMoreRules_RulesSentToDrawer()
        {
            var mockTriageDrawer = new Mock<ITriageDrawer>();
            TriageDrawer.Draw(DeserializeJson("TestFiles\\RuleSetWithMoreRules.json"), mockTriageDrawer.Object, OutputTypeEnum.HtmlImage);

            string resultDescription = "Based on the details, it can be assumed to be request information.";
            string defaultDescription = "This case can be processed as Default Result.";

            mockTriageDrawer.Verify(x => x.StartSet("Test Customer", "Test Customer Description", OutputTypeEnum.HtmlImage), Times.Once());
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 1"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(CriteriaEnum.IsCar, true), Times.Once);
            mockTriageDrawer.Verify(x => x.AddRegExRule(CriteriaEnum.DamageType, "Brand|Explo|Ueber"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddRangeRule(CriteriaEnum.CountDamagedParts, 2, 1), Times.Once);
            mockTriageDrawer.Verify(x => x.AddResult("RequestInformation", resultDescription, true, true, false, null), Times.Once());
            mockTriageDrawer.Verify(x => x.EndSetResult("DefaultResult", defaultDescription, false, false, false), Times.Once());
            mockTriageDrawer.Verify(x => x.GetVisualization(), Times.Once());
        }

        [Test]
        public void Draw_MoreRuleSetsWithOneRuleEach_RulesSentToDrawer()
        {
            var mockTriageDrawer = new Mock<ITriageDrawer>();
            TriageDrawer.Draw(DeserializeJson("TestFiles\\MoreRuleSetsWithOneRuleEach.json"), mockTriageDrawer.Object, OutputTypeEnum.HtmlImage);

            string resultDescriptionTotalLoss = "Based on the amount of damage, it can be assumed to be a total loss.";
            string resultDescriptionIsCar = "Regarding the damage to your vehicle, unfortunately we need more information.";
            string defaultDescription = "This case can be processed as Default Result.";

            mockTriageDrawer.Verify(x => x.StartSet("Test Customer", "Test Customer Description", OutputTypeEnum.HtmlImage), Times.Once());
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 1"), Times.Once);
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 2"), Times.Once);
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 3"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(CriteriaEnum.TotalLoss, true), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(CriteriaEnum.IsCar, false), Times.Once);
            mockTriageDrawer.Verify(x => x.AddRegExRule(CriteriaEnum.DamageType, "Brand|Explo|Ueber"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddResult("TotalLossAssessment", resultDescriptionTotalLoss, true, true, true, null), Times.Exactly(2));
            mockTriageDrawer.Verify(x => x.AddResult("OnSiteDamageAssessment", resultDescriptionIsCar, false, false, false, null), Times.Once());
            mockTriageDrawer.Verify(x => x.EndSetResult("DefaultResult", defaultDescription, false, false, false), Times.Once());
            mockTriageDrawer.Verify(x => x.GetVisualization(), Times.Once());
        }

        [Test]
        public void Draw_MoreRuleSetsWithMoreRules_RulesSentToDrawer()
        {
            var mockTriageDrawer = new Mock<ITriageDrawer>();
            TriageDrawer.Draw(DeserializeJson("TestFiles\\MoreRuleSetsWithMoreRules.json"), mockTriageDrawer.Object, OutputTypeEnum.HtmlImage);

            string resultDescriptionTotalLoss = "Based on the amount of damage, it can be assumed to be a total loss.";
            string resultDescriptionIsCar = "Regarding the damage to your vehicle, unfortunately we need more information.";
            string defaultDescription = "This case can be processed as Default Result.";

            mockTriageDrawer.Verify(x => x.StartSet("Test Customer", "Test Customer Description", OutputTypeEnum.HtmlImage), Times.Once());
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 1"), Times.Once);
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 2"), Times.Once);
            mockTriageDrawer.Verify(x => x.StartRuleSet("Ruleset 3"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(CriteriaEnum.TotalLoss, true), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(CriteriaEnum.IsCar, true), Times.Once);
            mockTriageDrawer.Verify(x => x.AddBoolRule(CriteriaEnum.IsComplexDamage, false), Times.Once);
            mockTriageDrawer.Verify(x => x.AddRangeRule(CriteriaEnum.CountDamagedParts, 2, 1), Times.Once);
            mockTriageDrawer.Verify(x => x.AddRegExRule(CriteriaEnum.DamageType, "Brand|Explo|Ueber"), Times.Once);
            mockTriageDrawer.Verify(x => x.AddResult("TotalLossAssessment", resultDescriptionTotalLoss, true, true, true, null), Times.Exactly(2));
            mockTriageDrawer.Verify(x => x.AddResult("OnSiteDamageAssessment", resultDescriptionIsCar, false, false, false, null), Times.Once());
            mockTriageDrawer.Verify(x => x.EndSetResult("DefaultResult", defaultDescription, false, false, false), Times.Once());
            mockTriageDrawer.Verify(x => x.GetVisualization(), Times.Once());
        }

        private static TriageConfig? DeserializeJson(string json)
        {
            string _configAsJSON = File.ReadAllText(json);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<TriageConfig>(_configAsJSON, options);
        }
    }
}
