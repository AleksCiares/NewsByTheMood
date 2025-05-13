using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NewsByTheMood.MVC.TagHelpers
{
    /*
        For corrent view and work need to add positivity-icon.css on page
     */
    public class PositivityIconTagHelper : TagHelper
    {
        public required short Positivity { get; set; }
        public bool UseCssTooltip { get; set; } = false;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var icon = new TagBuilder("i");
            switch (Positivity)
            {
                case 10:
                    icon.AddCssClass("bi bi-emoji-surprise");
                    icon.Attributes["style"] = "color: #37f221;";
                    break;
                case 9:
                    icon.AddCssClass("bi bi-emoji-laughing");
                    icon.Attributes["style"] = "color: #37f221;";
                    break;
                case 8:
                    icon.AddCssClass("bi bi-emoji-laughing");
                    icon.Attributes["style"] = "color: #34a30a;";
                    break;
                case 7:
                    icon.AddCssClass("bi bi-emoji-smile");
                    icon.Attributes["style"] = "color: #257a06;";
                    break;
                case 6:
                    icon.AddCssClass("bi bi-emoji-neutral");
                    icon.Attributes["style"] = "color: #f3ca35;";
                    break;
                case 5:
                    icon.AddCssClass("bi bi-emoji-frown");
                    icon.Attributes["style"] = "color: #f4a30b;";
                    break;
                case 4:
                    icon.AddCssClass("bi bi-emoji-astonished");
                    icon.Attributes["style"] = "color: #f46d0b;";
                    break;
                case 3:
                    icon.AddCssClass("bi bi-emoji-grimace");
                    icon.Attributes["style"] = "color: #f4560b;";
                    break;
                case 2:
                case 1:
                    icon.AddCssClass("bi bi-emoji-angry");
                    icon.Attributes["style"] = "color: #c20000;";
                    break;

                default:
                    icon.AddCssClass("bi bi-emoji-tear");
                    icon.Attributes["style"] = "color: antiquewhite;";
                    break;
            }

            var positivityDescription = Positivity switch
            {
                10 => "Very Positive",
                9 => "Positive",
                8 => "Mostly Positive",
                7 => "Somewhat Positive",
                6 => "Neutral",
                5 => "Somewhat Negative",
                4 => "Mostly Negative",
                3 => "Negative",
                2 => "Very Negative",
                1 => "Extremely Negative",
                _ => "Unknown"
            };

            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("positivity-icon", Positivity.ToString());
            output.Content.AppendHtml(icon);

            if (UseCssTooltip)
            {
                output.Attributes.SetAttribute("class", "positivity-icon");
                output.Attributes.SetAttribute("data-tooltip", $"Positivity: {positivityDescription}");
            }
            else
            {
                var script =
                    "<script>" +
                        "document.addEventListener('DOMContentLoaded', function () {" +
                            "var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle=\"tooltip\"]'));" +
                            "var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {" +
                                "return new bootstrap.Tooltip(tooltipTriggerEl);" +
                            "});" +
                        "});" +
                    "</script>";

                output.Attributes.SetAttribute("data-bs-toggle", "tooltip");
                output.Attributes.SetAttribute("data-bs-placement", "top");
                output.Attributes.SetAttribute("title", $"Positivity: {positivityDescription}");
                output.Attributes.SetAttribute("style", "cursor: pointer;");
                output.Content.AppendHtml(script);
            }
        }
    }
}
