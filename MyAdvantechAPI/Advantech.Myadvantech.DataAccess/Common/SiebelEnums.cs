using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public enum SiebelContactNeverEmail
    {
        Y,
        N
    }
    public enum SiebelActiveSource
    {
        MyAdvantech,
        eQuotation
    }
    public enum SiebelActiveType
    {
        CreateOpportunity,
        CreateQuote,
        UpdateOpportunity,
        CreateActivity
    }
    public enum SiebelActiveStatus
    {
        UnProcess,
        Processing,
        Success,
        Failed
    }
    internal enum SiebelActiveErrorMsg
    {
        Null_Object,
        ActiveSource_is_not_in_enum_or_empty,
        ActiveType_is_not_in_enum_or_empty,
        Status_is_not_in_enum_or_empty,
        QuoteID_is_empty,
        OptyID_is_empty,
        OptyName_is_empty
    }

    public enum SiebelActiveUpdatedUser
    {
        System
    }
}
