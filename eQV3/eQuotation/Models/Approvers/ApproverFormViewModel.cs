using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Advantech.Myadvantech.DataAccess;
using System.Web.Mvc;

namespace eQuotation.Models.Approvers
{
    public class ApproverFormViewModel
    {
        
        public string SalesCode { get; set; }

        public string SalesEmail { get; set; }

        public string Sector { get; set; }

        public List<string> SectorList { get; set; }

        public List<SelectListItem> SectorListItem {
            get
            {
                var listItems = this.SectorList.Select(s => new SelectListItem
                {
                    Text = s,
                    Value = s
                }).ToList();


                //if (!string.IsNullOrEmpty(this.Sector))
                //    listItems.FirstOrDefault(l => l.Value == this.Sector).Selected = true;                
                //else
                listItems.First().Selected = true;


                return listItems;
            }
        }

        public string IdSBU { get; set; }

        public List<ACN_EQ_Sales> SalesApprovers { get; set; }


        public bool IsNew { get; set; }
    }
}