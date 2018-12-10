Public Class CBOM_eStoreBTO_List3
    Inherits PageBase

    Public CATEGORY3_title As String = "", CATEGORY2_title As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
  
        If Request("CATEGORY2") IsNot Nothing AndAlso Request("CATEGORY3") IsNot Nothing Then
            SqlDataSource1.SelectCommand = "SELECT distinct CATEGORY1,'" + Request("CATEGORY2").ToString.Trim + "' as CATEGORY2,'" + Request("CATEGORY3").ToString.Trim + "' as CATEGORY3 from ESTORE_BTOS where CATEGORY2='" + Request("CATEGORY2").ToString.Trim + "' AND CATEGORY3='" + Request("CATEGORY3").ToString.Trim + "' and storeid='AUS'"
            CATEGORY3_title = Request("CATEGORY3").ToString.Trim
            CATEGORY2_title = Request("CATEGORY2").ToString.Trim
        End If
    End Sub
    Protected Function GetData(ByVal obj As Object) As DataTable
        Dim sql As String = "SELECT  distinct top 2 * from ESTORE_BTOS where CATEGORY2='" + Request("CATEGORY2").ToString.Trim + "' AND CATEGORY3='" + Request("CATEGORY3").ToString.Trim + "'  AND CATEGORY1='" + obj.ToString() + "' and storeid='AUS'"
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", sql)
        Return dt
    End Function
    Protected Function GetURL(ByVal obj As Object) As String
        If obj Is Nothing Then Return ""
        Dim Url As String = HttpUtility.UrlEncode(obj.ToString.Trim)
        Return Url
    End Function
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim item As RepeaterItem = CType(CType(sender, Button).NamingContainer, RepeaterItem)
        'Dim i As Integer = item.ItemIndex
        Dim partno As String = CType(sender, Button).CommandArgument
        ' Dim DISPLAYPARTNO As DataRowView = CType(item.DataItem, DataRowView)
        Response.Redirect("./QuotationDetail.aspx?VIEW=1&BTOITEM=" + partno + "&QTY=1&UID=" + Request("UID") + "")
    End Sub

End Class