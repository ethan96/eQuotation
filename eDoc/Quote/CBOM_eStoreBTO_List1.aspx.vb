Public Class CBOM_eStoreBTO_List1
    Inherits PageBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request("CATEGORY") IsNot Nothing AndAlso Request("CATEGORY").ToString <> "" Then
            SqlDataSource1.SelectCommand = ""
        Else
            SqlDataSource1.SelectCommand = String.Format(" SELECT distinct CATEGORY3  from ESTORE_BTOS where storeid='{0}'", "AUS")
        End If
    End Sub
    Dim CATEGORY3_str As String = ""
    Protected Function GetData(ByVal obj As Object) As DataTable
        Dim sql As String = "SELECT distinct CATEGORY2  from ESTORE_BTOS where CATEGORY3='" + obj.ToString() + "'"
        Dim dt As DataTable = tbOPBase.dbGetDataTable("B2B", sql)
        dt.Columns.Add(New DataColumn("Parent", GetType(String)))
        For i As Integer = 0 To dt.Rows.Count - 1
            dt.Rows(i).Item("Parent") = obj.ToString.Trim
        Next
        dt.AcceptChanges()
        Return dt
    End Function

    Protected Sub DataList1_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
        CATEGORY3_str = DataList1.DataKeys(e.Item.ItemIndex).ToString().Trim()
    End Sub

End Class