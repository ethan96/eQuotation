using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.DataAccess
{
    public enum  companyType
    {
        SholdTo,
        ShipTo,
        BillTo
    }
    public enum AccountWorkFlowStatus
    {
        NewRequest,
        NotifyCM,
        Approved,
        Reject
    }
}
