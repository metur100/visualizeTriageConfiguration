using AngleSharp.Html;
using AngleSharp.Html.Parser;
using HandlebarsDotNet;
using System.Reflection;
using System.Text;
using TriageConfiguration.Drawer;
using TriageConfiguration.TriageElements;

namespace TriageConfiguration.ImageDrawer
{
    public class HtmlImageTriageDrawer : ITriageDrawer
    {
        private readonly StringBuilder LogString = new();
        private StringBuilder SavedString = new();

        public void StartSet(string? customerName, string? description, OutputTypeEnum? outputType)
        {
            var styles = CheckStyle(outputType);

            string htmlStartSet =
                @$"<!DOCTYPE html>
                <html>
                <head>
                <style>{styles}</style>";

            string htmlCustomerAndDesc = @"
                   <div align=""center"">
                     <div class=""titleStyle"">
                        <h3>Customer name: {{CustomerName}}</h3>
                     </div>";

            var template = Handlebars.Compile(htmlStartSet + "</head><body>" + htmlCustomerAndDesc);
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
                @"<div class=""diamond"">
                  <div class=""centerCriteria"">";

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
                @"<div class=""rules"">
                     <p>{{Criteria}} = {{StateValue}}</p>
                </div>";

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
                @"<div class=""rules"">
                     <p>{{Criteria}} > {{MinRange}} < {{MaxRange}}</p>
                  </div>";

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
                @"<div class=""rules"">
                      <p>{{Criteria}} = {{RegEx}}</p>
                  </div>";

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
                @"</div>
                    <div class=""arrowDown"">
                  </div>
                  <div class=""no""><p>NO</p></div>
                  </div>
                    <div class=""arrowRight"">
                    </div>
                    <div class=""yes""><p>YES</p></div>
                    <div class=""rect"">
                        <h4>{{Name}}</h4>
                    <div class=""resultDescription"">
                        <p>{{Description}}</p>
                    </div>
                    <div class=""resultBools"">
                    <div id=""inputPreview"">";

            var inputRepairCosts = InputOptionsCheckBox("demo_opt_1", "RepairCosts", repairCostsVisible);
            var inputResidualValue = InputOptionsCheckBox("demo_opt_2", "ResidualValue", residualValueVisible);
            var inputReplacementValue = InputOptionsCheckBox("demo_opt_3", "ReplacementValue", replacementValueVisible);

            var actionResult = AddActionResult(actionType);

            var template = Handlebars.Compile(htmlResultName + inputRepairCosts + inputResidualValue + inputReplacementValue + "</div></div>" + actionResult + "</div>");
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

        public void EndSetResult(string? name, string? description, bool? repairCostsVisible, bool? residualValueVisible, bool? replacementValueVisible)
        {
            string htmlResultName =
                @"<div class=""defaultResultTitle"">
                        <h5>Default Result</h5>
                    </div>
                    <div class=""rectDefault"">
                        <h4>{{Name}}</h4>
                    <div class=""resultDescription"">
                        <p>{{Description}}</p>
                    <div class=""resultDefaultBools"">
                    <div id=""inputPreview"">";

            var inputRepairCosts = InputOptionsCheckBox("demo_opt_1", "RepairCosts", repairCostsVisible);
            var inputResidualValue = InputOptionsCheckBox("demo_opt_2", "ResidualValue", residualValueVisible);
            var inputReplacementValue = InputOptionsCheckBox("demo_opt_3", "ReplacementValue", replacementValueVisible);

            var template = Handlebars.Compile(htmlResultName + inputRepairCosts + inputResidualValue + inputReplacementValue + " </div></div></div></body></html>");
            var data = new
            {
                Name = name,
                Description = description
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

        public static string CheckBoxValue(bool? isChecked)
        {
            if (isChecked == true)
            {
                return "checked";
            }
            return string.Empty;
        }

        public static string InputOptionsCheckBox(string? option, string? lebel, bool? isChecked)
        {
            string htmlCheckBox = $@"
                   <input name = ""cssCheckbox"" id = ""{option}"" 
                       type = ""checkbox"" class=""css-checkbox""{CheckBoxValue(isChecked)}>
                       <label for= ""demo_opt_1"">{lebel}</label>";

            return htmlCheckBox;
        }

        public static string AddActionResult(ActionTypeEnum? actionType)
        {
            if (actionType == ActionTypeEnum.PerformLineByLineCalculation)
            {
                return $@"<div class=""lineByLine"">
                   <input name = ""cssCheckbox"" id = ""demo_opt_1"" 
                       type = ""checkbox"" class=""css-checkbox""checked>
                       <label for= ""demo_opt_1"">LineByLineCalculation</label></div>";
            }
            return string.Empty;
        }

        public static string GetStyles(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream? stream = assembly.GetManifestResourceStream(path);
            StreamReader reader = new(stream!);
            return reader.ReadToEnd();
        }

        public static string CheckStyle(OutputTypeEnum? outputType)
        {
            if(outputType == OutputTypeEnum.PngImage || outputType == OutputTypeEnum.JpgImage)
            {
                return GetStyles("TriageConfiguration.ImageDrawer.CSS.StylesImage.css");
            }
            else
            {
                return GetStyles("TriageConfiguration.ImageDrawer.CSS.Styles.css");
            }
        }
    }
}
