using Advantech.Myadvantech.Business;
using eQuotation.Models.Quotes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.ViewModels
{
    public class ViewDownloadForwardQuoteViewModel
    {
        public ViewDownloadForwardQuoteViewModel(string quoteId, string quoteNo, string org)
        {
            this.QuoteId = quoteId;
            this.QuoteNo = quoteNo;
            this.Org = org;
            this.CanClickPdfAndEmail = true;
            if (org.StartsWith("CN"))
            {
                List<SelectListItem> AdvantechCompanyTitleOptions = new List<SelectListItem>();
                AdvantechCompanyTitleOptions.Add(new SelectListItem() { Text = "北京", Value = "CN10", Selected = org == "CN10" });
                AdvantechCompanyTitleOptions.Add(new SelectListItem() { Text = "上海(CN30)", Value = "CN30", Selected = org == "CN30" });
                AdvantechCompanyTitleOptions.Add(new SelectListItem() { Text = "上海(CN70)", Value = "CN70", Selected = org == "CN70" });
                this.AdvantechCompanyOptions = AdvantechCompanyTitleOptions;
            }
            else
                this.AdvantechCompanyOptions = new List<SelectListItem>();

            var defaultQuoteTitleOptions = QuoteBusinessLogic.GetQuoteTitleOptions(AppContext.AppRegion);
            if (defaultQuoteTitleOptions.Any())
            {
                this.QuoteTitleOptions = defaultQuoteTitleOptions
                                .Select(x => new SelectListItem() { Text = x, Value = x })
                                .ToList();
            }

        }


        public string QuoteId { get; set; }
        public string QuoteNo{ get; set; }
        public string Org { get; set; }
        public string QuoteTitle { get; set; }
        public string AdvantechCompanyTitleOrg { get; set; }
        public List<SelectListItem> AdvantechCompanyOptions { get; set; }
        public List<SelectListItem> QuoteTitleOptions { get; set; }
        public DisplayItemOptionsForBto DisplayItemType { get; set; }

        public bool CanClickPdfAndEmail { get; set; }

        public bool AdvantechCompanyTitleIsViewable
        {
            get
            {
                if (AppContext.AppRegion == "ACN")
                    return true;
                return false;
            }
        }
        public bool DisplayItemOptionsForBtoIsViewable { get; set; }

        public bool QuoteTitleOptionsIsViewable {
            get
            {
                if (AppContext.AppRegion == "ASG")
                    return true;
                return false;
            }
        }
    }

    public class SendEmailViewModel
    {
        public string QuoteId { get; set; }
        public string QuoteNo{ get; set; }
        public string QuoteTitle { get; set; }
        public string AdvantechCompanyTitleOrg { get; set; }
        public DisplayItemOptionsForBto DisplayItemType { get; set; }



        public string CustomerMailSubject { get; set; }

        [Required]
        public string ReciverEmail { get; set; }

        public string MailSubject { get; set; }

        public string MailBody { get; set; }

        public FileType FileType { get; set; }
    }

    public enum DisplayItemOptionsForBto
    {
        MainItemOnly,
        SubItemWithPrice,
        SubItemWithoutPrice,
        SubItemWithoutPriceButShowLooseItemPrice
    }

    public enum FileType
    {
        HTML,
        PDF
    }
}