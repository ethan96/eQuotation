using Advantech.Myadvantech.Business;
using Advantech.Myadvantech.DataAccess;
using eQuotation.Dtos;
using eQuotation.Models.Approvers;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eQuotation.Controllers
{
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class ApproversController : AppControllerBase
    {
        // GET: Approvers
        [Authorize]
        [AuthorizeInfo("MP01001", "It allows user to view sales approvers.", "Module Approval Process", "MP00000")]
        public ActionResult SalesApproverList()
        {
           
            var salesApprovers = new List<ACN_EQ_Sales> ();

            string sector = UserRoleBusinessLogic.getSectorBySalesEmail(AppContext.UserEmail);

            if (string.IsNullOrEmpty(sector))
                salesApprovers = eQuotationContext.Current.ACN_EQ_Sales.ToList();
            else
                salesApprovers = eQuotationDAL.GetACNSalesApproversBySector(sector);

            return View(salesApprovers);
        }


        // GET: Approvers
        public ActionResult GetSalesApprovers()
        {
            var salesApproverDtos = new List<SalesApproverDto>();
            var salesApprovers = new List<ACN_EQ_Sales>();

            salesApprovers = eQuotationContext.Current.ACN_EQ_Sales.ToList();
            foreach (var group in salesApprovers.GroupBy(item => item.SalesCode))
            {
                var salesApproverDto = new SalesApproverDto();
                salesApproverDto.SalesCode = group.Key;
                salesApproverDto.SalesEmail = group.FirstOrDefault().SalesEmail;
                salesApproverDto.Sector = group.FirstOrDefault().Sector;
                salesApproverDto.IdSBU = group.FirstOrDefault().IdSBU;
                salesApproverDto.Approvers = new List<ApproverDto>();
                foreach (var approver in group.OrderBy(a=> a.Level))
                {
                    if (!string.IsNullOrEmpty(approver.ApproverEmail))
                    {
                        var approverDto = new ApproverDto();
                        approverDto.ApproverEmail = approver.ApproverEmail.Split('@')[0];
                        approverDto.Level = approver.Level == null ? 0 : approver.Level.Value;
                        salesApproverDto.Approvers.Add(approverDto);
                    }

                }
                salesApproverDtos.Add(salesApproverDto);
            }

            return Json(salesApproverDtos, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [AuthorizeInfo("MP01003", "It allows user to delete sales approvers.", "Module Approval Process", "MP00000")]
        [HttpPost]
        public ActionResult DeleteSalesApprovers(string id)
        {
            string msg = "";
            bool result = true;
            result = eQuotationDAL.DeleteACNSalesApproversBySalesCode(id, ref msg);
            return Json(new { succeed = result, err= msg });
        }

        public ActionResult NewSalesApprovers()
        {
           
            var executives = eQuotationContext.Current.ACN_Executive.ToList();

            var sectorGroup = executives.GroupBy(e => e.Sector);
            var model = new ApproverFormViewModel() { SectorList = new List<string>(), SalesApprovers = new List<ACN_EQ_Sales>(), IsNew = true };

            string sector = UserRoleBusinessLogic.getSectorBySalesEmail(AppContext.UserEmail);
            if (!string.IsNullOrEmpty(sector))
                model.SectorList.Add(sector);
            else
                model.SectorList.AddRange(sectorGroup.Select(g=>g.Key));



            foreach (var group in sectorGroup)
            {
                if (model.SectorList.Contains(group.Key))
                {
                    for (var i = 1; i <= group.Max(g => g.Level); i++)
                    {
                        var executive = group.SingleOrDefault(g => g.Level == i);
                        var salesApprover = new ACN_EQ_Sales() { Sector = group.Key, Level = i , ApproverEmail = executive !=null? executive.Email:""};
                        model.SalesApprovers.Add(salesApprover);
                    }
                }

            }





            return PartialView("SalesApproverForm", model);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MP01002", "It allows user to create or update sales approvers.", "Module Approval Process", "MP00000")]
        public ActionResult CreateOrUpdateSalesApprovers(ApproverFormViewModel model)
        {
            string msg = "";
            bool isNewApprovers = false;
            bool result = true;

            //only filter approver with model's sector
            var salesApproversForSector = model.SalesApprovers.Where(s => s.Sector == model.Sector).ToList();
            
            //if (salesApproversForSector.Select(s => s.Id).Any(id => id == 0))
            //    isNewApprovers = true;
            var existedSalesCodes = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesCode == model.SalesCode).ToList();

            //if new create approver and sales code exist, return error message
            if (model.IsNew && existedSalesCodes != null && existedSalesCodes.Count > 0)
            {
                msg = "Sales Code is duplicated!";
                result = false;

            }
            else {

                var acnSalesApprovers = salesApproversForSector.Select(item => new ACN_EQ_Sales()
                {
                    Id = item.Id,
                    SalesCode = model.SalesCode,
                    SalesEmail = model.SalesEmail,
                    IdSBU = model.IdSBU,
                    Sector = model.Sector,
                    ApproverEmail = item.ApproverEmail,
                    Level = item.Level
                }).ToList();

                result = eQuotationDAL.CreateOrUpdateACNSalesApprovers(acnSalesApprovers, ref msg);


            }


            if(result)
                return RedirectToAction("SalesApproverList");
            else
                return Json(new { succeed = result, err = msg });


        }


        public ActionResult EditSalesApprovers(string id)
        {

            var model = new ApproverFormViewModel() { SectorList = new List<string>(), SalesApprovers = new List<ACN_EQ_Sales>(), IsNew = false };
            var existedSalesApprover = eQuotationContext.Current.ACN_EQ_Sales.Where(s => s.SalesCode == id).OrderBy(a => a.Level).ToList();


            model.SalesCode = existedSalesApprover.FirstOrDefault().SalesCode;
            model.SalesEmail = existedSalesApprover.FirstOrDefault().SalesEmail;
            model.Sector = existedSalesApprover.FirstOrDefault().Sector;
            model.IdSBU = existedSalesApprover.FirstOrDefault().IdSBU;
            model.SalesApprovers = existedSalesApprover;
            model.SectorList.Add(model.Sector);

            return PartialView("SalesApproverForm", model);
        }

        // GET: PSM Approvers
        [Authorize]
        [AuthorizeInfo("MP01004", "It allows user to view PSM approvers.", "Module Approval Process", "MP00000")]
        public ActionResult PSMApproverList()
        {

            var PSMApprovers = eQuotationContext.Current.ACN_EQ_PSM.ToList();


            return View(PSMApprovers);
        }

        [Authorize]
        [AuthorizeInfo("MP01006", "It allows user to delete PSM approvers.", "Module Approval Process", "MP00000")]
        [HttpPost]
        public ActionResult DeletePSMApprovers(int id)
        {
            string msg = "";
            bool result = true;
            result = eQuotationDAL.DeleteACNPSMById(id, ref msg);
            return Json(new { succeed = result, err = msg });
        }

        public ActionResult NewPSMApprovers()
        {


            var psm = new ACN_EQ_PSM();

            return PartialView("PSMApproverForm", psm);
        }

        [HttpPost]
        [Authorize]
        [AuthorizeInfo("MP01005", "It allows user to create or update PSM approvers.", "Module Approval Process", "MP00000")]
        public ActionResult CreateOrUpdatePSMApprovers(ACN_EQ_PSM aCN_EQ_PSM)
        {
            string msg = "";
            bool result = true;

            result = eQuotationDAL.CreateOrUpdateACNPSMs(aCN_EQ_PSM, ref msg);


            
            return RedirectToAction("PSMApproverList");

        }

        public ActionResult EditPSMApprovers(int id)
        {

            var existedPSM = eQuotationContext.Current.ACN_EQ_PSM.FirstOrDefault(s => s.Id == id);



            return PartialView("PSMApproverForm", existedPSM);
        }

    }
}