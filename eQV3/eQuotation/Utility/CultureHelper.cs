
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Web;

namespace eQuotation.Utility
{
    public static class CultureHelper
    {

        public static string GetValueByKey(this ResourceManager resourceManager, string key, CultureInfo cultureInfo, bool ignoreCase = false)
        {

            var comparisonType = ignoreCase ? System.StringComparison.OrdinalIgnoreCase : System.StringComparison.Ordinal;
            var entry = resourceManager.GetResourceSet(cultureInfo, true, true)
                                       .OfType<DictionaryEntry>()
                                       .FirstOrDefault(dictionaryEntry => dictionaryEntry.Key.ToString().Equals(key, comparisonType));

            if (entry.Key == null)
                return string.Empty;
            else
                return entry.Value.ToString();
        }
    }
}