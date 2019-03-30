using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace System
{
    public static class WebFormat
    {
        public static string HtmlEncode(this string s) =>
            WebUtility.HtmlEncode(s);

        public static string UrlEncode(this string s) =>
            WebUtility.UrlEncode(s);

        public static string HtmlEncodeExcept(this string s, string tag) =>
            s.Replace($"<{tag}>", $"*$*{tag}*$*")
            .Replace($"</{tag}>", $"*$*/{tag}*$*")
            .HtmlEncode()
            .Replace($"*$*{tag}*$*", $"<{tag}>")
            .Replace($"*$*/{tag}*$*", $"</{tag}>");
    }
}
