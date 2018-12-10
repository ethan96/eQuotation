using Advantech.Myadvantech.Business;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowlAPI
{

    public sealed class UpdateApprovalsWorkFlowId : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> QuoteId { get; set; }


        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {


            // Obtain the runtime value of the Text input argument
            string quoteId = context.GetValue(this.QuoteId);
            string workflowId = context.WorkflowInstanceId.ToString();

            if (quoteId != null && workflowId !=null)
            {
                GPControlBusinessLogic.UpdateQuoteWaitApprovalWorkflowId(quoteId, workflowId);
            }
        }
    }
}
