using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Utility
{
    public enum LogLevel
    {
        [EnumDisplay("Error")]
        Error,

        [EnumDisplay("Warning")]
        Warning,

        [EnumDisplay("Info")]
        Info,

        [EnumDisplay("Debug")]
        Debug,

        [EnumDisplay("Fatal")]
        Fatal
    }


}