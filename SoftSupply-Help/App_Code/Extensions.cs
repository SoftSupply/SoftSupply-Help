using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{

    public enum Align
    {
        None, Left, Right
    }

    public static class HolderExtensions
    {
        public static MvcHtmlString Holder(this HtmlHelper htmlHelper, string path, int width, int height = -1, string text = "", short size = 50, string font = "", string fg = "", Align align = Align.None, string theme = "", string bg = "", bool outline = false, double lineWrap = 0.0)
        {
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(path)))
                return new MvcHtmlString(path);
            else
            {

                if (height == -1) height = width;

                var result = new StringBuilder();
                result.Append($"holder.js/{width}x{height}");

                var args = new List<string>();
                if (!string.IsNullOrEmpty(text)) args.Add($"text={text}");
                if (size != 50) args.Add($"size={size}");
                if (!string.IsNullOrEmpty(font)) args.Add($"font={font}");
                if (!string.IsNullOrEmpty(fg)) args.Add($"fg={fg}");
                if (align != Align.None) args.Add($"align={Enum.GetName(typeof(Align), align).ToLower()}");

                if (!string.IsNullOrEmpty(theme))
                    if (theme.ToLower() != "random")
                        args.Add($"theme={theme}");
                    else
                        args.Add($"random=yes");
                if (!string.IsNullOrEmpty(bg)) args.Add($"bg={bg}");

                if (outline) args.Add($"outline=yes");
                if (lineWrap != 0.0) args.Add($"lineWrap={bg}");

                if (args.Count > 0)
                    result.Append("?" + string.Join("&", args.ToArray()));

                return new MvcHtmlString(result.ToString());
            }
        }
    }
}