using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NewsByTheMood.MVC.TagHelpers
{
    public class SelectWithSearchTagHelper : SelectTagHelper
    {
        public required string SelectSelector { get; set; }
        public string? Placeholder { get; set; }
        public string? Title { get; set; }

        public SelectWithSearchTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var wrapper = new TagBuilder("div");
            wrapper.Attributes.Add("select-with-search-wrapper", SelectSelector);
            wrapper.AddCssClass("select-with-search-wrapper");

            var title = new TagBuilder("span");
            title.AddCssClass("text-muted");
            title.InnerHtml.Append(Title);

            var searchInput = new TagBuilder("input");
            searchInput.Attributes.Add("type", "text");
            searchInput.Attributes.Add("placeholder", Placeholder);
            searchInput.Attributes.Add("select-with-search-input", SelectSelector);
            searchInput.AddCssClass("select-with-search-input");
            searchInput.AddCssClass("form-control");

            output.TagName = "select";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("select-with-search", SelectSelector);
            output.AddClass("select-with-search", HtmlEncoder.Default);
            output.AddClass("form-select", HtmlEncoder.Default);

            var style = @"
                <style>
                    .select-with-search-wrapper {
                        display: flex;
                        flex-direction: column;
                        margin: 0 auto;
                        width: 100%;
                        border: 1px solid #ced4da;
                        border-radius: 4px;
                        transition: border-color 0.3s ease;
                    }
                    .select-with-search-wrapper:focus {
                        border-color: #80bdff;
                        outline: none;
                        box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
                    }
                    .select-with-search-wrapper span{
                        padding: 8px 12px;
                    }
                    .select-with-search-input{
                        border: 1px solid #ced4da;
                        border-radius: 4px 4px 0 0;
                        border-right: none !important;
                        border-left: none !important;
                    }
                    .select-with-search{
                        padding: 8px 12px;
                        border: none;
                        box-shadow: none;
                        overflow-y: scroll;
                        scrollbar-width: none; 
                    }
                    .select-with-search::-webkit-scrollbar {
                        display: none;
                    }
                    .select-with-search-input:focus, 
                    .select-with-search:focus{
                        box-shadow: none;
                    }
                </style>";
            var script = $@"
                <script>
                    document.addEventListener('DOMContentLoaded', function () {{
	                    var tagSearch = document.querySelector('[select-with-search-input=""{SelectSelector}""]');
	                    var tagSelect = document.querySelector('[select-with-search=""{SelectSelector}""]');
	                    tagSearch.addEventListener('input', function () {{
		                    var filter = tagSearch.value.toLowerCase();
		                    var options = tagSelect.options;
	                        for (var i = 0; i < options.length; i++) {{
		                        var option = options[i];
		                        var text = option.text.toLowerCase();

		                        if (text.includes(filter)) {{
			                        option.style.display = '';
		                        }} else {{
			                        option.style.display = 'none';
		                        }}
		                    }}
	                    }});
                    }});
                </script>";

            output.PreElement.AppendHtml(wrapper.RenderStartTag());
            output.PreElement.AppendHtml(title);
            output.PreElement.AppendHtml(searchInput);
            output.PostElement.AppendHtml(style);
            output.PostElement.AppendHtml(script);
            output.PostElement.AppendHtml(wrapper.RenderEndTag());
        }
    }
}
