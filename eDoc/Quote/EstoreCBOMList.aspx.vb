Public Class EstoreCBOMList
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("CategoryName") Is Nothing OrElse Request("UID") Is Nothing Then
            Response.End()
        End If
        If Not IsPostBack Then
            Dim _storeid As String = "AUS"
            If Role.IsJPAonlineSales() Then _storeid = "AJP"
            If Role.IsTWAonlineSales() Then _storeid = "ATW"
            If Role.IsCNAonlineSales() Then _storeid = "ACN"
            If Role.IsKRAonlineSales() Then _storeid = "AKR"
            If Role.IsHQDCSales() Then _storeid = "ATW"

            'SqlDataSource1.SelectCommand = " SELECT  SProductID,BTONo,DisplayPartno FROM ESTORE_BTOS_CATEGORY  where storeid ='" & _storeid & "' and CategoryName =N'" + Request("CategoryName") + "' order by DisplayPartno"
            Dim sql As New StringBuilder
            sql.AppendLine(" SELECT  a.SProductID,a.BTONo,a.DisplayPartno,b.ProductDesc ")
            sql.AppendLine(" FROM ESTORE_BTOS_CATEGORY  a ")
            sql.AppendLine(" left join eStoreProduction.dbo.PRODUCT b on a.SProductID=b.SProductID and a.StoreID=b.StoreID ")
            sql.AppendLine(" where a.storeid ='" & _storeid & "' and a.CategoryName =N'" & Server.UrlDecode(Request("CategoryName")) & "'  ")
            sql.AppendLine(" order by a.DisplayPartno ")
            SqlDataSource1.SelectCommand = sql.ToString
        End If
    End Sub
    Protected Sub btnConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim CATEGORY_ID As String = AdxGrid1.DataKeys(row.RowIndex).Values(0)
        Dim intQty As Integer = 1
        intQty = CType(row.FindControl("txtQty"), TextBox).Text
        'Dim str As String = "~/quote/QuotationDetail.aspx?VIEW=1&BTOITEM=" & CATEGORY_ID & "&QTY=" & intQty & "&UID=" & Request("UID")
        Dim str As String = "~/quote/Configurator.aspx?BTOITEM=" & CATEGORY_ID & "&QTY=" & intQty & "&UID=" & Request("UID")
        Response.Redirect(str)
    End Sub

End Class