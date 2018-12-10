using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advantech.Myadvantech.DataAccess;

namespace WorkFlowlAPI
{
    public sealed class SendMailV2 : NativeActivity
    {
        public InArgument<Boolean> IsCreateBookmark { get; set; }
        public OutArgument<Boolean> NeedTaxAdjustion { get; set; }

        public OutArgument<List<WorkFlowApproval>> AdditionalApprovals { get; set;}

        public InArgument<string> QuoteID { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            Advantech.Myadvantech.Business.QuoteBusinessLogic.SendApproveEmail(QuoteID.Get(context));
            if (IsCreateBookmark.Get(context) == true)
                context.CreateBookmark("Waiting for CM's approval", new BookmarkCallback(this.OnBookmarkCallback));
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

            //this.Name2.Set(context, (string)obj);
            // When the Bookmark is resumed, assign its value to
            // the Result argument.
            var test = (List<WorkFlowApproval>)obj;
            //NeedTaxAdjustion.Set(context, (bool)obj);
            AdditionalApprovals.Set(context, (List<WorkFlowApproval>) obj);
            
        }
    }
}
