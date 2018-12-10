using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace WorkFlowlAPI
{

    public sealed class CheckApprovalDetail :  NativeActivity
    {
        // Define an activity input argument of type string
        public InArgument<Boolean> IsCreateBookmark { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            BookmarkScope scope = new BookmarkScope(context.WorkflowInstanceId);


            if (IsCreateBookmark.Get(context) == true)
                context.CreateBookmark("Waiting for check approval detail", new BookmarkCallback(this.OnBookmarkCallback));
        }


        // NativeActivity derived activities that do asynchronous operations by calling   
        // one of the CreateBookmark overloads defined on System.Activities.NativeActivityContext   
        // must override the CanInduceIdle property and return true.  

        //CanInduceIdle to return the true meaning that this activity can cause the workflow to become idle
        protected override bool CanInduceIdle
        {
            get { return true; }
        }

        public void OnBookmarkCallback(NativeActivityContext context, Bookmark bookmark, object obj)
        { 
            //this.Name2.Set(context, (string)obj);
            // When the Bookmark is resumed, assign its value to
            // the Result argument.
            //Result.Set(context, (string)obj);
        }

    }
}
