using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models;

namespace Web.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static HtmlString PageLinks(this IHtmlHelper html,
            PaginInfo paginInfo, Func<int, string> pageUrl)
        {
            var result = new System.IO.StringWriter();
            for (int i = 1; i <= paginInfo.TotalPages; i++)
            {
                var tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", pageUrl(i));
                tagBuilder.InnerHtml.Append(i.ToString());
                
                if (i == paginInfo.CurrentPage)
                {
                    tagBuilder.AddCssClass("selected");    
                    tagBuilder.AddCssClass("btn-primary");    
                }

                tagBuilder.AddCssClass("btn btn-default");
                tagBuilder.WriteTo(result, HtmlEncoder.Default);
            }
            return new HtmlString(result.ToString());
        }
    }
}