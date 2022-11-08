using AngleSharp.Html;
using AngleSharp.Html.Parser;
using HandlebarsDotNet;
using System.Text;
using TriageConfiguration.Drawer;
using TriageConfiguration.TriageElements;

namespace TriageConfiguration.TextDrawer
{
    public class HtmlTextTriageDrawer : ITriageDrawer
    {
        private readonly StringBuilder LogString = new();
        private StringBuilder SavedString = new();

        public void StartSet(string? customerName, string? description, OutputTypeEnum? outputType)
        {
            string htmlStartSet =
                @"<!DOCTYPE html>
                <html>
                    <head>
                        <div class=""CustomerAndDesc"">
                        <div align=""center"">
                            <h1>Customer name: {{CustomerName}}</h1>
                            <h1>Description: {{Description}}</h1>
                        </div>
                        </div>
                   </head>";

            var template = Handlebars.Compile(htmlStartSet + "<body>");
            var data = new
            {
                CustomerName = customerName,
                Description = description
            };
            var result = template(data);
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void StartRuleSet(string? description)
        {
            string htmlRuleStartSet =
                @"<div class=""Rule"">
                     <h2>{{Description}}</h2>
                     <h4>Rules: </h4>
                  </div>";

            var template = Handlebars.Compile(htmlRuleStartSet);
            var data = new
            {
                Description = description,
            };
            var result = template(data);
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddBoolRule(CriteriaEnum? criteria, bool? stateValue)
        {
            string htmlBoolRule =
                @"<ul>
                       <li>Criteria: {{Criteria}} is {{StateValue}}</li>
                  </ul>";

            var template = Handlebars.Compile(htmlBoolRule);
            var data = new
            {
                StateValue = stateValue,
                Criteria = criteria
            };
            var result = template(data);
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddRangeRule(CriteriaEnum? criteria, double? maxRange, double? minRange)
        {
            string htmlRangeRule =
                @"<ul>
                        <li>Criteria: {{Criteria}} is between {{MinRange}} and {{MaxRange}}</li>
                  </ul>";

            var template = Handlebars.Compile(htmlRangeRule);
            var data = new
            {
                Criteria = criteria,
                MaxRange = maxRange,
                MinRange = minRange
            };
            var result = template(data);
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddRegExRule(CriteriaEnum? criteria, string? regEx)
        {
            string htmlRegExRule =
                @"<ul>
                       <li>Criteria: {{Criteria}} is {{RegEx}}</li>
                  </ul>";

            var template = Handlebars.Compile(htmlRegExRule);
            var data = new
            {
                Criteria = criteria,
                RegEx = regEx
            };
            var result = template(data);
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public void AddResult(string? name, string? description, bool? repairCostsVisible, bool? residualValueVisible, bool? replacementValueVisible, ActionTypeEnum? actionType)
        {
            string htmlResultName =
                @"<h4>Result: </h4>
                   <ul>
                       <li>Name: {{Name}}</li>
                       <li>Description: {{Description}}</li>
                       <li>RepairCostsVisible: {{repairCostsVisible}}</li>
                       <li>ResidualValueVisible: {{residualValueVisible}}</li>
                       <li>ReplacementValueVisible: {{replacementValueVisible}}</li>
                       <p>{{actionType}}</p>
                   </ul>";

            var template = Handlebars.Compile(htmlResultName);
            var data = new
            {
                Name = name,
                Description = description,
                RepairCostsVisible = repairCostsVisible,
                ResidualValueVisible = residualValueVisible,
                ReplacementValueVisible = replacementValueVisible,
                ActionType = actionType
            };
            var result = template(data);
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }
        public void EndSetResult(string? name, string? description, bool? repairCostsVisible, bool? residualValueVisible, bool? replacementValueVisible)
        {
            string htmlResultName =
                @"<h3>Default Result</h3>
                   <ul>
                       <li>Name: {{Name}}</li>
                       <li>Description: {{Description}}</li>
                       <li>RepairCostsVisible: {{repairCostsVisible}}</li>
                       <li>ResidualValueVisible: {{residualValueVisible}}</li>
                       <li>ReplacementValueVisible: {{replacementValueVisible}}</li>
                   </ul>
                 </body>
                </html>";

            var template = Handlebars.Compile(htmlResultName);
            var data = new
            {
                Name = name,
                Description = description,
                RepairCostsVisible = repairCostsVisible,
                ResidualValueVisible = residualValueVisible,
                ReplacementValueVisible = replacementValueVisible
            };
            var result = template(data);
            SavedString = LogString.Append(result).Append(Environment.NewLine);
        }

        public string GetVisualization()
        {
            if (SavedString.Length > 0)
            {
                var parser = new HtmlParser();
                var document = parser.ParseDocument(SavedString.ToString());
                var writer = new StringWriter();
                document.ToHtml(writer, new PrettyMarkupFormatter());
                var htmlPrettified = writer.ToString();

                return htmlPrettified;
            }
            return string.Empty;
        }
    }
}
