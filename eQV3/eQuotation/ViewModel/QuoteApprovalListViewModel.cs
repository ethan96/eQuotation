using Advantech.Myadvantech.DataAccess;
using eQuotation.Models.Quotes;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eQuotation.ViewModels
{
    public class QuoteApprovalViewModel
    {
        public QuoteApprovalViewModel()
        {
            this.AppRegion = AppContext.AppRegion;
            this.IsCheckBeforeApproval = false;
        }
        public string AppRegion { get; set; }
        public string QuoteNo { get; set; }
        public List<WorkFlowApproval> QuoteApprovalList { get; set; }
        public string ApprovalReason { get; set; }
        public bool ApprovedCommentIsViewable{ get;set;}
        public bool IsCheckBeforeApproval { get; set; }

        public IEnumerable<string> HiddenApprovers { get; set; }

    }
}