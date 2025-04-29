using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NewsByTheMood.MVC.TagHelpers
{
    public class SectionDividerTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var style = @"
                <style>
                    .section-divider {
                        display: flex;
                        align-items: center;
                        margin: 2rem 0;
                        position: relative;
                    }
                    .section-divider::before,
                    .section-divider::after {
                        content: '';
                        flex: 1;
                        height: 1px;
                        background-color: #ccc;
                    }
                    .section-divider .section-title {
                        margin: 0 1rem;
                        font-size: 1.25rem;
                        font-weight: bold;
                        color: #555;
                        background-color: #fff;
                        padding: 0 0.5rem;
                        white-space: nowrap;
                    }
                </style>";

            var span = new TagBuilder("span");
            span.AddCssClass("section-title");
            span.InnerHtml.Append(output.GetChildContentAsync().Result.GetContent());

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("section-divider", "");
            output.Attributes.Add("class", "section-divider");
            output.Content.SetHtmlContent(span);
            output.Content.AppendHtml(style);
        }
    }
}
