using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NewsByTheMood.MVC.TagHelpers
{
    public class DynamicTextAreaTagHelper : TextAreaTagHelper
    {
        public required string tagSelector { get; set; }
        public required int rows { get; set; }
        public int? maxRows { get; set; }

        public DynamicTextAreaTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var script = $@"
                <script>
                    document.addEventListener('DOMContentLoaded', function () {{
                        var textarea = document.querySelector('[text-area-selector=""{tagSelector}""]');
                        if (textarea) {{
                            textarea.style.height = 'auto';
                            textarea.style.height = (textarea.scrollHeight) + 'px';
                            textarea.addEventListener('input', function() {{
                                var scrollTop = this.scrollTop;
                                this.style.height = 'auto';
                                this.style.height = (this.scrollHeight) + 'px';
                                this.scrollTop = scrollTop;
                                var maxRows = {maxRows ?? 0};
                                if (maxRows > 0) {{
                                    var lineHeight = parseFloat(window.getComputedStyle(this).lineHeight);
                                    var maxHeight = lineHeight * maxRows;
                                    if (this.scrollHeight > maxHeight) {{
                                        this.style.height = maxHeight + 'px';
                                        this.value = this.value.slice(0, this.value.lastIndexOf('\\n'));
                                    }}
                                }}
                            }});
                        }}
                    }});
                </script>";

            output.TagName = "textarea";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("text-area-selector", tagSelector);
            output.Attributes.Add("style", "resize: none; overflow: hidden;");
            output.Attributes.Add("rows", rows.ToString());
            if (maxRows.HasValue)
            {
                output.Attributes.Add("data-max-rows", maxRows.Value.ToString());
            }
            output.PostElement.AppendHtml(script);
        }
    }
}
