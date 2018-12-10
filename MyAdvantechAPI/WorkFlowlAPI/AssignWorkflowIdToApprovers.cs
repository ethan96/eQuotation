using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.Business;

namespace WorkFlowlAPI
{

    public sealed class AssignWorkflowIdToApprovers : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> QuoteID { get; set; }


        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            var workflowId = context.WorkflowInstanceId.ToString();
            GPControlBusinessLogic.AssignWorkFlowIdToApprovers(QuoteID.Get(context),workflowId);
        }
    }
}
