using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

/// <summary>
/// Summary description for NewSAPAccountUtil
/// </summary>
public static class NewSAPAccountUtil
{
    public static string[] ATWCFCLeader = new string[] { "sherry.yang@advantech.com.tw", "joyce.chen@advantech.com.tw" };
    public enum UserRole {
        Sales =0,
        CFC =1, 
        OPLeader=2,
        MyAdvIT=3,
        NoOne=4
    }

    public enum ApprovalTransition {
        InitRequest,
        ManagerApprove,
        ManagerReject,
        OPApprove,
        OPReject
    }

    public static NewSAPAccountRequest getReqDetail(string ApplicationId) {
        var apt = new System.Data.SqlClient.SqlDataAdapter(
            @"
            SELECT          TicketId, CreatedBy, AppliedDate, ApprovalManager, isnull(ApprovalOP, '') AS ApprovalOP, ManagerComment, 
                                        isnull(OPComment,'') as OPComment, ManagerApprovalStatus, ManagerApprovalTime, OPApprovalStatus, 
                                        OPApprovalTime, AccountJsonData
            FROM              NEW_SAP_ACCOUNT_APPLICATIONS_HQ
            WHERE          (ApplicationId = @APPID)
            ", System.Configuration.ConfigurationManager.ConnectionStrings["MY_EC2"].ConnectionString);
        apt.SelectCommand.Parameters.AddWithValue("@APPID", ApplicationId);
        var dt = new DataTable();
        apt.Fill(dt);
        apt.SelectCommand.Connection.Close();
        //var list = DataTableToList<NewSAPAccountRequest>(dt);
        var JsonAccountData = dt.Rows[0]["AccountJsonData"].ToString();
        var jsr = new System.Web.Script.Serialization.JavaScriptSerializer();
        NewSAPAccountUtil.NewSAPAccountRequest req =
            jsr.Deserialize<NewSAPAccountUtil.NewSAPAccountRequest>(JsonAccountData);
        //return list[0];
        req.ApprovalManager= dt.Rows[0]["ApprovalManager"].ToString();
        req.ManagerComment = dt.Rows[0]["ManagerComment"].ToString();
        try { req.ManagerApprovalStatus = (NewSAPAccountUtil.NewAccountApprovalStatus)dt.Rows[0]["ManagerApprovalStatus"]; }
        catch (InvalidCastException exp) { req.ManagerApprovalStatus = NewAccountApprovalStatus.Waiting_For_Approval; }        
        if(!string.IsNullOrEmpty(dt.Rows[0]["ManagerApprovalTime"].ToString()))req.ManagerApprovalTime = DateTime.Parse( dt.Rows[0]["ManagerApprovalTime"].ToString());
        req.ApprovalOP = dt.Rows[0]["ApprovalOP"].ToString();
        req.OPComment = dt.Rows[0]["OPComment"].ToString();
        try { req.OPApprovalStatus = (NewSAPAccountUtil.NewAccountApprovalStatus)dt.Rows[0]["OPApprovalStatus"]; }
        catch (InvalidCastException exp) { req.OPApprovalStatus = NewAccountApprovalStatus.Waiting_For_Approval; }
        if (!string.IsNullOrEmpty(dt.Rows[0]["OPApprovalTime"].ToString())) req.OPApprovalTime = DateTime.Parse(dt.Rows[0]["OPApprovalTime"].ToString());
        return req;
    }

    public static ApprovalTransition getReqCurrentApprovalStatus(string ApplicationId) {        
        var AccountReq = getReqDetail(ApplicationId);
        if (AccountReq.ManagerApprovalStatus == NewAccountApprovalStatus.Waiting_For_Approval && AccountReq.OPApprovalStatus == NewAccountApprovalStatus.Waiting_For_Approval) return ApprovalTransition.InitRequest;
        if (AccountReq.ManagerApprovalStatus == NewAccountApprovalStatus.Approved && AccountReq.OPApprovalStatus == NewAccountApprovalStatus.Waiting_For_Approval) return ApprovalTransition.ManagerApprove;
        if (AccountReq.ManagerApprovalStatus == NewAccountApprovalStatus.Rejected || AccountReq.OPApprovalStatus == NewAccountApprovalStatus.Rejected) return ApprovalTransition.OPReject;
        if (AccountReq.ManagerApprovalStatus == NewAccountApprovalStatus.Rejected) return ApprovalTransition.ManagerReject;
        if (AccountReq.OPApprovalStatus == NewAccountApprovalStatus.Approved) return ApprovalTransition.OPApprove;
        return ApprovalTransition.InitRequest;
    }

    public static void SendApprovalEmail(string ApplicationId, ApprovalTransition transision, string SiteUrl) {
        var AccountReq = getReqDetail(ApplicationId);
        var mySmtpClient = new System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPServer"]);
        var htmlMessage = new System.Net.Mail.MailMessage();
        htmlMessage.From = new System.Net.Mail.MailAddress("myadvantech@advantech.com");
        htmlMessage.Bcc.Add("tc.chen@advantech.com.tw");
        htmlMessage.IsBodyHtml = true;
        htmlMessage.Subject = "[MyA SAP Account] ";
        var TicketNo = AccountReq.TicketId;
        var sbMailBody = new System.Text.StringBuilder();
        var OP = AccountReq.ApprovalOP;
        OP = "OPLeader.GBS.ACL@advantech.corp";
        if (AccountReq.SalesOffice == "1100")
        {
            OP = string.Join(",", ATWCFCLeader);
        }
        //OP = "polar.yu@advantech.com.tw";
        var Manager = AccountReq.ApprovalManager;
        //Manager = "tc.chen@advantech.com";
        var CreatedBy = AccountReq.CreatedBy;

        switch (transision) {
            case ApprovalTransition.InitRequest:
                htmlMessage.To.Add(Manager);
                htmlMessage.CC.Add(CreatedBy);
                htmlMessage.Subject += string.Format("{0} requests to create new SAP Account ({1})", CreatedBy, TicketNo);
                sbMailBody.AppendFormat("Dears,<br/>");
                sbMailBody.AppendFormat("Please click <a href='{0}/Admin/NC/NewSAPAccount.aspx?AppId={1}'>here</a> for approval.<br/>Thank you.", SiteUrl,ApplicationId);
                break;
            case ApprovalTransition.ManagerApprove:
                htmlMessage.To.Add(OP);
                htmlMessage.CC.Add(CreatedBy);
                htmlMessage.Subject += string.Format("{0} requests to create new SAP Account ({1})", CreatedBy, TicketNo);
                sbMailBody.AppendFormat("Dear CFC,<br/>");
                sbMailBody.AppendFormat("Please click <a href='{0}/Admin/NC/NewSAPAccount.aspx?AppId={1}'>here</a> for approval.<br/>Thank you.", SiteUrl, ApplicationId);
                break;
            case ApprovalTransition.ManagerReject:
                htmlMessage.To.Add(CreatedBy);
                htmlMessage.CC.Add(Manager);
                htmlMessage.Subject += string.Format("{0} has rejected your new SAP Account request ({1})", Manager, TicketNo);
                sbMailBody.AppendFormat("Dears,<br/>");
                sbMailBody.AppendFormat("Your manager rejected due to:{0}<br/>", AccountReq.ManagerComment);
                sbMailBody.AppendFormat("Please click <a href='{0}/Admin/NC/NewSAPAccount.aspx?AppId={1}'>here</a> to check.<br/>", SiteUrl, ApplicationId);
                break;
            case ApprovalTransition.OPApprove:
                htmlMessage.To.Add(CreatedBy);
                htmlMessage.CC.Add(Manager); htmlMessage.CC.Add(HttpContext.Current.User.Identity.Name);
                htmlMessage.Subject += string.Format("Your new SAP Account has been approved and created ({0})", TicketNo);
                sbMailBody.AppendFormat("Dears,<br/>");
                sbMailBody.AppendFormat("Please click <a href='{0}/Admin/NC/NewSAPAccount.aspx?AppId={1}'>here</a> to check.<br/>Thank you.", SiteUrl, ApplicationId);
                break;
            case ApprovalTransition.OPReject:
                htmlMessage.To.Add(CreatedBy);
                htmlMessage.CC.Add(Manager); htmlMessage.CC.Add(HttpContext.Current.User.Identity.Name);
                htmlMessage.Subject += string.Format("CFC {0} has rejected your new SAP Account request ({0})", OP, TicketNo);
                sbMailBody.AppendFormat("Dears,<br/>");
                sbMailBody.AppendFormat("Please click <a href='{0}/Admin/NC/NewSAPAccount.aspx?AppId={1}'>here</a> to check.<br/>Thank you.", SiteUrl, ApplicationId);
                break;
        }
        //sbMailBody.AppendFormat("<br/>Original Send To is {0}<br/>", htmlMessage.To[0].Address);
        //htmlMessage.To[0] = new System.Net.Mail.MailAddress("tc.chen@advantech.com.tw");
        htmlMessage.Body = sbMailBody.ToString();        
        mySmtpClient.Send(htmlMessage);
    }

    public static string getApprovalStatus(string ApplicationId)
    {
        var AccountReq = getReqDetail(ApplicationId);
        NewSAPAccountUtil.NewAccountApprovalStatus mstat = (NewSAPAccountUtil.NewAccountApprovalStatus)AccountReq.ManagerApprovalStatus;
        NewSAPAccountUtil.NewAccountApprovalStatus opstat = (NewSAPAccountUtil.NewAccountApprovalStatus)AccountReq.OPApprovalStatus;
        var Mgr = AccountReq.ApprovalManager;
        var ManagerComment = AccountReq.ManagerComment;
        var OP = AccountReq.ApprovalOP;
        var OPComment = AccountReq.OPComment;
        if (mstat == NewSAPAccountUtil.NewAccountApprovalStatus.Waiting_For_Approval)
            return string.Format("waiting for {0}'s approval", Mgr);
        if (mstat == NewSAPAccountUtil.NewAccountApprovalStatus.Rejected)
            return string.Format("Rejected by {0}, comment:{1}", Mgr, ManagerComment);
        if (mstat == NewSAPAccountUtil.NewAccountApprovalStatus.Approved && opstat == NewSAPAccountUtil.NewAccountApprovalStatus.Waiting_For_Approval)
            return string.Format("Approved by {0} with comment:{1}, waiting for CFC leader's approval", Mgr, ManagerComment);
        if (mstat == NewSAPAccountUtil.NewAccountApprovalStatus.Approved && opstat == NewSAPAccountUtil.NewAccountApprovalStatus.Rejected)
            return string.Format("Rejected by CFC {0}, comment:{1}", OP, OPComment);
        return string.Format("Approved by {0}, comment:{1}, then approved by {2}, comment:{3}", Mgr,ManagerComment, OP, OPComment);
    }   

    public static UserRole getCurrentUserRole() {
        
        var uid = HttpContext.Current.User.Identity.Name.ToLower();
        var strSQL = @"select c.GROUP_NAME as Name 
                    from AD_MEMBER a (nolock) left join AD_MEMBER_ALIAS b (nolock) on a.PrimarySmtpAddress=b.EMAIL 
                    inner join AD_MEMBER_GROUP c (nolock) on a.PrimarySmtpAddress=c.EMAIL 
                    where (a.PrimarySmtpAddress=@UID or b.ALIAS_EMAIL=@UID) 
                    and c.GROUP_NAME in ('GBS.ACL','MyAdvantech','OPLeader.GBS.ACL','InterCon.SALES')";
        var MyApt = new System.Data.SqlClient.SqlDataAdapter(strSQL, System.Configuration.ConfigurationManager.ConnectionStrings["MY"].ConnectionString);
        MyApt.SelectCommand.Parameters.AddWithValue("@UID", uid);
        var dtGroups = new DataTable();
        MyApt.Fill(dtGroups);
        MyApt.SelectCommand.Connection.Close();
        if (dtGroups.Select("Name='MyAdvantech'").Length > 0) return UserRole.MyAdvIT;
        if (dtGroups.Select("Name='OPLeader.GBS.ACL'").Length > 0 || ATWCFCLeader.Contains(uid)) return UserRole.OPLeader;
        if (dtGroups.Select("Name='GBS.ACL'").Length > 0) return UserRole.CFC;        
        if (dtGroups.Select("Name='InterCon.SALES'").Length > 0) return UserRole.Sales;
        MyApt.SelectCommand.CommandText = "select id_sap, id_email from eai_idmap (nolock) where id_email=@UID";
        var dtSales = new DataTable();
        MyApt.SelectCommand.Connection.Open(); MyApt.Fill(dtSales); MyApt.SelectCommand.Connection.Close();
        if (dtSales.Rows.Count > 0) return UserRole.Sales;
        return UserRole.NoOne;
    }

    public class NewSAPAccountRequest
    {
        public string ApplicationId { get; set; }
        public string TicketId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime AppliedDate { get; set; }
        public string ApprovalManager { get; set; }
        public string ManagerComment { get; set; }
        public NewAccountApprovalStatus ManagerApprovalStatus { get; set; }
        public DateTime ManagerApprovalTime { get; set; }
        public string ApprovalOP { get; set; }
        public string OPComment { get; set; }
        public NewAccountApprovalStatus OPApprovalStatus { get; set; }
        public DateTime OPApprovalTime { get; set; }
        public string AccountGroup { get; set; }
        public string SalesOrg { get; set; }
        public string SalesOffice { get; set; }
        public string SalesGroup { get; set; }
        public string VECode { get; set; }
        public string Z2Code { get; set; }
        public string ZVCode { get; set; }
        public string ERCode { get; set; }
        public string KUNNR { get; set; }
        public string Link2SoldToId { get; set; }
        public string CompanyName { get; set; }
        public string SiebelAccountId { get; set; }
        public string Comment { get; set; }
        public string SearchTerm1 { get; set; }
        public string SearchTerm2 { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Telephone { get; set; }
        public string TelExt { get; set; }
        public string FaxNo { get; set; }
        public string ContactFName { get; set; }
        public string ContactLName { get; set; }
        public string ContactEmail { get; set; }
        public string OfficeRegNo { get; set; }
        public string DUNSNo { get; set; }
        public string DBPayIdx { get; set; }
        public string VATNo { get; set; }
        public string WebsiteURL { get; set; }
        public string ShipCond { get; set; }
        public string PayTerm { get; set; }
        public bool IsSpecCreditLimit { get; set; }
        public string CreditLimitCurr { get; set; }
        public decimal CreditLimitAmt { get; set; }
        public string CreditLimitRiskCat { get; set; }
        public string CreditLimitRepGrp { get; set; }
        public string IncoTerm { get; set; }
        public string IncoText { get; set; }
        public string Industry { get; set; }
        public string IndustryCode1 { get; set; }
        public string IndustryCode2 { get; set; }
        public string CustGroup { get; set; }
        public string MWST_TaxCode { get; set; }
        public string UTXJ_TaxCode { get; set; }
        public string AccountAssignGroup { get; set; }
        public string CustClass { get; set; }
        public string PriceGrade1 { get; set; }
        public string PriceGrade2 { get; set; }
        public string PriceGrade3 { get; set; }
        public string PriceGrade4 { get; set; }
        public string PriceGroup { get; set; }
        public string Currency { get; set; }
        public string AdditionalCustAttr1 { get; set; }
        public string AdditionalCustAttr9 { get; set; }
        public List<string> FileDocIDs { get; set; }        
        public NewSAPAccountRequest()
        {
            FileDocIDs = new List<string>(); AppliedDate = DateTime.Now;
            ManagerApprovalStatus = NewAccountApprovalStatus.Waiting_For_Approval;
            OPApprovalStatus = NewAccountApprovalStatus.Waiting_For_Approval;
        }
    }
   
    public enum NewAccountApprovalStatus
    {
        Waiting_For_Approval = 0,
        Approved = 1,
        Rejected = 2,
        Need_More_Info = 3
    }

    public static List<TResult> DataTableToList<TResult>(this DataTable DataTableValue) where TResult : class, new()
    {
        //建立一個回傳用的 List<TResult>
        List<TResult> Result_List = new List<TResult>();

        //取得映射型別
        Type type = typeof(TResult);

        //儲存 DataTable 的欄位名稱
        List<PropertyInfo> pr_List = new List<PropertyInfo>();

        foreach (PropertyInfo item in type.GetProperties())
        {
            if (DataTableValue.Columns.IndexOf(item.Name) != -1)
                pr_List.Add(item);
        }

        //足筆將 DataTable 的值新增到 List<TResult> 中
        foreach (DataRow item in DataTableValue.Rows)
        {
            TResult tr = new TResult();

            foreach (PropertyInfo item1 in pr_List)
            {
                if (item[item1.Name] != DBNull.Value)
                    item1.SetValue(tr, item[item1.Name], null);
            }

            Result_List.Add(tr);
        }

        return Result_List;
    }
}