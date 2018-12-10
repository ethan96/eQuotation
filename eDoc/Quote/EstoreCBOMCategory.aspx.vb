Public Class EstoreCBOMCategory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim _storeid As String = "AUS"

            If Role.IsJPAonlineSales() Then _storeid = "AJP"
            If Role.IsTWAonlineSales() Then _storeid = "ATW"
            If Role.IsCNAonlineSales() Then _storeid = "ACN"
            If Role.IsKRAonlineSales() Then _storeid = "AKR"
            If Role.IsHQDCSales() Then _storeid = "ATW"

            SqlDataSource1.SelectCommand = " SELECT distinct CategoryName FROM ESTORE_BTOS_CATEGORY  where storeid ='" & _storeid & "' order by CategoryName "
        End If
    End Sub
    Protected Sub btnConfig_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim obj As Button = CType(sender, Button)
        Dim row As GridViewRow = CType(obj.NamingContainer, GridViewRow)
        Dim CATEGORY_ID As String = AdxGrid1.DataKeys(row.RowIndex).Values(0)
        Dim intQty As Integer = 1
        intQty = CType(row.FindControl("txtQty"), TextBox).Text
        Dim str As String = "~/quote/QuotationDetail.aspx?VIEW=1&BTOITEM=" & CATEGORY_ID & "&QTY=" & intQty & "&UID=" & Request("UID")
        Response.Redirect(str)
    End Sub
End Class