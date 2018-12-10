using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eQuotation.Dtos
{
    public class SalesApproverDto
    {
        public string SalesCode { get; set; }

        public string SalesEmail { get; set; }
        
        public string Sector { get; set; }

        public string IdSBU { get; set; }

        public List<ApproverDto> Approvers { get; set; }
    }
}