using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.WebPages;

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

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, object htmlAttributes)
        {
            return Button(helper, innerHtml, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Button(this HtmlHelper helper, string innerHtml, IDictionary<string, object> htmlAttributes)
        {
            var builder = new TagBuilder("button");
            builder.InnerHtml = innerHtml;
            builder.MergeAttributes(htmlAttributes);
            return MvcHtmlString.Create(builder.ToString());
        }

        #region Readability

        public static string RedableLong(this HtmlHelper htmlHelper, double size, string format = "")
        {
            var result = string.Empty;
            if (size >= 100000)
                result = htmlHelper.RedableLong(size / 1000) + "K";
            else if (size >= 10000)
                result = (size / 1000D).ToString("0.#") + "K";
            else
                result = size.ToString("#,0");

            result = result.Replace("KK", "M");

            if (!string.IsNullOrEmpty(format) && !string.IsNullOrEmpty(result))
                return string.Format(format, result);
            else
                return result;
        }

        public static string RedableLong(this HtmlHelper htmlHelper, long size, string format = "")
        {
            var result = string.Empty;
            if (size >= 100000)
                result = htmlHelper.RedableLong(size / 1000) + "K";
            else if (size >= 10000)
                result = (size / 1000D).ToString("0.#") + "K";
            else
                result = size.ToString("#,0");

            result = result.Replace("KK", "M");

            if (!string.IsNullOrEmpty(format) && !string.IsNullOrEmpty(result))
                return string.Format(format, result);
            else
                return result;
        }

        public static string RedableSize(this HtmlHelper htmlHelper, int size, string format = "")
        {
            return RedableSize(htmlHelper, (double)size, format);
        }

        public static string RedableSize(this HtmlHelper htmlHelper, double size, string format = "")
        {

            var result = string.Empty;
            if (size > 1024)
                result = Math.Round(size / 1024, 0) + " Mb.";
            else if (size > 0)
                result = Math.Round(size, 0) + " Kb.";

            if (!string.IsNullOrEmpty(format) && !string.IsNullOrEmpty(result))
                return string.Format(format, result);
            else
                return result;

        }

        public static string RedableDays(this HtmlHelper htmlHelper, int size, string format = "")
        {
            return RedableDays(htmlHelper, (double)size, format);
        }

        public static string RedableDays(this HtmlHelper htmlHelper, double size, string format = "")
        {
            var result = string.Empty;
            double value = 0;
            size = Math.Abs(size);

            if (size >= 365)
            {
                value = Math.Round(size / 365, 0);
                result = value + " " + (value == 1 ? "año" : "años");
            }
            else if (size >= 30)
            {
                value = Math.Round(size / 30, 0);
                result = value + " " + (value == 1 ? "mes" : "meses");
            }
            else
                result = size + " " + (size == 1 ? "día" : "días");

            if (!string.IsNullOrEmpty(format) && !string.IsNullOrEmpty(result))
                return string.Format(format, result);
            else
                return result;

        }

        #endregion

    }

    public static class ViewPageExtensions
    {
        private const string SCRIPTBLOCK_BUILDER = "ScriptBlockBuilder";

        public static MvcHtmlString ScriptBlock(this WebViewPage webPage, Func<dynamic, HelperResult> template)
        {
            return Block(webPage, template, SCRIPTBLOCK_BUILDER);
        }

        public static MvcHtmlString WriteScriptBlocks(this WebViewPage webPage)
        {
            return WriteBlocks(webPage, SCRIPTBLOCK_BUILDER);
        }

        public static MvcHtmlString Block(this WebViewPage webPage, Func<dynamic, HelperResult> template, string block)
        {
            if (!webPage.IsAjax)
            {
                var blockBuilder = webPage.Context.Items[block]
                                    as StringBuilder ?? new StringBuilder();

                blockBuilder.Append(template(null).ToHtmlString());

                webPage.Context.Items[block] = blockBuilder;

                return new MvcHtmlString(string.Empty);
            }
            return new MvcHtmlString(template(null).ToHtmlString());
        }

        public static MvcHtmlString WriteBlocks(this WebViewPage webPage, string block)
        {
            var blockBuilder = webPage.Context.Items[block]
                                as StringBuilder ?? new StringBuilder();

            return new MvcHtmlString(blockBuilder.ToString());
        }
    }
}

namespace System.Web.RequestHelpers
{

    public interface IParamSerializer
    {
        string Serialize(object obj);

        object Deserialize(string input);

        T Deserialize<T>(string input);

        object Deserialize(NameValueCollection input);

        T Deserialize<T>(NameValueCollection input);
    }

    public class ParamSerializer : IParamSerializer
    {
        /// <summary>
        /// Serialize an array of form elements or a set of key/values into a query string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            var paramString = this.Parametrize(JObject.FromObject(obj));
            return paramString.TrimEnd(new[] { '&' });
        }

        public object Deserialize(NameValueCollection input)
        {
            return this.Deserialize(this.ConvertNameValueCollection(input));
        }

        public T Deserialize<T>(NameValueCollection input)
        {
            return this.Deserialize<T>(this.ConvertNameValueCollection(input));
        }

        public object Deserialize(string input)
        {
            return this.Deparametrize(input).ToObject<object>();
        }

        public T Deserialize<T>(string input)
        {
            return this.Deparametrize(input).ToObject<T>();
        }

