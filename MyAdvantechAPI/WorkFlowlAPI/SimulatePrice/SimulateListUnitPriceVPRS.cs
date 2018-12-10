using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.DataAccess;

namespace WorkFlowlAPI.SimulatePrice
{

    public sealed class SimulateListUnitPriceVPRS : CodeActivity
    {
        // Define an activity input argument of type string
        public InOutArgument<QuotationMaster> QuoteMaster { get; set; }

        public InOutArgument<String> ErrorMessage { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            QuotationMaster quoteMaster= context.GetValue(this.QuoteMaster);
            string errMsg = context.GetValue(this.ErrorMessage);
            quoteMaster.SimulateListUnitPriceVPRS(ref errMsg);

            this.ErrorMessage.Set(context, errMsg);

            //Order order = quoteMaster.ConvertQuoteToOrder();

            //Advantech.Myadvantech.DataAccess.SAPDAL.SimulateOrder(ref order, ref errMsg);

            //// Process Simulate Result
            //if (String.IsNullOrEmpty(errMsg))
            //{
            //    foreach (Product p in order.LineItems)
            //    {
            //        // Write price back to quotationdetail's unitprice and ITP
            //        quoteMaster.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
            //                                        d.unitPrice == d.newUnitPrice &&
            //                                        d.line_No == p.LineNumber)
            //                            .ToList()
            //                            .ForEach(c =>
            //                            {
            //                                c.listPrice = p.ListPrice;
            //                                c.unitPrice = p.UnitPrice;
            //                                c.newUnitPrice = p.UnitPrice;
            //                            });

            //        quoteMaster.QuotationDetail.Where(d => d.partNo.Equals(p.PartNumber) &&
            //                    d.unitPrice != d.newUnitPrice &&
            //                    d.line_No == p.LineNumber)
            //        .ToList()
            //        .ForEach(c =>
            //        {
            //            c.listPrice = p.ListPrice;
            //            c.unitPrice = p.UnitPrice;
            //        });
            //    }
            //    quoteMaster.InitializeQuotationDetail();
            //}
        }
    }
}
