using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTech.Demo.Utilities.ViewEngine
{
    public abstract class CustomWebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>, IRiddhaViewPage
    {

        private readonly Localizer _localize = NullLocalizer.Instance;
        public Localizer T
        {
            get { return _localize; }
        }

    }
    public static class NullLocalizer
    {
        private static readonly Localizer _instance;
        public static Localizer Instance { get { return _instance; } }
        static NullLocalizer()
        {
            _instance = (format, args) => new LocalizedString((args == null || args.Length == 0) ? format : string.Format(format, args));
        }

    }
}