        /// <summary>
        /// Translation of jquery-deparam
        /// https://github.com/chrissrogers/jquery-deparam/blob/master/jquery-deparam.js
        /// </summary>
        private JObject Deparametrize(string input)
        {
            var obj = new JObject();

            var items = input.Replace("+", " ").Split(new[] { '&' });

            // Iterate over all name=value pairs.
            foreach (string item in items)
            {
                var param = item.Split(new[] { '=' });
                var key = HttpUtility.UrlDecode(param[0]);
                if (!string.IsNullOrEmpty(key))
                {
                    // If key is more complex than 'foo', like 'a[]' or 'a[b][c]', split it
                    // into its component parts.
                    var keys = key.Split(new[] { "][" }, StringSplitOptions.RemoveEmptyEntries);
                    var keysLast = keys.Length - 1;

                    // If the first keys part contains [ and the last ends with ], then []
                    // are correctly balanced.
                    if (Regex.IsMatch(keys[0], @"\[") && Regex.IsMatch(keys[keysLast], @"\]$"))
                    {
                        // Remove the trailing ] from the last keys part.
                        keys[keysLast] = Regex.Replace(keys[keysLast], @"\]$", string.Empty);

                        // Split first keys part into two parts on the [ and add them back onto
                        // the beginning of the keys array.
                        keys = keys[0].Split(new[] { '[' }).Concat(keys.Skip(1)).ToArray();
                        keysLast = keys.Length - 1;
                    }
                    else
                    {
                        // Basic 'foo' style key.
                        keysLast = 0;
                    }

                    // Are we dealing with a name=value pair, or just a name?
                    if (param.Length == 2)
                    {
                        var val = HttpUtility.UrlDecode(param[1]);

                        // Coerce values.
                        // Convert val to int, double, bool, string
                        if (keysLast != 0)
                        {
                            // Complex key, build deep object structure based on a few rules:
                            // * The 'cur' pointer starts at the object top-level.
                            // * [] = array push (n is set to array length), [n] = array if n is 
                            //   numeric, otherwise object.
                            // * If at the last keys part, set the value.
                            // * For each keys part, if the current level is undefined create an
                            //   object or array based on the type of the next keys part.
                            // * Move the 'cur' pointer to the next level.
                            // * Rinse & repeat.
                            object cur = obj;
                            for (var i = 0; i <= keysLast; i++)
                            {
                                int index = -1, nextindex;

                                // Array 'a[]' or 'a[1]', 'a[2]'
                                key = keys[i];

                                if (key == string.Empty || int.TryParse(key, out index))
                                {
                                    key = index == -1 ? "0" : index.ToString(CultureInfo.InvariantCulture);
                                }

                                if (cur is JArray)
                                {
                                    var jarr = cur as JArray;
                                    if (i == keysLast)
                                    {
                                        if (index >= 0 && index < jarr.Count)
                                        {
                                            jarr[index] = val;
                                        }
                                        else
                                        {
                                            jarr.Add(val);
                                        }
                                    }
                                    else
                                    {
                                        if (index < 0 || index >= jarr.Count)
                                        {
                                            if (keys[i + 1] == string.Empty || int.TryParse(keys[i + 1], out nextindex))
                                            {
                                                jarr.Add(new JArray());
                                            }
                                            else
                                            {
                                                jarr.Add(new JObject());
                                            }

                                            index = jarr.Count - 1;
                                        }

                                        cur = jarr.ElementAt(index);
                                    }
                                }
                                else if (cur is JObject)
                                {
                                    var jobj = cur as JObject;
                                    if (i == keysLast)
                                    {
                                        jobj[key] = val;
                                    }
                                    else
                                    {
                                        if (jobj[key] == null)
                                        {
                                            if (keys[i + 1] == string.Empty || int.TryParse(keys[i + 1], out nextindex))
                                            {
                                                jobj.Add(key, new JArray());
                                            }
                                            else
                                            {
                                                jobj.Add(key, new JObject());
                                            }
                                        }

                                        cur = jobj[key];
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Simple key, even simpler rules, since only scalars and shallow
                            // arrays are allowed.
                            if (obj[key] is JArray)
                            {
                                // val is already an array, so push on the next value.
                                (obj[key] as JArray).Add(val);
                            }
                            else if (obj[key] != null && val != null)
                            {
                                // val isn't an array, but since a second value has been specified,
                                // convert val into an array.
                                obj[key] = new JArray { obj[key], val };
                            }
                            else
                            {
                                // val is a scalar.
                                obj[key] = val;
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(key))
                    {
                        // No value was defined, so set something meaningful.
                        obj[key] = null;
                    }
                }
            }

            return obj;
        }

        private string Parametrize(object obj, string value = "")
        {
            var returnVal = string.Empty;

            if (obj is JObject)
            {
                var jobj = obj as JObject;
                foreach (var key in jobj.Properties())
                {
                    returnVal += this.Parametrize(
                        jobj[key.Name], value == string.Empty ? key.Name : string.Format("{0}[{1}]", value, key.Name));
                }
            }
            else if (obj is JArray)
            {
                var arr = obj as JArray;
                for (int i = 0; i < arr.Count; i++)
                {
                    var item = arr[i];
                    if (item is JArray || item is JObject)
                    {
                        returnVal += this.Parametrize(item, string.Format("{0}[{1}]", value, i));
                    }
                    else
                    {
                        returnVal += this.Parametrize(item, string.Format("{0}[]", value));
                    }
                }
            }
            else
            {
                return string.Format("{0}={1}&", value, obj);
            }

            return returnVal;
        }

        private string ConvertNameValueCollection(NameValueCollection input)
        {
            var output = new StringBuilder();
            foreach (var key in input.AllKeys)
            {
                var values = input.GetValues(key) ?? new string[] { };
                foreach (var value in values)
                {
                    output.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value));
                }
            }

            return output.ToString().TrimEnd(new[] { '&' });
        }
    }
}

namespace Newtonsoft.Json.Serialization
{
    public class LowerCasePropertyNamesContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }

}
