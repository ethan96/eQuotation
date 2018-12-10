using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.DataAccess;
using WorkFlowlAPI.Utility;
using Advantech.Myadvantech.Business;

namespace WorkFlowlAPI
{

    public sealed class SendFinalMail : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> QuoteId { get; set; }

        public InArgument<string> Region { get; set; }


        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {

            
            try
            {
                Advantech.Myadvantech.Business.QuoteBusinessLogic.SendFinalApprovalEmail(QuoteId.Get(context), Region.Get(context));
            }
            catch (Exception ex)
            {

                string subject = String.Format("eQ3.0 approval process errors, quote Id: {0}", QuoteId.Get(context));
                string strContent = String.Format("<p> eQ3.0 approval process errors in {0}</p>", DateTime.Now.ToString());
                strContent += String.Format("<p>Quote Id: {0}</p>", QuoteId.Get(context));
                strContent += ex.Message;
                MailHelper.SendMail("myadvantech@advantech.com", subject, strContent);
            }

        }
    }
}
