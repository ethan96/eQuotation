Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ACNSPR
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    '<WebMethod()> _
    'Public Function GetCBOMByACNSPRNum(ByVal SPRNumber As String) As DataTable
    '    Dim dt As New DataTable
    '    Dim apt As New SqlClient.SqlDataAdapter( _
    '        " select b.quoteId, b.line_No, b.partNo, b.qty, b.category, IsNull(d.edivision,'') as edivision, IsNull(d.egroup,'') as egroup, d.edesc, acl_pdl, " + _
    '        " IsNull((select top 1 z.product_status from MyAdvantechGlobal.dbo.SAP_PRODUCT_STATUS z where z.PART_NO=b.partNo and z.SALES_ORG='CN10'),'') as PRODUCT_STATUS, c.material_group " + _
    '        " from QuotationMaster a inner join QuotationDetail b on a.quoteId=b.quoteId " + _
    '        " left join MyAdvantechGlobal.dbo.SAP_PRODUCT c on b.partNo=c.PART_NO left join PIS.dbo.EAI_EPRICE_PRODUCT_LINE d on c.PRODUCT_LINE=d.acl_pdl " + _
    '        " where a.customId=@SPRNUM and b.quoteId like 'ACNQ%' order by b.quoteId, b.line_No ", ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
    '    apt.SelectCommand.Parameters.AddWithValue("SPRNUM", SPRNumber)
    '    apt.Fill(dt)
    '    apt.SelectCommand.Connection.Close()
    '    dt.TableName = "SPRNumberConfig"
    '    Return dt
    'End Function

    <WebMethod()> _
    Public Function GetCBOMByACNSPRNum(ByVal SPRNumber As String) As DataTable
        Dim dt As New DataTable
        Dim apt As New SqlClient.SqlDataAdapter( _
            " select b.quoteId, a.quoteNo, a.Revision_Number, b.line_No, b.partNo, b.qty, b.category, IsNull(d.edivision,'') as edivision, IsNull(d.egroup,'') as egroup, d.edesc, acl_pdl, " + _
            " IsNull((select top 1 z.product_status from MyAdvantechGlobal.dbo.SAP_PRODUCT_STATUS z where z.PART_NO=b.partNo and z.SALES_ORG='CN10'),'') as PRODUCT_STATUS, c.material_group " + _
            " from QuotationMaster a inner join QuotationDetail b on a.quoteId=b.quoteId " + _
            " left join MyAdvantechGlobal.dbo.SAP_PRODUCT c on b.partNo=c.PART_NO left join PIS.dbo.EAI_EPRICE_PRODUCT_LINE d on c.PRODUCT_LINE=d.acl_pdl " + _
            " where a.active=1 and a.customId=@SPRNUM and a.quoteNo like 'ACNQ%' order by b.quoteId, b.line_No ", ConfigurationManager.ConnectionStrings("EQ").ConnectionString)
        apt.SelectCommand.Parameters.AddWithValue("SPRNUM", SPRNumber)
        apt.Fill(dt)
        apt.SelectCommand.Connection.Close()
        dt.TableName = "SPRNumberConfig"
        Return dt
    End Function


End Class