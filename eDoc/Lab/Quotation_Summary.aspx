<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Quotation_Summary.aspx.vb" Inherits="EDOC.Quotation_Summary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:EQ %>" SelectCommand="select T.quoteNo,T.CompanyID,T.CompanyName,T.salesEmail,T.currency,SUM(T.Total_Amount)as Total_Amount,T.GP1,T.GP_Manager1,T.First_Approval,T.GP2,T.GP_Manager2,T.Second_Approval,T.quoteDate from(
select distinct quoteNo,quoteToErpId as CompanyID,quoteToName as CompanyName,salesEmail,qm.currency,qd.Total_Amount
,isnull(qa1.GPlever,'') as GP1,isnull(qa1.Approver,'') as GP_Manager1
,isnull((case qa1.status when 1 then 'Approved' when 2 then 'Approved' when 3 then 'Approved' when 0 then 'Pending' when -1 then 'Rejected'  end),'') as First_Approval
,isnull(qa2.GPlever,'') as GP2,isnull(qa2.Approver,'') as GP_Manager2
,isnull((case qa2.status when 1 then 'Approved' when 2 then 'Approved' when 3 then 'Approved' when 0 then 'Pending' when -1 then 'Rejected'  end),'') as Second_Approval
,quoteDate 
from QuotationMaster qm
left join [quotation_approval] qa1 on qm.quoteId = qa1.Quote_ID and qa1.Approval_level=1
left join [quotation_approval] qa2 on qm.quoteId = qa2.Quote_ID and qa2.Approval_level=2
inner join QUOTE_TO_ORDER_LOG qt on qm.quoteId = qt.QUOTEID
inner join MyAdvantechGlobal.dbo.SAP_ORDER_HISTORY soh on qt.SO_NO = soh.SO_NO 
left join (select (newUnitPrice * qty) as Total_Amount,quoteid from QuotationDetail) qd on qm.quoteId = qd.quoteId
where qm.SALESGROUP =321 and salesEmail &lt;&gt; '' and YEAR(quoteDate) =2013 
) T
group by T.quoteNo,T.CompanyID,T.CompanyName,T.salesEmail,T.currency,T.GP1,T.GP_Manager1,T.First_Approval,T.GP2,T.GP_Manager2,T.Second_Approval,T.quoteDate
order by T.salesEmail"></asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
