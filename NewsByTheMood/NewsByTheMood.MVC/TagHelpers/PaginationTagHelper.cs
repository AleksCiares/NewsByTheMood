﻿using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NewsByTheMood.MVC.Models;

namespace NewsByTheMood.MVC.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        public PageInfoModel PageInfo { get; set; }
        public string PageAction { get; set; }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            this._urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = this._urlHelperFactory.GetUrlHelper(ViewContext);
            var tag = new TagBuilder("div");
            tag.AddCssClass("btn-group");

            for (int i = 1; i <= this.PageInfo.TotalPages; i++)
            {
                var innerTag = new TagBuilder("a");
                innerTag.AddCssClass("btn");
                var innerTagHtml = i.ToString();


                if (int.TryParse(this.ViewContext.HttpContext.Request.Query["page"], out var currentPage))
                {
                    if (currentPage == i) innerTag.AddCssClass("active");
                }
                else
                {
                    if(i == 1)  innerTag.AddCssClass("active");
                }

                innerTag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                innerTag.InnerHtml.AppendHtml(innerTagHtml);
                tag.InnerHtml.AppendHtml(innerTag);
            }

            output.TagName = "div";
            output.AddClass("pagination", HtmlEncoder.Default);
            //output.TagMode = TagMode.SelfClosing;
            output.Content.AppendHtml(tag);
        }
    }
}
