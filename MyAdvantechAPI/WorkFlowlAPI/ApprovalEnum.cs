using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowlAPI
{
    public enum FindApproverResult
    {

        SalesSectorNotFound,
        SalesApproversNotFound,
        GPRuleNotFound,
        NoNeed,
        NeedApproval,
        NeedApprovalForGPAndExpiredDate,
        NeedApprovalForGP,
        NeedApprovalForExpiredDate,
        NegativeGP


    }

    public enum ApprovalResult
    {

        ApprovalBookmarkNotFound,
        WaitingApproverNotFound,
        Finish,
        WaitingForApproval,
        Exception

    }

    public enum ApprovalResultMessage
    {

        Approved,
        Rejected,
        HasBeenApproved,
        HasBeenRejected,
        NotCurrentApprovalLevel

    }

    public enum ApproverType
    {

        Sales,
        PSM,
        OptionalPSM,
        Other
    }
}
