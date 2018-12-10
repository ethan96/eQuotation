using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
  public static  class MyExtension
    {
      public static string BuildIn = "Build In";
      public static bool ContainsV2(this string source, string str)
      {
          return source.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;
      }
    

    }
}
