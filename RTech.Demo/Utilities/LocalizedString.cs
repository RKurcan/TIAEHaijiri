using RTech.Demo.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace RTech.Demo.Utilities
{
    public delegate LocalizedString Localizer(string text, params object[] args);
    public class LocalizedString : MarshalByRefObject, IHtmlString
    {
        private readonly string _localized;
        private readonly object[] _args;
        public LocalizedString()
        {
        }
        public LocalizedString(string localized)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            _localized = Resource.ResourceManager.GetString(localized, culture);
            _localized = _localized == null ? localized : _localized;


        }

        public string ToHtmlString()
        {
            return _localized;
        }
        public string Localize(string text)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            if (text == null)
            {
                return String.Empty;
            }
            string _loc = Resource.ResourceManager.GetString(text, culture);
            return _loc == null ? text : _loc;

        }
    }
}