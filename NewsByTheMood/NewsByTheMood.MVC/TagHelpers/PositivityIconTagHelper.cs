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
                case 9:
                    icon.AddCssClass("bi bi-emoji-smile");
                    icon.Attributes["style"] = "color: limegreen;";
                    break;

                case 8:
                case 7:
                    icon.AddCssClass("bi bi-emoji-smile");
                    icon.Attributes["style"] = "color: yellowgreen;";
                    break;

                case 6:
                case 5:
                    icon.AddCssClass("bi bi-emoji-neutral");
                    icon.Attributes["style"] = "color: orange;";
                    break;

                case 4:
                case 3:
                    icon.AddCssClass("bi bi-emoji-frown");
                    icon.Attributes["style"] = "color: orangered;";
                    break;

                case 2:
                case 1:
                    icon.AddCssClass("bi bi-emoji-frown");
                    icon.Attributes["style"] = "color: red;";
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
