using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.DataAccess;

namespace WorkFlowlAPI.SimulatePrice
{

    public sealed class GetACNOSPartITP : CodeActivity
    {
        // Define an activity input argument of type string
        public InOutArgument<List<QuotationDetail>> QuotationDetails { get; set; }

        public InArgument<string> ERPId { get; set; }

        public InOutArgument<String> ErrorMessage { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            List<QuotationDetail> quoteDetails = context.GetValue(this.QuotationDetails);
            string ERPId = context.GetValue(this.ERPId);
            string errMsg = context.GetValue(this.ErrorMessage);

            try
            {
                Advantech.Myadvantech.Business.QuoteBusinessLogic.GetACNOSPartITP(quoteDetails, ERPId);
                //if (quoteDetails != null)
                //{
                //    foreach (var item in quoteDetails)
                //    {
                //        var newItp = Advantech.Myadvantech.DataAccess.SAPDAL.GetACNOSPartITP(item.partNo, ERPId);
                //        if (newItp > 0)
                //        {
                //            item.itp = newItp;
                //            item.newItp = newItp;
                //        }
                //    }
                //}
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
            }
            this.ErrorMessage.Set(context, errMsg);


            //quoteMaster.InitializeQuotationDetail();

        }
    }
}
