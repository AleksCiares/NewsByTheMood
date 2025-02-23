using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NewsByTheMood.MVC.Models;

namespace NewsByTheMood.MVC.TagHelpers
{
    // Tag helper for pagination of elements on page
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        public required PageInfoModel PageInfo { get; set; }
        public required string PageAction { get; set; }
        [ViewContext]
        [HtmlAttributeNotBound]
        public required ViewContext ViewContext { get; set; }

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this._urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = this._urlHelperFactory.GetUrlHelper(ViewContext);
            var mainTag = new TagBuilder("div");
            mainTag.AddCssClass("btn-group");

            for (int i = 1; i <= this.PageInfo.TotalPages; i++)
            {
                var innerTag = new TagBuilder("a");
                innerTag.AddCssClass("btn");

                if (PageInfo.Page == i) innerTag.AddCssClass("active");
                innerTag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i })?.ToLower();
                innerTag.InnerHtml.AppendHtml(i.ToString());

                mainTag.InnerHtml.AppendHtml(innerTag);
            }

            output.TagName = "div";
            output.AddClass("pagination", HtmlEncoder.Default);
            //output.TagMode = TagMode.SelfClosing;
            output.Content.AppendHtml(mainTag);
        }
    }
}
