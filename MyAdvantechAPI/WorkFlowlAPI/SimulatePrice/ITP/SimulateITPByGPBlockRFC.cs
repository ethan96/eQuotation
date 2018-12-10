﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Advantech.Myadvantech.DataAccess;

namespace WorkFlowlAPI.SimulatePrice
{

    public sealed class SimulateITPByGPBlockRFC : CodeActivity
    {
        // Define an activity input argument of type string
        public InOutArgument<QuotationMaster> QuoteMaster { get; set; }

        public InOutArgument<String> ErrorMessage { get; set; }

        // If your activity returns a value, derive from CodeActivity<TResult>
        // and return the value from the Execute method.
        protected override void Execute(CodeActivityContext context)
        {
            // Obtain the runtime value of the Text input argument
            QuotationMaster quoteMaster = context.GetValue(this.QuoteMaster);
            string errMsg = context.GetValue(this.ErrorMessage);
            quoteMaster.SimulateITPBySAPGPBlockRFC(ref errMsg);
            this.ErrorMessage.Set(context, errMsg);

        }
    }
}