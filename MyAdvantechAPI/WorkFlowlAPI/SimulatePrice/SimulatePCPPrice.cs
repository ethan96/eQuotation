using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.DataAccess;

namespace WorkFlowlAPI.SimulatePrice
{

    public sealed class SimulatePCPPrice : CodeActivity
    {
        // Define an activity input argument of type string
        public InOutArgument<QuotationMaster> QuoteMaster { get; set; }
        public InArgument<string> ERPId { get; set; }

        public InArgument<string> Org { get; set; }
        public InOutArgument<String> ErrorMessage { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            QuotationMaster quoteMaster= context.GetValue(this.QuoteMaster);
            string ERPId = context.GetValue(this.ERPId);
            string org = context.GetValue(this.Org);
            string errMsg = context.GetValue(this.ErrorMessage);

            quoteMaster.SimulatePCPPrice(org, ERPId, ref errMsg);
            this.ErrorMessage.Set(context, errMsg);
            //Order order = quoteMaster.ConvertQuoteToOrder(org,ERPId);

            //Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);
            //if (String.IsNullOrEmpty(errMsg))
            //{
            //    foreach (Product p in order.LineItems)
            //    {
            //        // Write price back to quotationdetail's ACN PCPPrice
            //        quoteMaster.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
            //                                        d.line_No == p.LineNumber)
            //                            .ToList()
            //                            .ForEach(c =>
            //                            {
            //                                c.ACN_PCPPrice = p.UnitPrice;
            //                            });
            //    }
            //}
        }
    }
}
