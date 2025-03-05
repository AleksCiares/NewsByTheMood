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

            //var paginationWrap = new TagBuilder("nav");
            var pagList = new TagBuilder("ul");
            pagList.AddCssClass("pagination");

            for (int i = 1; i <= this.PageInfo.TotalPages; i++)
            {
                var pagElem = new TagBuilder("li");
                pagElem.AddCssClass("page-item");
                if (PageInfo.Page == i)
                {
                    pagElem.AddCssClass("active");
                }

                var pagLink = new TagBuilder("a");
                pagLink.AddCssClass("page-link");
                pagLink.Attributes["href"] = urlHelper.Action(PageAction, new { page = i })?.ToLower();
                pagLink.InnerHtml.AppendHtml(i.ToString());

                pagElem.InnerHtml.AppendHtml(pagLink);

                pagList.InnerHtml.AppendHtml(pagElem);
            }

            output.TagName = "nav";
            //output.AddClass("pagination", HtmlEncoder.Default);
            //output.TagMode = TagMode.SelfClosing;
            output.Content.AppendHtml(pagList);
        }
    }
}
