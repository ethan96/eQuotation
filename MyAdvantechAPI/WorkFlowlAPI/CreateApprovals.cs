using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess;

namespace WorkFlowlAPI
{

    public sealed class CreateApprovals : CodeActivity
    {
        // Define an activity input argument of type string


        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {

            // Obtain the runtime value of the Text input argument

            var waitingApprovals = context.GetExtension<List<WorkFlowApproval>>();



            if (waitingApprovals != null)
            {
                foreach (var approval in waitingApprovals)
                {
                    GPControlBusinessLogic.CreateQuoteApprover(
                        approval.TypeID,
                        (double)approval.LevelNum,
                        context.WorkflowInstanceId.ToString(),
                        approval.Url,
                        approval.Approver,
                        approval.ApproverType,
                        Convert.ToBoolean(approval.ViewGP), approval.Mailbody, approval.FinalMailBody);
                }
            }
        }
    }
}
