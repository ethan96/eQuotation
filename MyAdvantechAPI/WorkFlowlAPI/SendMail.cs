using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.DataAccess;
using WorkFlowlAPI.Utility;

namespace WorkFlowlAPI
{

    public sealed class SendMail : NativeActivity
    {
        public InArgument<Boolean> IsCreateBookmark { get; set; }

        public InArgument<string> QuoteID { get; set; }

        public InArgument<string> Region { get; set; }

        public OutArgument<List<WorkFlowApproval>> AdditionalApprovals { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                Advantech.Myadvantech.Business.QuoteBusinessLogic.SendApproveEmail(QuoteID.Get(context), Region.Get(context));
            }
            catch(Exception ex)
            {

                string subject = String.Format("eQ3.0 approval process errors, quote Id: {0}", QuoteID.Get(context));
                string strContent = String.Format("<p> eQ3.0 approval process errors in {0}</p>", DateTime.Now.ToString());
                strContent += String.Format("<p>Quote Id: {0}</p>", QuoteID.Get(context));
                strContent += ex.Message;
                MailHelper.SendMail("myadvantech@advantech.com", subject, strContent);
            }

            if (IsCreateBookmark.Get(context) == true)
                context.CreateBookmark("Waiting for approval", new BookmarkCallback(this.OnBookmarkCallback));
        }

        // NativeActivity derived activities that do asynchronous operations by calling   
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext   
        // must override the CanInduceIdle property and return true.  
        protected override bool CanInduceIdle
        {
            get { return true; }
        }


        public void OnBookmarkCallback(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            AdditionalApprovals.Set(context, (List<WorkFlowApproval>)obj);
        }
    }
}
