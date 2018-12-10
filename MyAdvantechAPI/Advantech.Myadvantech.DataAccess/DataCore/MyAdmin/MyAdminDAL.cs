using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
[assembly: InternalsVisibleTo("Advantech.Myadvantech.Business")]
namespace Advantech.Myadvantech.DataAccess
{
    public class MyAdminDAL
    {
        public SA_APPLICATION getApplicationByID(int AppID)
        {
            SA_APPLICATION _SA_APPLICATION = MyAdminContext.Current.SA_APPLICATION.Find(AppID);
            return _SA_APPLICATION;
        }
        public SA_APPLICATION getApplicationByWFInstanceID(String WFInstanceID)
        {
            SA_APPLICATION _SA_APPLICATION = MyAdminContext.Current.SA_APPLICATION.FirstOrDefault(p=>p.WFInstanceID ==WFInstanceID);
            return _SA_APPLICATION;
        }

        public object getAllApplication2Company()
        {
            var result = MyAdminContext.Current.SA_APPLICATION2COMPANY.Where(p=>p.CompanyType==(Int32)companyType.SholdTo).OrderByDescending(p => p.SA_APPLICATION.REQUEST_DATE).Select(p => new 
            {
                ApplicationID = p.SA_APPLICATION.ID,
                ApplicationNo = p.SA_APPLICATION.AplicationNO,
                CompanyName = p.SA_BAPIADDR1.FirstOrDefault().Name,
                CompanyID = p.CompanyID,
                Status = p.SA_APPLICATION.STATUS,
                RequestBy = p.SA_APPLICATION.REQUEST_BY,
                RequestDate = p.SA_APPLICATION.REQUEST_DATE
            });
            return result.Where(p => p.RequestBy != "ming.zhao@advantech.com.cn").ToList();
        }
        public List<SA_Proposal> getProposal(int AppID)
        {
            List<SA_Proposal> pls = MyAdminContext.Current.SA_Proposal.Where(p => p.AppID == AppID).OrderBy(p => p.CreateTime).ToList();
            return pls;
        }
    }
}
