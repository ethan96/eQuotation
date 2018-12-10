using Advantech.Myadvantech.DataAccess;
using eQuotation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Models.Approvers
{
    public class ApproverListViewModel
    {
        public string SalesCode { get; set; }
        public string Email { get; set; }
        public string Sector { get; set; }

        public List<ACN_EQ_Sales> Approvers { get; set; }


    }


}