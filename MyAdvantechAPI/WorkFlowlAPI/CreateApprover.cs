using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.Business;

namespace WorkFlowlAPI
{

    public sealed class CreateApprover : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> QuoteId { get; set; }
        public InArgument<double> LevelNum { get; set; }
        public InArgument<string> Url { get; set; }
        public InArgument<string> Approver { get; set; }
        public InArgument<string> ApproverType { get; set; }
        public InArgument<bool> ViewGp { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            string quoteId = context.GetValue(this.QuoteId);
            double levelNum = context.GetValue(this.LevelNum);
            string workflowId = context.WorkflowInstanceId.ToString();
            string url = context.GetValue(this.Url);
            string approver = context.GetValue(this.Approver);
            string approverType = context.GetValue(this.ApproverType);
            bool viewGP = context.GetValue(this.ViewGp);
            GPControlBusinessLogic.CreateQuoteApprover(quoteId, levelNum, workflowId, url, approver, approverType, viewGP,null,null);
        }
    }
}
