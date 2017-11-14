using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnZipFileForWeb.Models;
using System.Web.Mvc;
using ModelsPageinfo;

namespace UnZipFileForWeb.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(
           this HtmlHelper html,
           Pageinfo pagingInfo,
           Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                    tag.AddCssClass("Selected");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }    
    }
}