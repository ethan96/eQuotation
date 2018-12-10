using Advantech.Myadvantech.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Advantech.Myadvantech.Business
{
    public class MyAdminBusinessLogic
    {
        public static SA_APPLICATION getApplicationByID(int AppID)
        {
            MyAdminDAL _MyAdminDAL = new MyAdminDAL();
            return _MyAdminDAL.getApplicationByID(AppID);
        }
        public static SA_APPLICATION getApplicationByWFInstanceID(String WFInstanceID)
        {
            MyAdminDAL _MyAdminDAL = new MyAdminDAL();
            return _MyAdminDAL.getApplicationByWFInstanceID(WFInstanceID);
        }

        public static object getAllApplication2Company()
        {
            MyAdminDAL _MyAdminDAL = new MyAdminDAL();
            return _MyAdminDAL.getAllApplication2Company();
        }

        public static string GetSTATUS(string status)
        {
            return Enum.GetName(typeof(AccountWorkFlowStatus), int.Parse(status));
        }
        public static List<SA_Proposal> getProposal(int AppID)
        {
            MyAdminDAL _MyAdminDAL = new MyAdminDAL();
            List<SA_Proposal> pls=_MyAdminDAL.getProposal(AppID);
            return pls;
        }

    }
}
