using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess;

namespace WorkFlowlAPI
{

    public sealed class AddApprovalToList : CodeActivity
    {
        // Define an activity input argument of type string
        // Define an activity input argument of type string
        public InArgument<string> QuoteId { get; set; }
        public InArgument<double> LevelNum { get; set; }
        public InArgument<string> Url { get; set; }
        public InArgument<string> Approver { get; set; }
        public InArgument<string> ApproverType { get; set; }

        public InArgument<bool> ViewGp { get; set; }
        public InOutArgument<List<WorkFlowApproval>> ApprovalList { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            var approver = context.GetValue(this.Approver);
            var approverType = context.GetValue(this.ApproverType);
            var quoteId = context.GetValue(this.QuoteId);
            var levelNum = context.GetValue(this.LevelNum);
            var url = context.GetValue(this.Url);
            var viewGP = context.GetValue(this.ViewGp);

            if (!string.IsNullOrEmpty(context.GetValue(this.Approver)))
            { 
                // Obtain the runtime value of the Text input argument
                var approvalList = context.GetValue(this.ApprovalList);

                if(approvalList == null)
                    approvalList = new List<WorkFlowApproval>();

                var existApproval = approvalList
                    .SingleOrDefault(p => p.TypeID == quoteId && p.Approver == approver &&
                                          p.LevelNum == (decimal) levelNum);

                if (existApproval == null)
                {
                    var approval = new WorkFlowApproval();
                    approval.UID = System.Guid.NewGuid().ToString();
                    approval.Approver = approver;
                    approval.ApproverType = approverType;
                    approval.TypeID = quoteId;
                    approval.LevelNum = (decimal)levelNum;
                    approval.Url = url;
                    approval.ViewGP = viewGP ? 1 : 0;
                    approvalList.Add(approval);

                    this.ApprovalList.Set(context, approvalList);
                }
            }
        }
    }
}
