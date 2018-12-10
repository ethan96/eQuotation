using Advantech.Myadvantech.DataAccess;
using eQuotation.Entities;
using eQuotation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Models.Quotes
{
    public class PSMApproverViewModel
    {
        public PSMApproverViewModel()
        {
            ProductDivisionGroups = new List<ProductDivisionGroupModel>();
        }
        public List<ProductDivisionGroupModel> ProductDivisionGroups { get; set; }

        public List<WorkFlowApproval> CurrentApprovals { get; set; }

        public string SalesCode { get; set; }
        public string SalesEmail { get; set; }

    }

    public class ProductDivisionGroupModel
    {

        public string Name { get; set; }
        public List<ACN_EQ_PSM> PSMs { get; set; }
        //public List<ACN_EQ_PSM> PSMLeaders { get; set; }
        public bool Selected { get; set; }
    }


}