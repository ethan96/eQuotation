using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace eQuotationWorkFlow
{

    public sealed class CreateNotify : CodeActivity
    {
        // Define an activity input argument of type string
        public InArgument<string> QuoteID { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            //Todo 
            //string text = context.GetValue(this.Text);
            string _quoteid = context.GetValue(this.QuoteID);
            FlowUti.DoSomething();
        }
    }
}
